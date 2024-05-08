using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public static PlayerInteractor Instance { get; private set; }

    public event Action<IInteractable, bool> OnInteractablesChanged;

    [SerializeField] private float interactRadius = 5f;

    private SphereCollider sphereCollider;
    private List<IInteractable> currentInteractables;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentInteractables = new List<IInteractable>();

        CreateOverLapCollider();

        InteractionUi.Instance.OnPickUpBtnPerformed += InteractionUi_Instance_OnPickUpBtnPerformed;
    }

    private void InteractionUi_Instance_OnPickUpBtnPerformed(object sender, EventArgs e)
    {
        IInteractable closestCollectable = GetClosestInteractable();
        closestCollectable.Interact();
        currentInteractables.Remove(closestCollectable);
        HandleInteractableChanges();
    }

    private IInteractable GetClosestInteractable()
    {
        IInteractable closestCollectable = null;

        foreach (var collectables in currentInteractables)
        {
            if (closestCollectable == null)
            {
                closestCollectable = collectables;
            }
            else
            {
                if (Vector3.Distance(transform.position, collectables.GetPosition())
                    < Vector3.Distance(transform.position, closestCollectable.GetPosition()))
                {
                    closestCollectable = collectables;
                }
            }
        }

        return closestCollectable;
    }

    private void CreateOverLapCollider()
    {
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = interactRadius;
        sphereCollider.isTrigger = true;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.TryGetComponent(out IInteractable interactable))
    //    {
    //        interactable.SetActiveSelectedVisual(true);
    //        currentInteractables.Add(interactable);

    //        OnObjectDetected(interactable);
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            if (!currentInteractables.Contains(interactable))
            {
                currentInteractables.Add(interactable);
                interactable.SetActiveSelectedVisual(true);

                HandleInteractableChanges();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.SetActiveSelectedVisual(false);
            currentInteractables.Remove(interactable);

            HandleInteractableChanges();
        }
    }

    private void HandleInteractableChanges()
    {


        if (currentInteractables.Count == 0)
            OnInteractablesChanged?.Invoke(null, false);
        else
            OnInteractablesChanged?.Invoke(currentInteractables.Last(), true);

    }

    private void OnDrawGizmos()
    {
        if (sphereCollider != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
        }
    }
}

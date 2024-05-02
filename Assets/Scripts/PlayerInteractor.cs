using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public static PlayerInteractor Instance { get; private set; }

    public event Action<IInteractable> OnObjectDetected;
    public event Action<IInteractable> OnObjectMissed;

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
        currentInteractables[0].Interact();
        currentInteractables.RemoveAt(0);
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
            if(!currentInteractables.Contains(interactable))
            {
                currentInteractables.Add(interactable);
                OnObjectDetected(interactable);
                interactable.SetActiveSelectedVisual(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.SetActiveSelectedVisual(false);
            currentInteractables.Remove(interactable);

            OnObjectMissed(null);
        }
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

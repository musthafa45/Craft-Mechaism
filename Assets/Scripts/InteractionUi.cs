using System;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUi : MonoBehaviour
{
    public static InteractionUi Instance { get; private set; }

    public event EventHandler OnPickUpBtnPerformed;
    public event EventHandler OnCrafterInteractBtnPerformed;

    [SerializeField] private Button pickUpButton,interactButton;

    private void Awake()
    {
        Instance = this;

        pickUpButton.onClick.AddListener(() =>
        {
            if(HasItemToPickInScene())
                 OnPickUpBtnPerformed?.Invoke(this, EventArgs.Empty);
            else
                SetActiveInteractButton(false);
        });

        interactButton.onClick.AddListener(() =>
        {
            OnCrafterInteractBtnPerformed?.Invoke(this, EventArgs.Empty);

            SetActiveInteractButton(false);
        });

        SetActiveInteractButton(false);
        SetActivePickUpButton(false);
    }

    private bool HasItemToPickInScene()
    {
        return FindObjectsOfType<Item>().Length > 0;
    }

    private void Update()
    {
        if (pickUpButton.gameObject.activeSelf ||
            interactButton.gameObject.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void SetActivePickUpButton(bool active)
    {
        pickUpButton.gameObject.SetActive(active);
    }

    private void SetActiveInteractButton(bool active)
    {
        interactButton.gameObject.SetActive(active);
    }

    private void Start()
    {
        PlayerInteractor.Instance.OnObjectDetected += PlayerInteractor_Instance_OnObjectDetected;
        PlayerInteractor.Instance.OnObjectMissed += PlayerInteractor_Instance_OnObjectMissed;
    }


    private void PlayerInteractor_Instance_OnObjectDetected(IInteractable interactable)
    {
        switch(interactable)
        {
            case CraftingSystem:
                SetActiveInteractButton(true);
                break;
            case Item:
                SetActivePickUpButton(true);
                break;
        }
      
    }

    private void PlayerInteractor_Instance_OnObjectMissed(IInteractable interactable)
    {
        SetActiveInteractButton(false);
        SetActivePickUpButton(false);
    }
}

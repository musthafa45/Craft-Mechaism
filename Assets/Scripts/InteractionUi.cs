using System;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUi : MonoBehaviour
{
    public static InteractionUi Instance { get; private set; }

    public event EventHandler OnPickUpBtnPerformed;
    public event EventHandler OnCrafterInteractBtnPerformed;

    [SerializeField] private Button pickUpButton, interactButton;

    private void Awake()
    {
        Instance = this;

        pickUpButton.onClick.AddListener(() => {
            if (HasItemToPickInScene())
                OnPickUpBtnPerformed?.Invoke(this, EventArgs.Empty);
            else
                SetActiveInteractButton(false);
        });

        interactButton.onClick.AddListener(() => {
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
        PlayerInteractor.Instance.OnInteractablesChanged += HandleInteractionUiStatus;
    }


    private void HandleInteractionUiStatus(IInteractable interactable, bool status)
    {
        switch (interactable)
        {
            case CraftBench:
                SetActiveInteractButton(status);
                SetActivePickUpButton(!status);
                break;
            case Item:
                SetActiveInteractButton(!status);
                SetActivePickUpButton(status);
                break;
            default:
                DisableAllInteractionUi(interactable);
                break;
        }

    }

    private void DisableAllInteractionUi(IInteractable interactable)
    {
        SetActiveInteractButton(false);
        SetActivePickUpButton(false);
    }
}

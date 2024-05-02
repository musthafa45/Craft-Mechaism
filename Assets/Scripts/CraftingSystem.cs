using UnityEngine;

public class CraftingSystem : MonoBehaviour, IInteractable
{
    [SerializeField] private Outline outline;

    private void Awake()
    {
        SetActiveOutLine(false);
    }
    public void Interact()
    {
        Debug.Log("Crafting System Interated");
    }

    public void SetActiveSelectedVisual(bool active)
    {
        SetActiveOutLine(active);
    }
    private void SetActiveOutLine(bool active)
    {
        outline.enabled = active;
    }
}

using System;
using UnityEngine;

public class Item : MonoBehaviour,IInteractable
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private Outline outline;

    public static event Predicate<ItemSO> OnItemTryingToPick;
    private void Awake()
    {
        SetActiveOutLine(false);
    }
    public void SetActiveSelectedVisual(bool active)
    {
        //Debug.Log(active ? $"{itemSO.ItemName} Is Selected" : "Is Not Selected");
       SetActiveOutLine(active);
    }

    private void SetActiveOutLine(bool active)
    {
        outline.enabled  = active;
    }

    public void Interact()
    {
        bool onItemPicked = OnItemTryingToPick(itemSO);
        if (onItemPicked)
        {
            Destroy(gameObject);
        }
    }
}

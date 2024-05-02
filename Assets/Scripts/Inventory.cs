using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public event EventHandler<OnInventoryModifiedArgs> OnInventoryModified;
    public class OnInventoryModifiedArgs : EventArgs { public List<ItemSO> itemSOList; }


    [SerializeField] private int inventoryItemLimit = 8;

    private List<ItemSO> inventoryItems;

    private void Awake()
    {
        Instance = this;
        inventoryItems = new List<ItemSO>(inventoryItemLimit);
    }

    private void OnEnable()
    {
        Item.OnItemTryingToPick += Item_OnItemTryingToPick;
    }

    private bool Item_OnItemTryingToPick(ItemSO itemSO)
    {
        ItemSO pickuppedItemSO = itemSO;

        if (inventoryItems.Count < inventoryItemLimit)
        {
            AddToInventory(pickuppedItemSO);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddToInventory(ItemSO itemSO)
    {
        inventoryItems.Add(itemSO);
        Debug.Log($"Inventory Added Item : {itemSO.ItemName}");

        OnInventoryModified?.Invoke(this, new OnInventoryModifiedArgs { itemSOList = GetInventoryItems() });
    }

    public void RemoveFromInventory(ItemSO itemSo)
    {
        inventoryItems.Remove(itemSo);

        Debug.Log($"Inventory Used Item : {itemSo.ItemName}");
        OnInventoryModified?.Invoke(this, new OnInventoryModifiedArgs { itemSOList = GetInventoryItems() });
    }

    public void RemoveListOfItemFromInventory(List<ItemSO> itemSoList)
    {
        foreach (ItemSO item in itemSoList)
        {
            inventoryItems.Remove(item);
            Debug.Log($"Inventory Used Item : {item.ItemName}");
        }

        OnInventoryModified?.Invoke(this, new OnInventoryModifiedArgs { itemSOList = GetInventoryItems() });
    }

    public List<ItemSO> GetInventoryItems() => inventoryItems;

}

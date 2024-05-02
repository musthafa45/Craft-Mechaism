using System.Linq;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySingleItemUi : MonoBehaviour
{
    public static Action<ItemSO> OnItemIconClicked;

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemQuantity;

    private ItemSO itemSO;

    public void SetItemSO(ItemSO itemSO)
    {
        this.itemSO = itemSO;

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        iconImage.sprite = itemSO.ItemImageSprite;
        itemNameText.text = itemSO.ItemName;
        itemQuantity.text = GetQuantityFromItem(itemSO).ToString();
    }

    private int GetQuantityFromItem(ItemSO itemSO)
    {
        // Filter inventory items based on the gatherable object type
        var inventoryItems = Inventory.Instance.GetInventoryItems()
            .Where(item => item == itemSO);

        // Count the number of filtered inventory items
        int quantity = inventoryItems.Count();

        return quantity;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private Transform itemTemplateSingleUi;

    [SerializeField] private Button inventoryToggleButton;

    private List<ItemSO> instantiatedItemsUi = new List<ItemSO>();
    private void Awake()
    {
        inventoryToggleButton.onClick.AddListener(() =>
        {
            ToggleInventory();
        });
    }

    private void ToggleInventory()
    {
        itemsContainer.gameObject.SetActive(!itemsContainer.gameObject.activeSelf);
    }

    private void Start()
    {
        Inventory.Instance.OnInventoryModified += Inventoty_OnInventoryModified;

        itemsContainer.gameObject.SetActive(false);
        itemTemplateSingleUi.gameObject.SetActive(false);
    }

    private void Inventoty_OnInventoryModified(object sender, Inventory.OnInventoryModifiedArgs e)
    {
        UpdateInventoryUi(e.itemSOList);
    }

    private void UpdateInventoryUi(List<ItemSO> gatherableObjectSoList)
    {
        CleanUpTemplates();

        SpawnTemplates(gatherableObjectSoList);
    }

    private void SpawnTemplates(List<ItemSO> itemSOList)
    {
        instantiatedItemsUi.Clear();

        foreach (ItemSO itemSO in itemSOList)
        {
            if (!instantiatedItemsUi.Contains(itemSO))
            {
                InventorySingleItemUi inventorySingleItemUi = Instantiate(itemTemplateSingleUi, itemsContainer).
                                                              GetComponent<InventorySingleItemUi>();
                inventorySingleItemUi.gameObject.SetActive(true);
                inventorySingleItemUi.SetItemSO(itemSO);

                instantiatedItemsUi.Add(itemSO);
            }

        }
    }

    //private void CleanUpTemplates()
    //{
    //    foreach(Transform child in  itemsContainer)
    //    {
    //        if (child == itemTemplateSingleUi) continue;

    //        Destroy(child.gameObject);
    //    }

    //}

    private void CleanUpTemplates()
    {
        foreach (Transform child in itemsContainer)
        {
            if (child != itemTemplateSingleUi)
            {
                Destroy(child.gameObject);
            }
        }
    }
}

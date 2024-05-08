using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSingleUi : MonoBehaviour
{
    public static event EventHandler OnAnyCraftBtnPerformed;

    [SerializeField] private Image outputRecipeImage;
    [SerializeField] private TextMeshProUGUI outputRecipeNameText;
    [SerializeField] private Transform requirementItemTemplate;
    [SerializeField] private Transform requirementsContainer;
    [SerializeField] private Button craftButton;

    private RecipeSO outPutRecipeSO;

    private void Awake()
    {
        requirementItemTemplate.gameObject.SetActive(false);

        craftButton.onClick.AddListener(() => {
            Inventory.Instance.RemoveListOfItemFromInventory(GetListItemSOFromRecipeDatas(outPutRecipeSO.RequireItemSOList));

            var go = Instantiate(Resources.Load<GameObject>("Prefabs/Hammer"), GameObject.Find("Crafting Table").transform);

            Inventory.Instance.AddToInventory(outPutRecipeSO.OutputItemSO);

            ValidateCraftButton();

            OnAnyCraftBtnPerformed?.Invoke(this, EventArgs.Empty);
        });
    }

    public void ValidateCraftButton()
    {
        craftButton.interactable = HasSufficiantItemsOnInventory(outPutRecipeSO.RequireItemSOList);
    }

    private List<ItemSO> GetListItemSOFromRecipeDatas(RecipeSO.RecipeData[] requireItemSOList)
    {
        List<ItemSO> itemSoList = new List<ItemSO>();

        for (int i = 0; i < requireItemSOList.Length; i++)
        {
            for (int j = 0; j < requireItemSOList[i].ItemQuantity; j++)
            {
                itemSoList.Add(requireItemSOList[i].RequireItemSO);
            }
        }

        return itemSoList;
    }

    public void SetRecipeSo(RecipeSO outPutRecipeSO)
    {
        this.outPutRecipeSO = outPutRecipeSO;

        UpdateRecipeVisual();
    }

    private void UpdateRecipeVisual()
    {
        outputRecipeImage.sprite = outPutRecipeSO.OutputItemSO.ItemImageSprite;
        outputRecipeNameText.text = outPutRecipeSO.OutputItemSO.ItemName;

        CleanUpContainer();
        SpawnRequirmentsTemplates();

        ValidateCraftButton();

    }


    private void CleanUpContainer()
    {
        foreach (Transform child in requirementsContainer)
        {
            if(child == requirementItemTemplate) 
                continue;

            Destroy(child.gameObject);
        }
    }
    private void SpawnRequirmentsTemplates()
    {
        for (int i = 0; i < outPutRecipeSO.RequireItemSOList.Length; i++)
        {
            Transform reqTemplate = Instantiate(requirementItemTemplate, requirementsContainer);
            reqTemplate.GetComponent<Image>().sprite = outPutRecipeSO.RequireItemSOList[i].RequireItemSO.ItemImageSprite;
            reqTemplate.GetComponentInChildren<TextMeshProUGUI>().text = outPutRecipeSO.RequireItemSOList[i].ItemQuantity.ToString();
            reqTemplate.gameObject.SetActive(true);
        }
    }
    private bool HasSufficiantItemsOnInventory(RecipeSO.RecipeData[] requireItemSOList)
    {
        // Dictionary to store the count of each item in the player's inventory
        Dictionary<ItemSO, int> inventoryCounts = new Dictionary<ItemSO, int>();

        // Count items in the player's inventory
        foreach (ItemSO itemSO in Inventory.Instance.GetInventoryItems())
        {
            if (inventoryCounts.ContainsKey(itemSO))
            {
                inventoryCounts[itemSO]++;
            }
            else
            {
                inventoryCounts[itemSO] = 1;
            }
        }

        // Check if the player has enough of each required item
        foreach (RecipeSO.RecipeData recipeData in requireItemSOList)
        {
            if (!inventoryCounts.ContainsKey(recipeData.RequireItemSO) ||
                inventoryCounts[recipeData.RequireItemSO] < recipeData.ItemQuantity)
            {
                // Player does not have enough of this item
                return false;
            }
        }

        // Player has enough of all required items
        return true;
    }
}

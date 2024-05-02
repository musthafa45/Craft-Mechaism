using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSingleUi : MonoBehaviour
{
    [SerializeField] private Image outputRecipeImage;
    [SerializeField] private TextMeshProUGUI outputRecipeNameText;
    [SerializeField] private Transform requirementItemTemplate;
    [SerializeField] private Transform requirementsContainer;
    [SerializeField] private Button craftButton;

    private RecipeSO outPutRecipeSO;

    private void Awake()
    {
        requirementItemTemplate.gameObject.SetActive(false);

        craftButton.onClick.AddListener(() =>
        {
            Inventory.Instance.RemoveListOfItemFromInventory(GetListItemSOFromRecipeDatas(outPutRecipeSO.RequireItemSOList));

            Inventory.Instance.AddToInventory(outPutRecipeSO.OutputItemSO);

            ValidateCraftButton();
        });
    }

    private void ValidateCraftButton()
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
            Transform reqTemplate = Instantiate(requirementItemTemplate,requirementsContainer);
            reqTemplate.GetComponent<Image>().sprite = outPutRecipeSO.RequireItemSOList[i].RequireItemSO.ItemImageSprite;
            reqTemplate.GetComponentInChildren<TextMeshProUGUI>().text = outPutRecipeSO.RequireItemSOList[i].ItemQuantity.ToString();
            reqTemplate.gameObject.SetActive(true);
        }
    }
    private bool HasSufficiantItemsOnInventory(RecipeSO.RecipeData[] requireItemSOList)
    {
        List<ItemSO> inventoryItemSoList = Inventory.Instance.GetInventoryItems();

        for (int i = 0; i < requireItemSOList.Length; i++)
        {
            for (int j = 0; j < requireItemSOList[i].ItemQuantity; j++)
            {
                if(inventoryItemSoList.Contains(requireItemSOList[i].RequireItemSO))
                {
                    inventoryItemSoList.Remove(requireItemSOList[i].RequireItemSO);
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystemUi : MonoBehaviour
{
    public static CraftingSystemUi Instance { get; private set; }

    public event EventHandler OnCraftUiClosed;

    [SerializeField] private List<RecipeSO> recipieSOList;
    [SerializeField] private Transform baseUiTransform;
    [SerializeField] private Transform containerOfRecipies;
    [SerializeField] private Transform recipeSingleUiTemplateUi;
    [SerializeField] private Button closeCraftingUiButton;

    private void Awake()
    {
        Instance = this;
        closeCraftingUiButton.onClick.AddListener(() =>
        {
            Hide();

            OnCraftUiClosed?.Invoke(this, EventArgs.Empty);
        });
    }
    private void Start()
    {
        InteractionUi.Instance.OnCrafterInteractBtnPerformed += InteractionUi_Instance_OnCrafterInteractBtnPerformed;
        RecipeSingleUi.OnAnyCraftBtnPerformed += RecipeSingleUi_OnAnyCraftBtnPerformed;

        recipeSingleUiTemplateUi.gameObject.SetActive(false);

        Hide();
    }

    private void RecipeSingleUi_OnAnyCraftBtnPerformed(object sender, EventArgs e)
    {
        foreach (RecipeSingleUi recipeSingleUi in FindObjectsOfType<RecipeSingleUi>())
        {
            recipeSingleUi.ValidateCraftButton();
        }
    }

    private void InteractionUi_Instance_OnCrafterInteractBtnPerformed(object sender, EventArgs e)
    {
        Show();

        recipeSingleUiTemplateUi.gameObject.SetActive(false);

        UpdateCraftableList();
    }

    private void UpdateCraftableList()
    {
        CleanUpCraftableList();

        SpawnCraftableItems();
    }

    private void SpawnCraftableItems()
    {
        for (int i = 0; i < recipieSOList.Count; i++)
        {
            RecipeSingleUi recipeSingleUi = Instantiate(recipeSingleUiTemplateUi, containerOfRecipies).GetComponent<RecipeSingleUi>();
            recipeSingleUi.gameObject.SetActive(true);
            recipeSingleUi.SetRecipeSo(recipieSOList[i]);
        }

    }

    private void CleanUpCraftableList()
    {
        foreach (Transform child in containerOfRecipies)
        {
            if (child == recipeSingleUiTemplateUi)
                continue;

            Destroy(child.gameObject);
        }
    }

    private void Show()
    {
        baseUiTransform.gameObject.SetActive(true);
    }

    private void Hide()
    {
        baseUiTransform.gameObject.SetActive(false);
    }
}

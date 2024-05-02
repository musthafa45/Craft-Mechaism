using UnityEngine;

[CreateAssetMenu(fileName = "RecipeSO", menuName = "ScriptableObject/RecipeSO")]
public class RecipeSO : ScriptableObject 
{
    public RecipeData[] RequireItemSOList;
    public ItemSO OutputItemSO;

    [System.Serializable]
    public class RecipeData
    {
        public ItemSO RequireItemSO;
        public int ItemQuantity = 1;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName ="ItemSO",menuName ="ScriptableObject/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string ItemName;
    public Sprite ItemImageSprite;
    public GameObject ItemPrefab;
}

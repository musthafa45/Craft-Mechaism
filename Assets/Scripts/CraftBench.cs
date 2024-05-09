using UnityEngine;

public class CraftBench : MonoBehaviour, IInteractable
{
    [SerializeField] private Outline outline;
    [SerializeField] private ItemProgressMode progressMode;
    [SerializeField] private Transform ItemSpawnPoint;

    private bool interactDisabling = false;

    public enum ItemProgressMode
    {
        InstantSpawn,
        Progress
    }

    private void Start()
    {
        RecipeSingleUi.OnAnyCraftItemSpawned += RecipeSingleUi_OnAnyCraftItemSpawned;
        //InteractionUi.Instance.OnGenericInteractBtnPerformed += InetractorUi_Instance_OnCrafterInteractBtnPerformed;
    }

    //private bool InetractorUi_Instance_OnCrafterInteractBtnPerformed()
    //{
        
    //}

    private void RecipeSingleUi_OnAnyCraftItemSpawned(object sender, RecipeSingleUi.CraftItemSpawnedArgs e)
    {
        switch(progressMode)
        {
            case ItemProgressMode.InstantSpawn:

                if (!e.toSpawnItemSO.ItemPrefab)
                {
                    Debug.LogWarning("Item Prefab Not Referenced On ItemSO");
                    return;
                }
                    
                Transform itemSpawnedTr = Instantiate(e.toSpawnItemSO.ItemPrefab, ItemSpawnPoint).transform;
                itemSpawnedTr.localPosition = Vector3.zero;

                break;

            case ItemProgressMode.Progress:

                break;
        }
    }

    private void Update()
    {
        interactDisabling = ItemSpawnPoint.childCount > 0;

        if (ItemSpawnPoint.childCount > 0)
        { 
            //Has Still Spawned Object
        }
        else
        {
            //Object Picked Or Not Yet Spawned

        }
    }

    private void Awake()
    {
        SetActiveOutLine(false);
    }
    public void Interact()
    {
        if(interactDisabling)
        {
            Debug.Log($"This {gameObject.name} Interaction Disabled");
            return;
        }
        Debug.Log("Crafting System Interated");
    }

    public void SetActiveSelectedVisual(bool active)
    {
        SetActiveOutLine(active && !interactDisabling);
    }
    private void SetActiveOutLine(bool active)
    {
        outline.enabled = active;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

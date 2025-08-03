using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    #region Instance
    public static Items Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    #endregion
    public bool HasWeapon = false;
    public bool HasKey = false;
    [SerializeField] Transform itemGrid;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] private ItemSpriteData spriteDatabase;
    void Start()
    {
        ActionNotifier.Instance.Item += SetItem;
    }
    void OnDestroy()
    {
        ActionNotifier.Instance.Item -= SetItem;
    }
    public void SetItem(Enums.Items item)
    {
        GameObject _item = Instantiate(itemPrefab, itemGrid);
        ItemUi itemUI = _item.GetComponent<ItemUi>();

        Sprite sprite = spriteDatabase.GetSprite(item);
        itemUI.SetSprite(sprite);
        switch (item)
        {
            case Enums.Items.Key1:
                HasKey = true;
                break;
            case Enums.Items.Dagger:
                HasWeapon = true;
                break;
            default:
                break;
        }
    }
}

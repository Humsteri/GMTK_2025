using System.Collections.Generic;
using UnityEngine;

public class ItemSpriteData : MonoBehaviour
{
    [System.Serializable]
    public struct ItemSpritePair
    {
        public Enums.Items item;
        public Sprite sprite;
    }
    public List<ItemSpritePair> itemSprites;

    private Dictionary<Enums.Items, Sprite> itemSpriteDict;

    void Awake()
    {
        itemSpriteDict = new Dictionary<Enums.Items, Sprite>();
        foreach (var pair in itemSprites)
        {
            itemSpriteDict[pair.item] = pair.sprite;
        }
    }

    public Sprite GetSprite(Enums.Items item)
    {
        return itemSpriteDict.TryGetValue(item, out var sprite) ? sprite : null;
    }
}

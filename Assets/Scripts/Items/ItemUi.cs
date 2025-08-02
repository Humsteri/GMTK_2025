using UnityEngine;
using UnityEngine.UI;

public class ItemUi : MonoBehaviour
{
    [SerializeField] private Image itemImage;

    public void SetSprite(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }
}

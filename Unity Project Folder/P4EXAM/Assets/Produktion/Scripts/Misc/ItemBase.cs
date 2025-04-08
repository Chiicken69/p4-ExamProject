using UnityEngine;





public class ItemBase : MonoBehaviour
{
    [System.Serializable]
    public enum ItemType { Wood, iron, Copper };
    [SerializeField] public ItemType type;
    public string itemName;
    public string itemDescription;

    public Sprite Sprite;
}

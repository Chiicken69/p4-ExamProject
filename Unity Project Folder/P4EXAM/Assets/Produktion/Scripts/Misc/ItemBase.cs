using UnityEngine;





public class ItemBase : MonoBehaviour
{
    [System.Serializable]
    public enum ItemType {Drone, Spool, Wire, metalParts, Gear, MetalPipe, SheetMetal, 
                         Fabric, Generator, Structure, Blade, FlyingWindmill, Iron, Copper,
                         Cotton};
    [SerializeField] public ItemType type;
    public string itemName;
    public string itemDescription;

    public Sprite Sprite;
}

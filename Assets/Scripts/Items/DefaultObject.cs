using UnityEngine;

[CreateAssetMenu(fileName = "new default object", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Default;    
    }
}

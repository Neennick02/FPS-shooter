using UnityEngine;

[CreateAssetMenu(fileName = "new ammo object", menuName = "Inventory System/Items/Ammo")]

public class AmmoObject : ItemObject
{
    public int amount;
    public void Awake()
    {
        type = ItemType.Ammo;
    }
}

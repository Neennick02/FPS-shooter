using UnityEngine;

[CreateAssetMenu(fileName = "new Equipment object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}

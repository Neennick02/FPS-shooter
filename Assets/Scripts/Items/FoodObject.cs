using UnityEngine;

[CreateAssetMenu(fileName = "new food object", menuName = "Inventory System/Items/Food")]

public class FoodObject : ItemObject
{
    public int restoreHealthValue;
    public void Awake()
    {
        type = ItemType.Food;
    }
}

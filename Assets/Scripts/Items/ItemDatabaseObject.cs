using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Item DataBase", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] items;

    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
        for(int i =0; i < items.Length; i++)
        {
            items[i].Id = i;
            GetItem.Add( i, items[i]);

        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
    }
}

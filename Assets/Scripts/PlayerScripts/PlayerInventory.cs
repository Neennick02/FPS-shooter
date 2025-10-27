using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;

    public void AddItem(GameObject other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            inventory.AddItem(new Item(item.item), 1);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("Load");
            inventory.Load();
        }
    }

    private void OnApplicationQuit()
    {
        //on close empty inventory slots
        inventory.Container.Items = new InventorySlot[24];
    }
}

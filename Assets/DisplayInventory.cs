using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;

    public int X_Start;
    public int Y_Start;

    public int Number_Of_Columns;
    public int X_offset;
    public int Y_Offset;
    Dictionary<InventorySlot, GameObject> itemsDisplay = new Dictionary<InventorySlot, GameObject>();
    void Start()
    {
        createDisplay();
    }

    // Update is called once per frame
    void Update()
    {   
            UpdateDisplay();
    }

    public void createDisplay()
    {
        for(int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);//this line
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            itemsDisplay.Add(inventory.Container[i], obj);

        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_Start + ( X_offset * (i % Number_Of_Columns)), Y_Start + (-Y_Offset * (i/Number_Of_Columns)), 0f );
    }

    public void UpdateDisplay()
    {
        for(int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplay.ContainsKey(inventory.Container[i]))
            {
                itemsDisplay[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);//this line
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                itemsDisplay.Add(inventory.Container[i], obj);
            }
        }
    }
}

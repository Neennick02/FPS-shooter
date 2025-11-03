using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class DynamicInterface : UserInterface
{
    public int X_Start;
    public int Y_Start;

    public int Number_Of_Columns;
    public int X_offset;
    public int Y_Offset;

    public GameObject inventoryPrefab;

    public override void CreateSlots()
    {
        itemsDisplay = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            itemsDisplay.Add(obj, inventory.Container.Items[i]);
        }
    }
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_Start + (X_offset * (i % Number_Of_Columns)), Y_Start + (-Y_Offset * (i / Number_Of_Columns)), 0f);
    }

}

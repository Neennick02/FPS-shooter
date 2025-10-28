using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    [SerializeField] GameObject inventoryCanvas;
    [SerializeField] PlayerLook playerLook;
    [SerializeField] PlayerMotor playerMotor;
    bool inventoryOpened = false;
    InputManager input;
    List<Transform> inventorySlots = new List<Transform>();
    bool firstFrame = true;
    private void Start()
    {
        input = GetComponent<InputManager>();
        inventorySlots.AddRange(inventoryCanvas.GetComponentsInChildren<Transform>());
    }

    private void Update()
    {
        if (firstFrame)
        {
            EnableCanvas(false);
            ActivateMouse(false);
            firstFrame = false;
        }

        if (input.onFoot.Inventory.triggered)
        {
            inventoryOpened = !inventoryOpened;

            if (!inventoryOpened)
            {
                OpenAndCloseInventory(false);
            }
            else
            {
                OpenAndCloseInventory(true);
            }
        }
    }

    public void AddItem(GameObject other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            inventory.AddItem(new Item(item.item), 1);
        }
    }

    public void Save()
    {
        inventory.Save();
    }

    public void Load()
    {
        inventory.Load();
    }

    private void OnApplicationQuit()
    {
        //on close empty inventory slots
        inventory.Container.Items = new InventorySlot[25];
    }

    void OpenAndCloseInventory(bool value)  //set mouselock, inventory canvas
    {                                       //disable player look/ move
        ActivateMouse(value);
        EnableCanvas(value);

       input.BlockInput(value); //prevent player from moving/ turn head during inventory
    }
    public void ActivateMouse(bool active)
    {
        //makes cursor invisible during gameplay
        Cursor.visible = active;
        if (!active)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void EnableCanvas(bool enable)
    {
        Image img = inventoryCanvas.GetComponent<Image>();
        if (!enable)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0); //disable
        }
        else
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1); //enable
        }

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].gameObject.SetActive(enable);
        }
    }
}

using UnityEngine;

public class KeyPad : Interactable
{
    protected override void Interact()
    {
        Debug.Log("Interacted with" + gameObject.name);
    }
}

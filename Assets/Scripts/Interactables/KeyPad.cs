using UnityEngine;

public class KeyPad : Interactable
{
    bool isOpen;
    [SerializeField] GameObject door;
    protected override void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
        isOpen = !isOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", isOpen);
    }
}

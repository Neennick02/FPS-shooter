using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //displays message to player when looking 
    public string promptMessage;
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {

    }
}

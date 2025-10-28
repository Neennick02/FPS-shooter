using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //add or remove interactionEvent to gameobject
    public bool useEvents;
    //displays message to player when looking 
    public string promptMessage;
    public void BaseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().onInteract.Invoke();
        }

        Interact();
    }

    protected virtual void Interact()
    {

    }
}

using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] float distance = 3f;
    [SerializeField]  LayerMask mask;
    PlayerUI PlayerUI;
    InputManager inputManager;
    void Start()
    {
        PlayerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }


    void Update()
    {
        PlayerUI.UpdateText(string.Empty);

        //draw ray to look for interactable
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; //variable to store collision info

        //raycast to center of screen
       if(Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            //check if object is interactable
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                //show promt message
                PlayerUI.UpdateText(interactable.promptMessage);

                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }

    }
}

using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] float distance = 3f;
    [SerializeField]  LayerMask mask;
    PlayerUI PlayerUI;
    void Start()
    {
        PlayerUI = GetComponent<PlayerUI>();
    }

    void Update()
    {
        PlayerUI.UpdateText(string.Empty);

        //draw ray to look for interactable
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; //variable to store collision info
       if(Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                PlayerUI.UpdateText(hitInfo.collider.GetComponent<Interactable>().promptMessage);
            }
        }

    }
}

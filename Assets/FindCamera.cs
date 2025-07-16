using UnityEngine;

public class FindCamera : MonoBehaviour
{
    Camera cam;
    [SerializeField] Canvas canvas;

    private void Start()
    {
        cam = FindFirstObjectByType<Camera> ();
        if(cam != null)
        {
            canvas.worldCamera = cam;
        }
    }
}

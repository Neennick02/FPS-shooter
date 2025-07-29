using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    float xRotation = 0f;
    float yRotation = 0f;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    float recoilX = 0;
    float recoilY = 0;
    [SerializeField] float recoilReturnSpeed = 8;
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // Apply vertical rotation (pitch) with recoil
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation += recoilX; // Add recoil pitch
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Apply horizontal rotation (yaw) with recoil
        yRotation += (mouseX * Time.deltaTime) * xSensitivity;
        yRotation += recoilY;

        // Apply rotations
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);

        // Smoothly reset recoil over time
        recoilX = Mathf.Lerp(recoilX, 0, Time.deltaTime * recoilReturnSpeed);
        recoilY = Mathf.Lerp(recoilY, 0, Time.deltaTime * recoilReturnSpeed);
    }

    public void AddCamRecoil(float up, float side)
    {
        recoilX += up;
        recoilY += side;
    }
}

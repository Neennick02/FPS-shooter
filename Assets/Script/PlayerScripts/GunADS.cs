using UnityEngine;

public class GunADS : MonoBehaviour
{
    [SerializeField] Transform ADS_pos;
    [SerializeField] Transform hip_Pos;
    [SerializeField] float aimSpeed = 8f;

    [SerializeField] Camera playerCam;
    [SerializeField] float zoomFOV = 40f;
    float normalFOV;

    bool isAiming = false;

    private void Start()
    {
        normalFOV = playerCam.fieldOfView;
        
    }

    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

        //move between hipPos and ADSpos
        if (isAiming)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, ADS_pos.localPosition, Time.deltaTime * aimSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, ADS_pos.localRotation, Time.deltaTime * aimSpeed);

            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, zoomFOV, Time.deltaTime * aimSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, hip_Pos.localPosition, Time.deltaTime * aimSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, hip_Pos.localRotation, Time.deltaTime * aimSpeed);

            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, normalFOV, Time.deltaTime * aimSpeed);
        }
    }
}

using UnityEditor;
using UnityEngine;

public class GunADS : MonoBehaviour
{
    [SerializeField] Settings _playerSettings;
    [SerializeField] Transform ADS_pos;
    [SerializeField] Transform hip_Pos;
    [SerializeField] float aimSpeed = 8f;

    [SerializeField] Camera playerCam;
    [SerializeField] float zoomFOV;
    float normalFOV;

    bool isAiming = false;

    private void Start()
    {
        normalFOV = playerCam.fieldOfView;
        zoomFOV = normalFOV / 1.5f;
    }

    void Update()
    {
        if (InputManager.Instance.onFoot.Aim.triggered)
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

            playerCam.fieldOfView = Mathf.Lerp(normalFOV, zoomFOV, Time.deltaTime * aimSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, hip_Pos.localPosition, Time.deltaTime * aimSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, hip_Pos.localRotation, Time.deltaTime * aimSpeed);

            playerCam.fieldOfView = Mathf.Lerp(zoomFOV, normalFOV, Time.deltaTime * aimSpeed);
        }

        //normalFOV = PlayerSettings.fieldOfView;
        zoomFOV = normalFOV / 1.5f;
    }
}

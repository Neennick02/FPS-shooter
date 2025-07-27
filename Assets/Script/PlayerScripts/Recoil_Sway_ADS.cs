using UnityEngine;

public class Recoil_Sway_ADS : MonoBehaviour
{
    [Header("Sway config")]
    [SerializeField] float swayAmount = .02f;
    [SerializeField] float swaySmooth = 6f;

    [Header("Recoil config")]
    [SerializeField] float recoilKickBack = .1f;
    [SerializeField] float recoilUp = 2f;
    [SerializeField] float recoilReturnSpeed = 6f;

    Vector3 startPos;
    Quaternion startRotation;
    Vector3 currentPosition;
    Quaternion currentRotation;

    [Header("ADS config")]
    [SerializeField] Transform ADS_pos;
    [SerializeField] Transform hip_Pos;
    [SerializeField] float aimSpeed = 8f;

    [SerializeField] Camera playerCam;
    [SerializeField] float zoomFOV = 40f;
    float normalFOV;

    bool isAiming = false;

    private void Start()
    {
        currentPosition = transform.localPosition;
        currentRotation = transform.localRotation;

        startPos = hip_Pos.localPosition;
        startRotation = hip_Pos.localRotation;

        normalFOV = playerCam.fieldOfView;

    }

    private void Update()
    {
        HandleSway();
        HandleRecoil();
        ChangeGrip();
    }

    void HandleSway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //target pos based on mouse pos
        Vector3 swayOffset = new Vector3(-mouseX, -mouseY, 0) * swayAmount;

        //smooth move position & rotation
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos + swayOffset + currentPosition, Time.deltaTime * swaySmooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, startRotation * currentRotation, Time.deltaTime * recoilReturnSpeed);
    }

    void HandleRecoil()
    {
        // Slowly reset recoil position and rotation
        currentPosition = Vector3.Lerp(currentPosition, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
        currentRotation = Quaternion.Slerp(currentRotation, Quaternion.identity, Time.deltaTime * recoilReturnSpeed);
    }

    //gets called when player shoots
    public void ApplyRecoil()
    {
        //gun recoil
        currentPosition -= new Vector3(0, 0, recoilKickBack);
        currentRotation *= Quaternion.Euler(-recoilUp, 0, 0);

        //cam recoil
    }

    void ChangeGrip()
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
            startPos = Vector3.Lerp(startPos, ADS_pos.localPosition, Time.deltaTime * aimSpeed);
            startRotation = Quaternion.Lerp(startRotation, ADS_pos.localRotation, Time.deltaTime * aimSpeed);

            //change FOV
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, zoomFOV, Time.deltaTime * aimSpeed);
        }
        else //move back
        {
            startPos = Vector3.Lerp(startPos, hip_Pos.localPosition, Time.deltaTime * aimSpeed);
            startRotation = Quaternion.Lerp(startRotation, hip_Pos.localRotation, Time.deltaTime * aimSpeed);

            //change FOV
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, normalFOV, Time.deltaTime * aimSpeed);
        }
    }
}

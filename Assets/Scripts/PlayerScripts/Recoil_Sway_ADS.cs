using UnityEngine;

public class Recoil_Sway_ADS : MonoBehaviour
{
    [Header("Sway config")]
    [SerializeField] float swayAmount = .02f;
    [SerializeField] float swaySmooth = 6f;

    // Internal sway offsets
    private Vector3 swayOffset;
    private Quaternion swayRotation;

    [Header("Recoil config")]
    [SerializeField] float recoilKickBack = .1f;
    [SerializeField] float recoilReturnSpeed = 6f;

    Vector3 currentPosition;
    Quaternion currentRotation;

    [Header("ADS config")]
    public bool isAiming = false;
    [SerializeField] Transform ADS_pos;
    [SerializeField] Transform hip_Pos;

    [SerializeField] Transform pistol_Hip_Pos;
    [SerializeField] Transform pistol_ADS_Pos;

    [SerializeField] float aimSpeed = 8f;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] Camera playerCam;
    [SerializeField] float zoomFOV = 40f;
    float normalFOV;

    Vector3 basePos;
    Quaternion baseRot;

    [SerializeField] PlayerLook playerLookScript;
    public InputManager input;
    GunScript gun;
    private void Start()
    {
        gun = GetComponentInChildren<GunScript>();
        currentPosition = Vector3.zero;
        currentRotation = Quaternion.identity;

        basePos = transform.position;
        baseRot = transform.rotation;

        normalFOV = playerCam.fieldOfView;
    }

    private void Update()
    {
        HandleRecoil();
        ChangeGrip();
    }

    private void LateUpdate()
    {
        HandleSway();
    }

    void ChangeGrip()
    {
        if (input.onFoot.Aim.IsPressed() && !gun.isReloading)
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }


        //makes sure to use pistol hold positions when using pistols
        if(weaponManager.weaponIndex == 0)
        {
            Aim(pistol_Hip_Pos, pistol_ADS_Pos);
        }
        else
        {
            Aim(hip_Pos, ADS_pos);
        }
    }

    void Aim(Transform hipPos, Transform ADSpos)
    {
        //move between hipPos and ADSpos
        if (isAiming)
        {
            basePos = Vector3.Lerp(basePos, ADS_pos.localPosition, Time.deltaTime * aimSpeed);
            baseRot = Quaternion.Lerp(baseRot, ADS_pos.localRotation, Time.deltaTime * aimSpeed);

            //change FOV
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, zoomFOV, Time.deltaTime * aimSpeed);
        }
        else //move back
        {
            basePos = Vector3.Lerp(basePos, hip_Pos.localPosition, Time.deltaTime * aimSpeed);
            baseRot = Quaternion.Lerp(baseRot, hip_Pos.localRotation, Time.deltaTime * aimSpeed);

            //change FOV
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, normalFOV, Time.deltaTime * aimSpeed);
        }
    }


    void HandleRecoil()
    {
        // Slowly reset recoil position and rotation
        currentPosition = Vector3.Lerp(currentPosition, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
        currentRotation = Quaternion.Slerp(currentRotation, Quaternion.identity, Time.deltaTime * recoilReturnSpeed);
    }

    void HandleSway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //target pos based on mouse pos
        swayOffset = new Vector3(-mouseX, -mouseY, 0) * swayAmount;
        swayRotation = Quaternion.Euler(-mouseY * swayAmount * 30f, mouseX * swayAmount * 30f, 0f);

        //combine sway + base + recoil
        Vector3 finalPos = basePos + swayOffset + currentPosition;
        Quaternion finalRot = baseRot * swayRotation * currentRotation;

        //smooth move position & rotation
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, Time.deltaTime * swaySmooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRot, Time.deltaTime * recoilReturnSpeed);
    }


    public void RecoilFire(float recoilUp, float recoilSide)
    {
        //gun recoil
        currentPosition -= new Vector3(0, 0, recoilKickBack);
        currentRotation *= Quaternion.Euler(-recoilUp, 0, 0);

        //camera recoil
        playerLookScript.AddCamRecoil(Random.Range(recoilUp, recoilUp - 1) /3, Random.Range(-recoilSide, recoilSide));
    }
}

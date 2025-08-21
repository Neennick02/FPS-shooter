using UnityEngine;

public class Recoil_Sway_ADS : MonoBehaviour
{
    [Header("Sway config")]
    [SerializeField] float swayAmount = .02f;
    [SerializeField] float swaySmooth = 6f;

    // Internal sway offsets
    private Vector3 swayOffset;
    private Quaternion swayRotation;

    [Header("Movement Sway/Bob")]
    [SerializeField] float walkSwayAmount = 0.05f;
    [SerializeField] float runSwayAmount = 0.1f;
    [SerializeField] float walkSwaySpeed = 6f;
    [SerializeField] float runSwaySpeed = 10f;

    private float swayTimer = 0f;

    [SerializeField] PlayerMotor playerMovement; // reference your movement script

    [Header("Recoil config")]
    [SerializeField] float recoilKickBack = .1f;
    [SerializeField] float recoilReturnSpeed = 6f;

    Vector3 currentPosition;
    Quaternion currentRotation;

    [SerializeField] WeaponManager weaponManager;
    [SerializeField] Camera playerCam;

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

        basePos = transform.localPosition;
        baseRot = transform.localRotation;

    }

    private void Update()
    {
        HandleRecoil();
    }

    private void LateUpdate()
    {
        HandleSway();
        HandleMovementSway();
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
        playerLookScript.AddCamRecoil(Random.Range(recoilUp - 1, recoilUp ) /3, Random.Range(-recoilSide, recoilSide));
    }

    void HandleMovementSway()
    {
        if (playerMovement == null) return;

        // Get player speed factor (0 = idle, 1 = walking, 2 = running)
        float speedFactor = playerMovement.currentSpeed ; // or normalized 0..1 or 0..2
        if (speedFactor < 0.01f) return; // no movement, no sway

        float swayAmount = speedFactor > 1 ? runSwayAmount /10: walkSwayAmount / 10;
        float swaySpeed = speedFactor > 1 ? runSwaySpeed /10 : walkSwaySpeed / 10;

        swayTimer += Time.deltaTime * swaySpeed;

        // Vertical bob (up/down) and horizontal sway (left/right)
        float swayX = Mathf.Sin(swayTimer) * swayAmount;
        float swayY = Mathf.Sin(swayTimer * 2f) * swayAmount;

        // Apply to final gun position
        Vector3 movementSwayOffset = new Vector3(swayX, swayY, 0);
        transform.localPosition += movementSwayOffset;
    }
}

using UnityEngine;

public class Recoil_Sway_ADS : MonoBehaviour
{
    [SerializeField] private GunSwayObject _gunSwayData;
    [SerializeField] private Settings _settings;
    // Internal sway offsets
    private Vector3 swayOffset;
    private Quaternion swayRotation;

   

    private float swayTimer = 0f;

    [SerializeField] PlayerMotor playerMovement; // reference your movement script

  

    Vector3 currentPosition;
    Quaternion currentRotation;

    [SerializeField] WeaponManager weaponManager;
    [SerializeField] Camera playerCam;
    private float camBobTimer = 0f;

    private Vector3 _camInitialLocalPos;
    Vector3 basePos;
    Quaternion baseRot;

    [SerializeField] PlayerLook playerLookScript;
    private InputManager input;
    GunScript gun;
    private void Start()
    {
        gun = GetComponentInChildren<GunScript>();
        currentPosition = Vector3.zero;
        currentRotation = Quaternion.identity;

        basePos = transform.localPosition;
        baseRot = transform.localRotation;

        if (playerCam != null)
            _camInitialLocalPos = playerCam.transform.localPosition;

        if (InputManager.Instance != null) input = InputManager.Instance;
    }

    private void Update()
    {
        if (_settings.Paused) return;

        HandleRecoil();
    }

    private void LateUpdate()
    {
        if(_settings.Paused) return;
        HandleSway();
        HandleCameraSway();
        HandleMovementSway();
    }


    void HandleRecoil()
    {
        // Slowly reset recoil position and rotation
        currentPosition = Vector3.Lerp(currentPosition, Vector3.zero, Time.deltaTime * _gunSwayData.ReturnSpeed);
        currentRotation = Quaternion.Slerp(currentRotation, Quaternion.identity, Time.deltaTime * _gunSwayData.ReturnSpeed);
    }

    void HandleSway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //target pos based on mouse pos
        swayOffset = new Vector3(-mouseX, -mouseY, 0) * _gunSwayData.SwayAmount;
        swayRotation = Quaternion.Euler(-mouseY * _gunSwayData.SwayAmount * 30f, mouseX * _gunSwayData.SwayAmount * 30f, 0f);

        //combine sway + base + recoil
        Vector3 finalPos = basePos + swayOffset + currentPosition;
        Quaternion finalRot = baseRot * swayRotation * currentRotation;

        //smooth move position & rotation
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, Time.deltaTime * _gunSwayData.SwaySmooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRot, Time.deltaTime * _gunSwayData.ReturnSpeed);
    }


    public void RecoilFire(float recoilUp, float recoilSide)
    {
        //gun recoil
        currentPosition -= new Vector3(0, 0, _gunSwayData.RecoilKickback);
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

        float swayAmount = speedFactor > 1 ? _gunSwayData.RunSwayAmount / 10: _gunSwayData.WalkSwayAmount / 10;
        float swaySpeed = speedFactor > 1 ? _gunSwayData.RunSwaySpeed / 10 : _gunSwayData.WalkSwaySpeed / 10;

        swayTimer += Time.deltaTime * swaySpeed;

        // Vertical bob (up/down) and horizontal sway (left/right)
        float swayX = Mathf.Sin(swayTimer) * swayAmount;
        float swayY = Mathf.Sin(swayTimer * 2f) * swayAmount;

        // Apply to final gun position
        Vector3 movementSwayOffset = new Vector3(swayX, swayY, 0);
        transform.localPosition += movementSwayOffset;
    }

    void HandleCameraSway()
    {
        float speedFactor = playerMovement.currentSpeed;

        bool isRunning = speedFactor > 1f;
        bool isWalking = speedFactor > 0;

        float bobAmount;
        float bobSpeed;

        if (isRunning)
        {
             bobAmount =  _gunSwayData.RunBobAmount;
             bobSpeed = _gunSwayData.RunBobSpeed;
        }
        else if (isWalking)
        {
             bobAmount = _gunSwayData.WalkBobAmount;
             bobSpeed =  _gunSwayData.WalkBobSpeed;
        }
        else
        {
             bobAmount = _gunSwayData.DefaultBobAmount;
             bobSpeed =  _gunSwayData.DefaulkBobSpeed;
        }


        camBobTimer += Time.deltaTime * bobSpeed;

        float bobX = Mathf.Sin(camBobTimer) * bobAmount * 0.5f; // subtle side sway
        float bobY = Mathf.Sin(Mathf.Cos(camBobTimer * 2f) * bobAmount); // vertical bob

        Vector3 finalCamPos = _camInitialLocalPos + new Vector3(bobX, bobY, 0);

        playerCam.transform.localPosition = Vector3.Lerp(
            playerCam.transform.localPosition,
            finalCamPos,
            Time.deltaTime * 5f
        );
    }
}

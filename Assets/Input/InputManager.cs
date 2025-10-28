using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;

    PlayerMotor motor;
    PlayerLook look;

    bool blockInputs = false;
    void Awake()
    {
        //null check
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerInput = new PlayerInput();

        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        onFoot.Jump.performed += ctx => motor.Jump();
    }

    private void FixedUpdate()
    {
        if (blockInputs) return;
        //tell playermotor to move using values from movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        if (blockInputs) return;
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }

    public void BlockInput(bool value)
    {
        blockInputs = value;
    }
}

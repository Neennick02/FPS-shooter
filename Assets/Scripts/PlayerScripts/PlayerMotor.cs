using System.Collections;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private MovementObject _movementObject;
    Vector3 playerVelocity;
    bool isGrounded;
    bool isSprinting = false;
    public int currentSpeed;


    // Ladder climbing
    bool isClimbing = false;
    Transform currentLadder;

    // Crouch and slide
    bool isCrouching = false;
    bool isSliding = false;
    Vector3 slideVelocity;

    private float _targetHeight;
    Vector3 targetCenter;

    //  float slideInterval = 1f; 
    Coroutine _slideRoutine;
    float slideTimer = 0;
    
    InputManager _input;
    private CharacterController _controller;
    private PlayerLook _playerLook;
    void Start()
    {
        _input = GetComponent<InputManager>();
        _controller = GetComponent<CharacterController>();
        _playerLook = GetComponent<PlayerLook>();
        currentSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = _controller.isGrounded;

        Vector2 moveInput = _input.onFoot.Movement.ReadValue<Vector2>();

        if (_input.onFoot.Sprint.IsPressed() && moveInput.magnitude > 0.1f) //if moving and sprint is pressed
        {
            isSprinting = true;
        }
        else if (_input.onFoot.Sprint.WasReleasedThisFrame() || moveInput.magnitude < 0.1f) //no movement
        {
            isSprinting = false;
        }

        HandleCrouchInput(); //check for crouch input
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(_movementObject.JumpHeight * -3f * _movementObject.Gravity);
        }
    }

    //receive input from InputManager and apply to CharacterController
    public void ProcessMove(Vector2 input)
    {
        if (isClimbing)
        {
            ClimbLadder(input);
            return;
        }

        if (isSliding) return;

        NormalMovement(input);
    }

    void NormalMovement(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero; //reset direction and set x & y
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        if (isSprinting) //when sprinting change the movement speed and set currentSpeed to 2
        {
            _controller.Move(transform.TransformDirection(moveDirection) * _movementObject.SprintSpeed * Time.deltaTime);
            currentSpeed = 3;
            isCrouching = false;
        }
        else if(!isSprinting) //when not sprinting currentSpeed = 1  
        {
            _controller.Move(transform.TransformDirection(moveDirection) * _movementObject.Speed * Time.deltaTime);
            currentSpeed = 2;
        }

        if (isCrouching) //set movementspeed to crouchspeed and currentSpeed = 1
        {
            _controller.Move(transform.TransformDirection(moveDirection) * _movementObject.CrouchSpeed * Time.deltaTime);
            currentSpeed = 1;
        }

        if (input.magnitude < 0.1f) //when no input currentspeed == 0
        {
            currentSpeed = 0;
        }

        if (_controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }

        //handles gravity
        playerVelocity.y += _movementObject.Gravity * Time.deltaTime;

        //apply vertical velocity
        _controller.Move(playerVelocity * Time.deltaTime);
    }


    #region Ladder
    void ClimbLadder(Vector2 _input)
    {
        //cancel crouch 
        isCrouching = false;


        // Only vertical movement on ladder
        Vector3 climbDirection = new Vector3(0, _input.y, 0);
        _controller.Move(climbDirection * _movementObject.Speed * Time.deltaTime);

        // Reset vertical velocity so gravity doesn't pull down while climbing
        playerVelocity.y = 0;

        //exit ladder if player jumps or moves off ladder top/bottom
        if (this._input.onFoot.Jump.IsPressed())
        {
            isClimbing = false;
        }

        if (_input.y == 0 && !_controller.isGrounded)
        {
            // Stay on ladder if not grounded and no vertical input
        }
    }
    // Ladder triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            currentLadder = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            currentLadder = null;
        }
    }

    #endregion


    void HandleCrouchInput()
    {
        if (isSliding) return; // ignore crouch input during slide

        Vector2 moveInput = _input.onFoot.Movement.ReadValue<Vector2>();
        bool movingForward = moveInput.y > 0.1f;
        bool crouchPressed = _input.onFoot.Crouch.triggered;
        bool sprinting = _input.onFoot.Sprint.IsPressed();


        //check if player is sliding
        if (sprinting && !isSliding && crouchPressed && movingForward && _slideRoutine == null)
        {
            _controller.height = _movementObject.CrouchHeight;
            _slideRoutine = StartCoroutine(Slide(moveInput));
            return;
        }

        //crouch / stand toggle
        if (crouchPressed)
        {
            if (isCrouching) StandUp();
            else Crouch();
        }

        //cancel crouch on jump / sprint
        if(isCrouching && (_input.onFoot.Jump.triggered || sprinting))
        {
            StandUp();
        }
    }

    void Crouch()
    {
        isCrouching = true;
        _controller.height = _movementObject.CrouchHeight;
       // _controller.center = new Vector3(0, _movementObject.CrouchHeight, 0);

    }

    void StandUp()
    {
        // Check if there's room to stand up
        RaycastHit hit;
        float castDistance = _movementObject.DefaultHeight - _controller.height;
        Vector3 start = transform.position + Vector3.up * _controller.height;

        if (!Physics.SphereCast(start, _controller.radius, Vector3.up, out hit, castDistance))
        {
            isCrouching = false;
            _controller.height = _movementObject.DefaultHeight;
          //  _controller.center = new Vector3(0, _movementObject.DefaultHeight, 0);
        }
    }

    IEnumerator Slide(Vector2 inputDir)
    {

        isSprinting = false; //make sure sprint/crouch is disabled
        isCrouching = false;
        isSliding = true;

        //add gravity
        playerVelocity.y = -2f;
        //make player smaller
        _controller.height = _movementObject.CrouchHeight;

        Vector3 moveDir = transform.forward;
        slideVelocity = moveDir * _movementObject.SlideSpeed;

        float currentSlideSpeed = _movementObject.SlideSpeed;

        while (slideTimer < _movementObject.SlideDuration)
        {
            _controller.Move(slideVelocity * Time.deltaTime);

            //add to timer
            slideTimer += Time.deltaTime;

            slideVelocity = Vector3.Lerp(slideVelocity, Vector3.zero, Time.deltaTime * 4);
            yield return null;
        }
        _controller.height = _movementObject.DefaultHeight;

        isSliding = false;
        slideTimer = 0;
        _slideRoutine = null;

        //transfer velocity to movement
        playerVelocity = new Vector2(slideVelocity.x, slideVelocity.y);
    }
}

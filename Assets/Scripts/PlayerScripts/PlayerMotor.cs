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


    float targetHeight;

    float crouchSpeed;
    float standSpeed;
    float slideSpeed;

    Vector3 targetCenter;

    float slideDuration = 0.3f;
  //  float slideInterval = 1f; 
    float slideTimer = 0;

    [Header("Scripts")]
    InputManager input;
    [SerializeField] CharacterController controller;
    [SerializeField] PlayerLook playerLook;
    void Start()
    {
        input = GetComponent<InputManager>();
        crouchSpeed = _movementObject.Speed / 2;
        standSpeed = _movementObject.Speed;
        slideSpeed = _movementObject.Speed * 1.1f;
        currentSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        Vector2 moveInput = input.onFoot.Movement.ReadValue<Vector2>();

        if (input.onFoot.Sprint.triggered && moveInput.magnitude > 0.1f)
        {
            isSprinting = true;
        }
        else if (input.onFoot.Sprint.WasReleasedThisFrame() || moveInput.magnitude < 0.1f)
        {
            isSprinting = false;
        }
        HandleCrouchInput();
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(_movementObject.JumpHeight * -3f * _movementObject.Gravity);
        }
    }

    void NormalMovement(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        if (isSprinting)
        {
            controller.Move(transform.TransformDirection(moveDirection) * _movementObject.SprintSpeed * Time.deltaTime);
            currentSpeed = 2;
            isCrouching = false;
        }
        else
        {
            controller.Move(transform.TransformDirection(moveDirection) * _movementObject.Speed * Time.deltaTime);
            currentSpeed = 1;
        }
      
        if (input.magnitude < 0.1f)
        {
            currentSpeed = 0;
        }


        if (isCrouching)
        {
            controller.Move(transform.TransformDirection(moveDirection) * crouchSpeed * Time.deltaTime);
            currentSpeed = 1;
        }

        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }

        //handles gravity
        playerVelocity.y += _movementObject.Gravity * Time.deltaTime;

        //apply vertical velocity
        controller.Move(playerVelocity * Time.deltaTime);
    }




    //receive input from InputManager and apply to CharacterController
    public void ProcessMove(Vector2 input)
    {
        if (isClimbing) 
        {
            ClimbLadder(input); 
            return;
        }

        if (isSliding)
            {
                StartCoroutine(Slide(input));
            }
        else
            {
                NormalMovement(input);
            }
    }



    void ClimbLadder(Vector2 _input)
    {
        //cancel crouch 
        isCrouching = false;


        // Only vertical movement on ladder
        Vector3 climbDirection = new Vector3(0, _input.y, 0);
        controller.Move(climbDirection * _movementObject.Speed * Time.deltaTime);

        // Reset vertical velocity so gravity doesn't pull down while climbing
        playerVelocity.y = 0;

        //exit ladder if player jumps or moves off ladder top/bottom
        if (input.onFoot.Jump.IsPressed())
        {
            isClimbing = false;
        }

        if (_input.y == 0 && !controller.isGrounded)
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




    void HandleCrouchInput()
    {
        if (isSliding) return; // ignore crouch input during slide

        if (input.onFoot.Crouch.triggered)
        {
            if (isCrouching) StandUp();
            else Crouch();
        }

        if(isCrouching &&
            input.onFoot.Jump.triggered ||
            input.onFoot.Sprint.triggered) //when jumping or sprinting
        {
            StandUp();
        }


        // Start slide if sprinting, crouching, and moving forward
        if (isSprinting && !isSliding && input.onFoot.Crouch.triggered && input.onFoot.Movement.ReadValue<Vector2>().y > 0.1f)
        {
            isSliding = true;
        }
    }

    void Crouch()
    {
        isCrouching = true;
        _movementObject.Speed = crouchSpeed;
        controller.height = _movementObject.CrouchHeight;
    }

    void StandUp()
    {
        // Check if there's room to stand up
        RaycastHit hit;
        float castDistance = _movementObject.DefaultHeight - controller.height;
        Vector3 start = transform.position + Vector3.up * controller.height;

        if (!Physics.SphereCast(start, controller.radius, Vector3.up, out hit, castDistance))
        {
            isCrouching = false;
            _movementObject.Speed = standSpeed;
            controller.height = _movementObject.DefaultHeight;
        }
    }

    IEnumerator Slide(Vector2 inputDir)
    {
        isSprinting = false; //make sure sprint/crouch is disabled
        isCrouching = false;

        _movementObject.Speed = slideSpeed;
        playerVelocity.y = 0f;

        Vector2 lastDirection = Vector2.zero;

        while (slideTimer < slideDuration)
        {
            //make player smaller
            controller.height = _movementObject.CrouchHeight;

            //move in slide direction
            if (inputDir == Vector2.zero) inputDir = lastDirection;


            Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);
            controller.Move(transform.TransformDirection(moveDir) * slideSpeed * Time.deltaTime + playerVelocity * Time.deltaTime);

            //add gravity
            playerVelocity.y += _movementObject.Gravity * Time.deltaTime;

            //add to timer
            slideTimer += Time.deltaTime;

            lastDirection = inputDir;
            yield return null;
        }
        controller.height = _movementObject.DefaultHeight;
        _movementObject.Speed = standSpeed;
        isSliding = false;
        slideTimer = 0;
    }
}

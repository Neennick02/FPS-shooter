using System.Collections;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float jumpHeight = 3;
    Vector3 playerVelocity;
    public float speed = 5f;
    public float sprintSpeed = 10f;
    bool isGrounded;
    bool isSprinting = false;
    public float gravity = -9.8f;
    public int currentSpeed;


    // Ladder climbing
    bool isClimbing = false;
    Transform currentLadder;
    public float climbSpeed = 3f;

    // Crouch and slide
    bool isCrouching = false;
    bool isSliding = false;

    public float crouchHeight = 1f;
    float crouchSpeed = 2;
    float standingHeight;
    float targetHeight;
    Vector3 targetCenter;

    public float slideSpeed = 15f;
    public float slideDuration = 0.7f;
    float slideTimer;

    [Header("Scripts")]
    [SerializeField] InputManager input;
    [SerializeField] CharacterController controller;
    [SerializeField] PlayerLook playerLook;
    void Start()
    {
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
        HandleSlideTimer();
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    void NormalMovement(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        if (isSprinting)
        {
            controller.Move(transform.TransformDirection(moveDirection) * sprintSpeed * Time.deltaTime);
            currentSpeed = 2;
            isCrouching = false;
        }
        else
        {
            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
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
        playerVelocity.y += gravity * Time.deltaTime;

        //apply vertical velocity
        controller.Move(playerVelocity * Time.deltaTime);
    }




    //receive input from InputManager and apply to CharacterController
    public void ProcessMove(Vector2 input)
    {
        if (!isClimbing)
        {
            if (isSliding)
            {
                SlideMovement(input);
            }
            else
            {
                NormalMovement(input);

            }
        }
        else
        {
            ClimbLadder(input);
        }
    }



    void ClimbLadder(Vector2 _input)
    {
        //cancel crouch 
        isCrouching = false;


        // Only vertical movement on ladder
        Vector3 climbDirection = new Vector3(0, _input.y, 0);
        controller.Move(climbDirection * climbSpeed * Time.deltaTime);

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




    void SlideMovement(Vector2 input)
    {
        //// Slide forward direction
        //Vector3 slideDir = transform.forward;
        //controller.Move(slideDir * slideSpeed * Time.deltaTime);

        //// Gravity during slide
        //playerVelocity.y += gravity * Time.deltaTime;
        //controller.Move(playerVelocity * Time.deltaTime);
    }




    void HandleCrouchInput()
    {
        if (isSliding) return; // ignore crouch input during slide

        if (input.onFoot.Crouch.WasReleasedThisFrame())
        {
            if (isCrouching) StandUp();
            else Crouch();
        }

        // Start slide if sprinting, crouching, and moving forward
        if (isCrouching && isSprinting && !isSliding && input.onFoot.Movement.ReadValue<Vector2>().y > 0.1f)
        {
            StartSlide();
        }
    }

    void Crouch()
    {
        isCrouching = true;
        controller.height = crouchHeight;
    }

    void StandUp()
    {
        // Check if there's room to stand up
        RaycastHit hit;
        float castDistance = standingHeight - controller.height;
        Vector3 start = transform.position + Vector3.up * controller.height;

        if (!Physics.SphereCast(start, controller.radius, Vector3.up, out hit, castDistance))
        {
            isCrouching = false;
            controller.height = standingHeight;
        }
    }

    void StartSlide()
    {
        //isSliding = true;
        //slideTimer = slideDuration;
        //controller.height = crouchHeight / 2f;
        //Vector3 center = controller.center;
        //center.y = controller.height / 2f;
        //controller.center = center;
    }

    void HandleSlideTimer()
    {
        //    if (isSliding)
        //    {
        //        slideTimer -= Time.deltaTime;
        //        if (slideTimer <= 0f)
        //        {
        //            isSliding = false;
        //            if (isCrouching)
        //            {
        //                controller.height = crouchHeight;
        //                Vector3 center = controller.center;
        //                center.y = crouchHeight / 2f;
        //                controller.center = center;
        //            }
        //            else
        //            {
        //                controller.height = standingHeight;
        //                Vector3 center = controller.center;
        //                center.y = standingHeight / 2f;
        //                controller.center = center;
        //            }
        //        }
        //    }
        //}
    }
}

using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float jumpHeight = 3;
    CharacterController controller;
    Vector3 playerVelocity;
    public float speed = 5f;
    public float sprintSpeed = 10f;
    bool isGrounded;
    bool isSprinting = false;
    public float gravity = -9.8f;
    InputManager input;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<InputManager>();
        //makes cursor invisible during gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;


        if (input.onFoot.Sprint.triggered)
        {
            isSprinting = true;
        }
        else if (input.onFoot.Sprint.WasReleasedThisFrame())
        {
            isSprinting = false;
        }
    }

    //receive input from InputManager and apply to CharacterController
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        if (isSprinting)
        {
            controller.Move(transform.TransformDirection(moveDirection) * sprintSpeed * Time.deltaTime);
        }
        else
        {
            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        }

        if(controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }

        //handles gravity
        playerVelocity.y += gravity * Time.deltaTime;

        //apply vertical velocity
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }
}

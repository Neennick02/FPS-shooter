using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float jumpHeight = 3;
    CharacterController controller;
    Vector3 playerVelocity;
    public float speed = 5f;
    bool isGrounded;
    public float gravity = -9.8f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    //receive input from InputManager and apply to CharacterController
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

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

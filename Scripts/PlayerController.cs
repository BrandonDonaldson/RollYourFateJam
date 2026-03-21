using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private bool isGrounded = false;
    [SerializeField] private CollisionTrigger groundDetection;
    [SerializeField] private Rigidbody2D playerPhysics;

    [SerializeField] private Vector2 movementVel;
    [SerializeField] private Vector2 environmentVel;
    [SerializeField] private Vector2 naturalVel;
    [SerializeField] private Vector2 velocity;

    [SerializeField] private float gravityStrength = -9.6f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 25f;

    private KeyCode lastInput;
    private float timeSinceLastInput = 0.0f;

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            movementVel.y += gravityStrength;
        }

        velocity = movementVel + environmentVel + naturalVel;
        playerPhysics.linearVelocity = velocity;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D) && isGrounded)
        {
            movementVel.x = movementSpeed;
            lastInput = KeyCode.D;
        }
        if (Input.GetKey(KeyCode.A) && isGrounded)
        {
            movementVel.x = -movementSpeed;
            lastInput = KeyCode.A;
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A) && isGrounded || !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && isGrounded)
        {
            movementVel.x = 0.0f;
        }


        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            movementVel.y = jumpForce;
            lastInput = KeyCode.Space;
        }

        if (!Input.GetKey(KeyCode.Space) && isGrounded)
        {
            movementVel.y = 0;
        }
    }

    public void GroundCollider()
    {
        for (int i = 0; i < groundDetection.currentlyTouching.Count; i++)
        {
            if (groundDetection.currentlyTouching[i].tag == "Ground")
            {
                isGrounded = true;
                return;
            }
        }

        isGrounded = false;
    }
}

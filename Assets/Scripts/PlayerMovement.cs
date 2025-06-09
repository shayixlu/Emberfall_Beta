using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D AYA;
    private BoxCollider2D boxCollider;
    ControllerSupport defaultcontroller;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpMultiplier = 1.5f; // Double jump force multiplier
    [SerializeField] private LayerMask groundLayer;

    private bool canDoubleJump;
    private bool jumpRequested;

    private void Awake() // Get references
    {
        AYA = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        
        defaultcontroller = new ControllerSupport();
        defaultcontroller.Gameplay.Jump.performed += ctx => Jump();
    }

    private void Jump()
    {
        jumpRequested = true; // Set jump request flag
    }

    private void OnEnable()
    {
        defaultcontroller.Gameplay.Enable();
    }

    private void OnDisable()
    {
        defaultcontroller.Gameplay.Disable();
    }

    private void Update() // Check for jump input and set request flag
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate() // Horizontal movement
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        AYA.linearVelocity = new Vector2(horizontalInput * speed, AYA.linearVelocity.y);
        
        if (horizontalInput > 0.01f) // Flip player sprite
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
       
        bool grounded = isGrounded();  // Ground check
        if (grounded)
        {
            canDoubleJump = true;
        }

        if (jumpRequested) // Perform jump if requested
        {
            PerformJump(grounded);
            jumpRequested = false; // Reset jump request after processing
        }
    }

    private void PerformJump(bool grounded)
    {
        if (grounded)
        {
            Jump(jumpForce);
        }
        else if (canDoubleJump)
        {
            Jump(jumpForce * doubleJumpMultiplier);
            canDoubleJump = false; // Disable double jump after using it
        }
    }

    private void Jump(float force)
    {
        AYA.linearVelocity = new Vector2(AYA.linearVelocity.x, force);
    }

    private bool isGrounded() // BoxCast downwards with small tolerance
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);

        // Debug draw
        Debug.DrawLine(boxCollider.bounds.center, boxCollider.bounds.center + Vector3.down * 0.1f, Color.red);

        return raycastHit.collider != null;
    }
}

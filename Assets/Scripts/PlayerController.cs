using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Vector3 playerStartPos;
    [SerializeField] private int speed = 5; // Player movement speed
    [SerializeField] private float jumpForce = 5f; // Player jump force
    [SerializeField] private float raycastDistance = 1f; // Raycast distance for ground check
    [SerializeField] private float slopeCheckDistance = 1f; // Raycast distance for slope check
    [SerializeField] private LayerMask groundLayer; // Layer mask for ground detection
    private Rigidbody2D rb;
    private bool isGrounded = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
         transform.position = playerStartPos;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerMovements();
    }
    
    //Player Movements
    void PlayerMovements()
    {
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector2.right * speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movement += Vector2.left * speed;
        }

        // Apply movement, but adjust for slopes
        if (movement != Vector2.zero && rb.linearVelocity.magnitude < speed)
        {
            if (IsOnSlope())
            {
                // Apply a smaller force on slopes to prevent bouncing
                rb.AddForce(movement * 0.9f, ForceMode2D.Force);
            }
            else
            {
                rb.AddForce(movement, ForceMode2D.Force);
            }
        }
        
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }
    
    //Jump function is here
    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    //Groundcheck to make sure the player can only jump once
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);
        return hit.collider != null;
    }

    // Slope detection using raycast
    bool IsOnSlope()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down + Vector2.right * (Input.GetKey(KeyCode.D) ? 1 : -1), slopeCheckDistance, groundLayer);
        return hit.collider != null;
    }

    // For developers - visualize the raycasts
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down + Vector3.right) * slopeCheckDistance);
    }

    //**********************Trigger Zone********************************
    
    // Destroying the enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }

        //Player advances to next level
        if (other.gameObject.CompareTag("NextLevel"))
        {
            UIManager.instance.NextLevel();
        }
    }

    //**********************Collision Zone********************************

    // Getting Hit Animation 
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Animations.instance.PlayerOnOff();
        }

        //Reducing JumpForce on special ground
        if (other.gameObject.CompareTag("MetalGround"))
        {
            jumpForce = 13;
        }
        
    }

    //Return to original JumpForce
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MetalGround"))
        {
            jumpForce = 20;
        }
    }
}

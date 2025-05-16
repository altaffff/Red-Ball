using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;
    public GameObject playerPrefab; // Prefab of the player GameObject
    private Vector3 startPos; // Starting position of the enemy
    [SerializeField] private float distance = 10f; // Max distance the enemy can move
    [SerializeField] private int speed = 2; // Speed of the enemy
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private float move; // Current movement value based on PingPong
    private bool facingRight = true; // Direction flag to track enemy's movement

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

    private void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on the enemy!");
        }

        // Ensure the distance is not zero or negative
        if (distance <= 0)
        {
            Debug.LogError("Invalid distance value. Distance must be greater than 0.");
        }
    }

    private void Update()
    {
        EnemyMovement();
        FlipSprite();
    }

    // PingPong Function to move the enemy back and forth
    void EnemyMovement()
    {
        // Move between startPos and the distance specified using PingPong
        move = Mathf.PingPong(Time.time * speed, distance);

        // Move the enemy based on the calculated move value
        transform.position = new Vector3(startPos.x + move, transform.position.y, transform.position.z);
    }

    // Flip the enemy sprite based on the movement direction
    void FlipSprite()
    {
        // When the enemy is moving to the right
        if (move > 0.1f && !facingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Flip to right
            facingRight = true; // Set direction to right
        }
        // When the enemy is moving to the left
        else if (move < -0.1f && facingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f); // Flip to left
            facingRight = false; // Set direction to left
        }
    }

    //*******************Collision Zone******************************

    // Killing the player and health system  
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIManager.instance.numberOfHearts--;
            Animations.instance.PlayerOnOff();

            if (UIManager.instance.numberOfHearts <= 0)
            {
                UIManager.instance.GameOver();
            }

            // Save progress
            UIManager.instance.SaveProgress();
        }
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;

    public float strength = 5f;
    public float gravity = -9.81f;
    public float tilt = 5f;

    private Vector3 direction;
    private AudioSource jumpAudioSource;

    private bool sizeIncreased = false; // Flag to track if player size has increased
    private bool resetSizePending = false; // Flag to track if size reset is pending
    private float sizeResetDelay = 3f; // Delay in seconds before resetting the size
    private bool canDestroy = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumpAudioSource = transform.Find("JumpAudio").GetComponent<AudioSource>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
        sizeIncreased = false; // Reset the sizeIncreased flag
        ResetPlayerSize(); // Reset the player's size
    }

    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
            PlayJumpSound();
        }

        // Apply gravity and update the position
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        // Tilt the bird based on the direction
        Vector3 rotation = transform.eulerAngles;
        rotation.z = direction.y * tilt;
        transform.eulerAngles = rotation;

        if (resetSizePending)
        {
            sizeResetDelay -= Time.deltaTime;
            if (sizeResetDelay <= 0f)
            {
                ResetPlayerSize();
                
            }
        }
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }

        if (spriteIndex < sprites.Length && spriteIndex >= 0)
        {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (sizeIncreased)
            {
                Destroy(other.gameObject);
            }
            else
            {
                FindObjectOfType<GameManager>().GameOver();
                ResetPlayerSize(); // Reset the player's size on collision with an obstacle
            }
        }
        else if (other.gameObject.CompareTag("GroundTag"))
        {
            if (!sizeIncreased)
            {
                FindObjectOfType<GameManager>().GameOver();
                ResetPlayerSize(); // Reset the player's size on collision with an obstacle
            }

        }
        else if(other.gameObject.CompareTag("Scoring"))
        {
            FindObjectOfType<GameManager>().IncreaseScore();
        }
        else if (other.gameObject.CompareTag("poweruptag"))
        {
           ActivatePowerUP();   
        }
    }

    private void PlayJumpSound()
    {
        if (jumpAudioSource != null && jumpAudioSource.clip != null)
            jumpAudioSource.PlayOneShot(jumpAudioSource.clip);
    }

    private void IncreasePlayerSize()
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); // Increase the player's scale
        sizeIncreased = true; // Set the sizeIncreased flag to true
        resetSizePending = true; // Set the resetSizePending flag to true
        sizeResetDelay = 3f; // Reset the size reset delay
    }

    private void ResetPlayerSize()
    {
        transform.localScale = Vector3.one; // Reset the player's size
        canDestroy = false; 
        resetSizePending = false;
    }

    public void ActivatePowerUP()
    {
        IncreasePlayerSize();
        canDestroy = true;
    }
}

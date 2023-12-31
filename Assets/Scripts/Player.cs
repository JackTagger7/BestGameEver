﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;
    public Text countDownText;

    public float powerUpSize = 3f;
    public float strength = 5f;
    public float gravity = -9.81f;
    public float tilt = 5f;
    public float powerUpDuration;
    public float sPowerUpSize = 3f;
    public float sPowerUpDuration;
    public bool paused = true;
    public UnityEngine.UI.Image countdownImage;

    private Vector3 direction;
    private AudioSource jumpAudioSource;
    public AudioClip powerUpCollisionSound;
    private AudioSource powerUpAudioSource;
    private AudioSource sPowerUpAudioSource;
    public AudioClip sPowerUpCollisionSound;

    private float sizeResetDelay = 0f; // Delay in seconds before resetting the size
    private bool poweredUp = false;
    private bool sPoweredUp = false;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumpAudioSource = transform.Find("JumpAudio").GetComponent<AudioSource>();

        powerUpAudioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component for power-up collision sound
        powerUpAudioSource.playOnAwake = false;
        powerUpAudioSource.spatialBlend = 0f;
        sPowerUpAudioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component for spower-up collision sound
        sPowerUpAudioSource.playOnAwake = false;
        sPowerUpAudioSource.spatialBlend = 0f;
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);

    }

    private void Start()
    {

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
        poweredUp = false; // Reset the sizeIncreased flag
        DeactivatePowerUp(); // Reset the player's size
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
            PlayJumpSound();
        }
        if (sizeResetDelay > 0)
        {
            countDownText.text = sizeResetDelay.ToString("0.0");
            countdownImage.GetComponent<UnityEngine.UI.Image>().enabled = true;
        }
        else
        {
            countDownText.text = "";
            countdownImage.GetComponent<UnityEngine.UI.Image>().enabled = false;
        }


        if (!paused)
        {
            // Apply gravity and update the position
            direction.y += gravity * Time.deltaTime;
            Vector2 position = transform.position + direction * Time.deltaTime;
            float y = poweredUp ? Mathf.Max(position.y, -3.2f) : position.y;
            transform.position = new Vector2(position.x, y);

            // Tilt the bird based on the direction
            Vector3 rotation = transform.eulerAngles;
            rotation.z = direction.y * tilt;
            transform.eulerAngles = rotation;

        }


        if (poweredUp)
        {
            sizeResetDelay -= Time.deltaTime;
            if (sizeResetDelay <= 0f)
            {
                DeactivatePowerUp();

            }
        }
        if (sPoweredUp)
        {
            sizeResetDelay -= Time.deltaTime;
            if (sizeResetDelay <= 0f)
            {
                DeactivateSPowerUp();
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
            if (poweredUp)
            {
                Destroy(other.gameObject);
            }
            else
            {
                FindObjectOfType<GameManager>().GameOver();
                DeactivatePowerUp(); // Reset the player's size on collision with an obstacle
            }
        }
        else if (other.gameObject.CompareTag("GroundTag"))
        {
            if (!poweredUp)
            {
                FindObjectOfType<GameManager>().GameOver();
                DeactivatePowerUp(); // Reset the player's size on collision with an obstacle
            }

        }
        else if (other.gameObject.CompareTag("Scoring"))
        {
            FindObjectOfType<GameManager>().IncreaseScore();
        }
        else if (other.gameObject.CompareTag("poweruptag"))
        {
            ActivatePowerUp();
            Destroy(other.gameObject);
            if (powerUpCollisionSound != null)
            {
                powerUpAudioSource.PlayOneShot(powerUpCollisionSound);
            }

        }
        else if (other.gameObject.CompareTag("SPowerUpTag"))
        {
            ActivateSPowerUp();
            Destroy(other.gameObject);
            if (sPowerUpCollisionSound != null)
            {
                sPowerUpAudioSource.PlayOneShot(sPowerUpCollisionSound);
            }
        }
    }

    private void PlayJumpSound()
    {
        if (jumpAudioSource != null && jumpAudioSource.clip != null)
            jumpAudioSource.PlayOneShot(jumpAudioSource.clip);
    }
    private void DeactivatePowerUp()
    {
        transform.localScale = Vector3.one; // Reset the player's size
        poweredUp = false;
    }
    private void DeactivateSPowerUp()
    {
        transform.localScale = Vector3.one; // Reset the player's size
        sPoweredUp = false;
    }

    public void ActivatePowerUp()
    {
        DeactivateSPowerUp();
        sizeResetDelay = powerUpDuration; // Reset the size reset delay
        transform.localScale = new Vector3(powerUpSize, powerUpSize, powerUpSize); // Increase the player's scale
        poweredUp = true;
    }
    public void ActivateSPowerUp()
    {
        DeactivatePowerUp();
        sizeResetDelay = sPowerUpDuration; // Reset the size reset delay
        transform.localScale = new Vector3(sPowerUpSize, sPowerUpSize, sPowerUpSize); // Increase the player's scale
        sPoweredUp = true;
    }
}

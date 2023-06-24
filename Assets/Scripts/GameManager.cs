using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Player player;
    private Spawner spawner;

    public Text scoreText;
    public GameObject playButton;
    public GameObject gameOver;
    public HighScoreManager highScoreManager;
    public int score { get; private set; }
    public bool gameStarted { get; private set; }

    public AudioSource audioSource;
    public AudioClip scoreIncreaseSound;
    public AudioClip gameOverSound;
    public AudioClip score20Sound;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();
        highScoreManager = FindObjectOfType<HighScoreManager>();
        highScoreManager.HideHighScore();

        Pause();
    }

    public void StartGame()
    {
        spawner.StartSpawning();
        highScoreManager.HideHighScore();
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
        PowerUPScript[] powerUPScripts = FindObjectsOfType<PowerUPScript>();

        for (int i = 0;i < powerUPScripts.Length; i++)
        {
            Destroy(powerUPScripts[i].gameObject);  
        }
        gameStarted = true;
    }

    public void GameOver()
    {
        highScoreManager.ShowHighScore();
        playButton.SetActive(true);
        gameOver.SetActive(true);
        spawner.StopSpawning();

        Pause();

        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        gameStarted = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();

        if (scoreIncreaseSound != null)
        {
            audioSource.PlayOneShot(scoreIncreaseSound);
        }
        if (score == 20 && score20Sound != null)
        {
            audioSource.PlayOneShot(score20Sound);
        }

        highScoreManager.UpdateHighScore(score);
    }
}

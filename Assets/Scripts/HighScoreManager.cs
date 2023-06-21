using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public Text highScoreText;

    private int highScore;
    private bool isGameOver;

    private void Start()
    {
        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void UpdateHighScore(int currentScore)
    {
        if (currentScore > highScore)
        {
            // Update the high score value
            highScore = currentScore;

            // Save the new high score to PlayerPrefs
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    public void ShowHighScore()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
        highScoreText.gameObject.SetActive(true);
    }

    public void HideHighScore()
    {
        highScoreText.gameObject.SetActive(false);
    }
}

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

            // Set the text color to indicate a new high score
            highScoreText.text = "New High Score! " + highScore.ToString();
            highScoreText.color = Color.yellow;

            // Save the new high score to PlayerPrefs
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        else
        {
            highScoreText.text = "Previous High Score: " + highScore.ToString();
            highScoreText.color = Color.black;
        }
    }


    public void ShowHighScore()
    {
        highScoreText.gameObject.SetActive(true);
    }

    public void HideHighScore()
    {
        highScoreText.gameObject.SetActive(false);
    }
}

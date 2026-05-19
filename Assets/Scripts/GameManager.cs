using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI starText;

    private bool isPaused = false;
    private float score = 0f;
    private int coinCount = 0;
    private bool isGameOver = false;
    public GameObject pausePanel;
    
    public void StopScoring()
    {
        isGameOver = true;
    }

    
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pausePanel.SetActive(isPaused);
    }

    void Awake()
    {
        instance = this;
    }

   void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            TogglePause();
            return;
        }
        if (!isGameOver)
        {
            score += Time.deltaTime * 10f;
            scoreText.text = "Distance: " + Mathf.FloorToInt(score) + "m";
        }
    }

    public void AddCoin()
    {
        coinCount++;
        if (coinText != null)
            coinText.text = "Total Stars: " + coinCount;
        if (starText != null)
            starText.text = "Stars: " + coinCount;
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;

        int finalScore = Mathf.FloorToInt(score) + coinCount * 10;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (finalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            highScore = finalScore;
        }

        if (gameOverScoreText != null)
            gameOverScoreText.text = "Distance: " + finalScore + "m";
        if (highScoreText != null)
            highScoreText.text = "Best: " + highScore;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
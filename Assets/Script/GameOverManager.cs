using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timeText;
    public Button restartButton;
    public Button mainMenuButton;

    private void Start()
    {
        // Hiển thị text game over
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER";
        }

        // Hiển thị số coins thu thập được
        if (coinsText != null && GameManager.Instance != null)
        {
            coinsText.text = $"● Coins Collected: {GameManager.Instance.coins}";
        }

        // Hiển thị tổng thời gian đã chơi
        if (timeText != null && GameManager.Instance != null)
        {
            int minutes = Mathf.FloorToInt(GameManager.Instance.totalTimePlayed / 60f);
            int seconds = Mathf.FloorToInt(GameManager.Instance.totalTimePlayed % 60f);
            timeText.text = $"[Total Time: {minutes:D2}:{seconds:D2}]";
        }

        // Gắn sự kiện cho các nút
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(Restart);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(BackToMainMenu);
        }
    }

    private void Restart()
    {
        
        if (GameManager.Instance != null)
        {
            // Restart game ngay lập tức từ màn 1.1
            GameManager.Instance.RestartGame();
        }
        else
        {
            // Nếu không có GameManager, load trực tiếp
            SceneManager.LoadScene("1.1");
        }
    }

    private void BackToMainMenu()
    {
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        
        SceneManager.LoadScene("MainMenu");
    }
}

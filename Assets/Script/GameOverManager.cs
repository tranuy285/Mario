using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button mainMenuButton;

    private void Start()
    {
        // Hiển thị text game over
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER";
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

        // Tự động restart sau 5 giây nếu không có interaction
        Invoke(nameof(Restart), 5f);
    }

    private void Restart()
    {
        CancelInvoke(); // Hủy auto restart
        
        if (GameManager.Instance != null)
        {
            // Reset game mới
            Destroy(GameManager.Instance.gameObject);
        }
        
        SceneManager.LoadScene("MainMenu");
    }

    private void BackToMainMenu()
    {
        CancelInvoke();
        
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        
        SceneManager.LoadScene("MainMenu");
    }
}

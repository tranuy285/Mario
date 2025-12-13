using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI scoreText;
    public Button continueButton;
    public Button mainMenuButton;

    private void Start()
    {
        // Hiển thị text chúc mừng
        if (winnerText != null)
        {
            winnerText.text = "CONGRATULATIONS!";
        }

        // Hiển thị điểm số (coins thu thập được)
        if (scoreText != null && GameManager.Instance != null)
        {
            scoreText.text = $"Total Coins: {GameManager.Instance.coins}";
        }

        // Gắn sự kiện cho các nút
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(Continue);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(BackToMainMenu);
        }
    }

    private void Continue()
    {
        // Có thể load world tiếp theo hoặc về menu
        BackToMainMenu();
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

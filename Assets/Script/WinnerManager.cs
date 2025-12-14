using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timeText;
    public Button continueButton;

    private void Start()
    {
        // Hiển thị text chúc mừng
        if (winnerText != null)
        {
            winnerText.text = "CONGRATULATIONS!";
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

        // Gắn sự kiện cho nút Continue
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(Continue);
        }
    }

    private void Continue()
    {
        // Về main menu và destroy GameManager để reset toàn bộ
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        
        SceneManager.LoadScene("MainMenu");
    }
}

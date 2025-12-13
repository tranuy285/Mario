using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI worldText;
    public TextMeshProUGUI timeText;

    private int timeRemaining = 400;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f); // Cập nhật timer mỗi giây
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Cập nhật toàn bộ UI
    public void UpdateUI()
    {
        if (GameManager.Instance != null)
        {
            UpdateLives(GameManager.Instance.lives);
            UpdateCoins(GameManager.Instance.coins);
            UpdateWorld(GameManager.Instance.world, GameManager.Instance.stage);
        }
    }

    // Cập nhật hiển thị số mạng
    public void UpdateLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = $"x{lives}";
        }
    }

    // Cập nhật hiển thị số coin
    public void UpdateCoins(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = $"x{coins:D2}"; // Format 2 chữ số (01, 02, ..., 99)
        }
    }

    // Cập nhật hiển thị world-stage
    public void UpdateWorld(int world, int stage)
    {
        if (worldText != null)
        {
            worldText.text = $"WORLD {world}-{stage}";
        }
    }

    // Cập nhật timer
    private void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining--;
            if (timeText != null)
            {
                timeText.text = timeRemaining.ToString("D3"); // Format 3 chữ số
            }

            // Hết thời gian thì player chết
            if (timeRemaining == 0)
            {
                GameManager.Instance?.LevelReset();
            }
        }
    }

    // Reset timer khi bắt đầu level mới
    public void ResetTimer(int time = 400)
    {
        timeRemaining = time;
        if (timeText != null)
        {
            timeText.text = timeRemaining.ToString("D3");
        }
    }
}

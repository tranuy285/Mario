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

    private int timeRemaining = 120;

    private void Awake()
    {
        // UIManager mới mỗi scene
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        
        // Reset timer về 120s mỗi khi load scene (kể cả respawn)
        timeRemaining = 120;
        UpdateTimeDisplay(timeRemaining);
        
        // Bắt đầu đếm ngược
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f);
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
            livesText.text = $"♥ x{lives}"; // Heart symbol (Unicode basic)
        }
    }

    // Cập nhật hiển thị số coin
    public void UpdateCoins(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = $"● x{coins:D3}"; // Circle cho coin - Format 3 chữ số (001, 002...)
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
            UpdateTimeDisplay(timeRemaining);

            // Hết thời gian thì player chết
            if (timeRemaining == 0)
            {
                GameManager.Instance?.LevelReset();
            }
        }
    }

    // Cập nhật hiển thị timer
    private void UpdateTimeDisplay(int time)
    {
        if (timeText != null)
        {
            timeText.text = $"[{time:D3}s]";
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int world { get; private set; } // Biến lưu trữ thế giới hiện tại
    public int stage { get; private set; } // Biến lưu trữ màn chơi hiện tại
    public int lives { get; private set; } // Biến lưu trữ số mạng của người chơi
    public int coins { get; private set; } // Biến lưu trữ số coin của người chơi

    private void Awake()
    {
        // Đảm bảo chỉ có một instance của GameManager tồn tại
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ GameManager khi chuyển cảnh
        }
        else
        {
            Destroy(gameObject); // Hủy bỏ các instance phụ
        }
    }

    // Giải phóng instance khi GameManager bị hủy 
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        // Khởi tạo các thành phần cần thiết khi trò chơi bắt đầu
        Application.targetFrameRate = 60;
        NewGame(); // Bắt đầu trò chơi mới
    }

    private void NewGame()
    {
        // Thiết lập trạng thái ban đầu cho trò chơi mới
        lives = 3; // bắt đầu với 3 mạng
        coins = 0; // bắt đầu với 0 coin

        LoadStage(1, 1); // load màn chơi đầu tiên
    }

    // Hàm load màn chơi 
    public void LoadStage(int world, int stage)
    {
        this.world = world;
        this.stage = stage;
        // Thêm mã để tải cảnh dựa trên world và stage
        SceneManager.LoadScene($"{world}.{stage}");
        
        // // Cập nhật UI sau khi load scene
        // Invoke(nameof(UpdateUI), 0.1f);
    }

    // Hàm load màn tiếp theo (thiết kế tạm thời cho world 1 có 3 stage. Nếu chỉ có 1 stage thì bỏ hàm này)
    public void LoadNextStage()
    {
        if (world == 1 && stage < 3)
        {
            LoadStage(world, stage + 1);
        } else if (world == 1 && stage == 3)
        {
            SceneManager.LoadScene("Winner");
        }
    }

    // Tạo độ trễ trước khi xử lý người chơi chết, tránh việc load lại màn chơi ngay lập tức
    public void LevelReset(float delay)
    {
        Invoke(nameof (LevelReset), delay);
    }

    // Hàm xử lý khi người chơi chết sẽ reset level hoặc game over nếu hết mạng
    public void LevelReset()
    {
        lives--;
        
        // // Cập nhật UI
        // if (UIManager.Instance != null)
        // {
        //     UIManager.Instance.UpdateLives(lives);
        // }

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            LoadStage(world, stage); // Tải lại màn chơi hiện tại
        }
    }

    // Hàm helper để cập nhật UI
    private void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateUI();
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    // Hàm thêm coin và xử lý khi đủ 100 coin sẽ cộng thêm mạng
    public void AddCoin()
    {
        coins++;

        // // Cập nhật UI
        // if (UIManager.Instance != null)
        // {
        //     UIManager.Instance.UpdateCoins(coins);
        // }

        if (coins == 100)
        {
            AddLife();
            coins = 0;
        }
    }

    // Hàm thêm mạng cho người chơi
    public void AddLife()
    {
        lives++;
        
        // // Cập nhật UI
        // if (UIManager.Instance != null)
        // {
        //     UIManager.Instance.UpdateLives(lives);
        // }
    }
}

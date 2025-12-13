using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button startButton;
    public Button quitButton;

    private void Start()
    {
        // Gắn sự kiện cho các nút
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    private void StartGame()
    {
        // Bắt đầu game mới
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadStage(1, 1);
        }
        else
        {
            // Nếu chưa có GameManager, load trực tiếp
            SceneManager.LoadScene("1.1");
        }
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private RectTransform panelBoks; // Panel/kotak utama di dalam UI

    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Settings")]
    [Tooltip("Nama scene Main Menu Anda")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void OnDestroy()
    {
        if (restartButton != null)
            restartButton.onClick.RemoveListener(RestartGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveListener(ReturnToMainMenu);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel == null) return;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("level failure");
        }

        gameOverPanel.SetActive(true);

        if (panelBoks != null)
        {
            panelBoks.localScale = Vector3.zero;
            panelBoks.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
        }

        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        AudioManager.Instance.PlaySFX("button_click");

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ReturnToMainMenu()
    {
        AudioManager.Instance.PlaySFX("button_click");

        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM("MainMenu_BGM");
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
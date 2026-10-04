using UnityEngine;

public class EndSceneUI : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Nama scene Main Menu untuk kembali saat layar disentuh")]
    [SerializeField] private string mainMenuSceneName = "UIMenu";

    [Header("Audio Settings")]
    [Tooltip("Nama BGM yang diputar khusus di End Scene")]
    [SerializeField] private string endSceneBGMName = "EndScene_BGM";

    private bool isTransitioning = false;

    private void Start()
    {
        if (AudioManager.Instance != null && !string.IsNullOrEmpty(endSceneBGMName))
        {
            AudioManager.Instance.PlayBGM(endSceneBGMName);
        }
    }

    private void Update()
    {
        if (!isTransitioning && Input.anyKeyDown)
        {
            isTransitioning = true;
            ReturnToMenu();
        }
    }

    private void ReturnToMenu()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadSceneWithLoading(mainMenuSceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
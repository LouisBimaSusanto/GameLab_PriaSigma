using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Indeks Panel")]
    public GameObject indeksPanel;
    public GameObject mainMenuGroup;

    [Header("Scene")]
    public string gameSceneName = "GameScene";

    [Header("Audio Settings")]
    public string mainMenuBGMName = "MainMenu_BGM"; 
    public string inGameBGMName = "Gameplay_BGM";   
    public string clickSFXName = "button_click";

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            Debug.Log("Berhasil menemukan AudioManager, memutar BGM: " + mainMenuBGMName);
            AudioManager.Instance.PlayBGM(mainMenuBGMName);
        }
        else
        {
            Debug.LogError("GAGAL: AudioManager tidak ditemukan atau belum aktif di Scene!");
        }
    }

    public void StartGame()
    {
        PlayClickSound();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM(inGameBGMName);
        }

        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenIndeks()
    {
        PlayClickSound();
        indeksPanel.SetActive(true);
        mainMenuGroup.SetActive(false);
    }

    public void CloseIndeks()
    {
        PlayClickSound();
        indeksPanel.SetActive(false);
        mainMenuGroup.SetActive(true);
    }

    public void ExitGame()
    {
        PlayClickSound();
        Debug.Log("Keluar Game");
        Application.Quit();
    }

    private void PlayClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clickSFXName);
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Indeks Panel")]
    public GameObject indeksPanel;
    public GameObject mainMenuGroup;

    [Header("Scene")]
    public string gameSceneName = "GameScene";

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenIndeks()
    {
        indeksPanel.SetActive(true);
        mainMenuGroup.SetActive(false);
    }

    public void CloseIndeks()
    {
        indeksPanel.SetActive(false);
        mainMenuGroup.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Keluar Game");
        Application.Quit();
    }
}
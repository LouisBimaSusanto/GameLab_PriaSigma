using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Credits UI")]
    public CanvasGroup creditsGroup;

    [Header("Scene")]
    public string gameSceneName = "GameScene";

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenCredits()
    {
        StartCoroutine(FadeIn());
    }

    public void CloseCredits()
    {
        StartCoroutine(FadeOut());
    }

    public void ExitGame()
    {
        Debug.Log("Keluar Game");
        Application.Quit();
    }

    IEnumerator FadeIn()
    {
        creditsGroup.gameObject.SetActive(true);
        creditsGroup.alpha = 0;

        while (creditsGroup.alpha < 1)
        {
            creditsGroup.alpha += Time.deltaTime * 2;
            yield return null;
        }

        creditsGroup.alpha = 1;
    }

    IEnumerator FadeOut()
    {
        while (creditsGroup.alpha > 0)
        {
            creditsGroup.alpha -= Time.deltaTime * 2;
            yield return null;
        }

        creditsGroup.alpha = 0;
        creditsGroup.gameObject.SetActive(false);
    }
}
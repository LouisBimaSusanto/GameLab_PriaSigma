using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [Header("Video Settings")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Tooltip("Nama scene gameplay yang dituju setelah cutscene selesai (misal: Stage1)")]
    [SerializeField] private string nextSceneName = "Stage1";

    [Header("Audio Settings")]
    [Tooltip("BGM yang diputar saat transisi ke gameplay dimulai")]
    [SerializeField] private string gameplayBGMName = "Gameplay_BGM";

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBGM();
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        FinishCutscene();
    }

    public void SkipCutscene()
    {
        if (SceneTransition.Instance != null && !SceneTransition.Instance.IsTransitioning)
        {
            if (videoPlayer != null) videoPlayer.Stop();
            FinishCutscene();
        }
    }

    private void FinishCutscene()
    {
        if (AudioManager.Instance != null && !string.IsNullOrEmpty(gameplayBGMName))
        {
            AudioManager.Instance.PlayBGM(gameplayBGMName);
        }

        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadSceneWithLoading(nextSceneName);
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}
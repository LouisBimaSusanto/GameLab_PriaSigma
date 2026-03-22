using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private CinemachineCamera vcam;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            vcam = Object.FindFirstObjectByType<CinemachineCamera>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && vcam != null)
        {
            vcam.Follow = player.transform;
        }
    }
}
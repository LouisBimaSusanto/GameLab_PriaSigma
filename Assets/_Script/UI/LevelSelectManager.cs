using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    // Tombol Level: transisi -> Loading screen -> transisi -> scene level
    public void LoadLevel(string sceneName)
    {
        SceneTransition.Instance.LoadSceneWithLoading(sceneName);
    }

    public void BackToMenu()
    {
        SceneTransition.Instance.LoadScene("UIMenu"); // sesuaikan nama scene menu kamu
    }
}
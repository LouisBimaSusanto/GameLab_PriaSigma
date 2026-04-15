using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    public float timerToAdd = 10f;
    public string nextSceneName;

    private bool isUsed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isUsed) return;

        if (collision.CompareTag("Player"))
        {
            Timer timer = Object.FindAnyObjectByType<Timer>();

            if (timer != null)
            {
                timer.AddTime(timerToAdd);
            }

            isUsed = true;

            //Change Scene
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

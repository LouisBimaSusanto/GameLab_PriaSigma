using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float timerToAdd = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (GameFlowController.Instance.CurrentState != GameState.Driving) return;

        Timer.Instance?.AddTime(timerToAdd);
        GameFlowController.Instance.EnterSortingPhase();
    }
}
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float timerToAdd = 10f;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;
        if (!collision.CompareTag("Player")) return;
        if (GameFlowController.Instance.CurrentState != GameState.Driving) return;

        hasTriggered = true;
        Timer.Instance?.AddTime(timerToAdd);
        GameFlowController.Instance.EnterSortingPhase();
    }

    // Reset saat player keluar dari base (siap untuk siklus berikutnya)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        hasTriggered = false;
    }
}
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float timerToAdd = 10f;
    [SerializeField] private Collider2D checkpointCollider;
    [SerializeField] private GameObject visualIndicator;

    private bool hasTriggered = false;

    private void Start()
    {
        SetActive(false);

        if (StageManager.Instance != null)
            StageManager.Instance.OnAllTrashCollected += OnTrashAllCollected;
    }

    private void OnDestroy()
    {
        if (StageManager.Instance != null)
            StageManager.Instance.OnAllTrashCollected -= OnTrashAllCollected;
    }

    private void OnTrashAllCollected()
    {
        SetActive(true);
        Debug.Log("[CheckPoint] Collider aktif!");
    }

    private void SetActive(bool active)
    {
        if (checkpointCollider != null)
            checkpointCollider.enabled = active;
        if (visualIndicator != null)
            visualIndicator.SetActive(active);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;
        if (!collision.CompareTag("Player")) return;
        if (GameFlowController.Instance.CurrentState != GameState.Driving) return;

        hasTriggered = true;

        Timer.Instance?.AddTime(timerToAdd);

        float snapshot = Timer.Instance != null ? Timer.Instance.timeRemaining : 0f;
        Debug.Log($"[CheckPoint] Snapshot timeRemaining: {snapshot}");

        SortingResultUI.Instance.BeginSession(snapshot);

        GameFlowController.Instance.EnterSortingPhase();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        hasTriggered = false;
    }

    public void ResetCheckpoint()
    {
        hasTriggered = false;
        SetActive(false);
    }
}
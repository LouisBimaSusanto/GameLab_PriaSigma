using UnityEngine;
using System;
using System.Collections;

public enum GameState { Driving, Sorting }

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CarController playerCar;
    [SerializeField] private PlayerRespawn playerRespawn;
    [SerializeField] private GameObject sortingUI;
    [SerializeField] private CheckPoint checkpoint;

    public GameState CurrentState { get; private set; } = GameState.Driving;
    public event Action<GameState> OnStateChanged;

    private bool allTrashNotified = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (sortingUI != null) sortingUI.SetActive(false);
    }

    private void Start()
    {
        if (Timer.Instance != null)
            Timer.Instance.OnTimeUp += EndGame;

        StartCoroutine(SubscribeWhenReady());
    }

    private IEnumerator SubscribeWhenReady()
    {
        yield return new WaitUntil(() => PlayerInventory.Instance != null);
        PlayerInventory.Instance.OnInventoryChanged += CheckAllItemsCollected;
        Debug.Log("[GameFlow] Subscribe OnInventoryChanged berhasil.");
    }

    private void OnDestroy()
    {
        if (Timer.Instance != null)
            Timer.Instance.OnTimeUp -= EndGame;
        if (PlayerInventory.Instance != null)
            PlayerInventory.Instance.OnInventoryChanged -= CheckAllItemsCollected;
        if (Instance == this)
            Instance = null;
    }

    private void CheckAllItemsCollected()
    {
        if (CurrentState == GameState.Sorting) return;
        if (allTrashNotified) return;

        CollectibleItem[] allItems = FindObjectsOfType<CollectibleItem>();
        if (allItems.Length == 0) return;

        foreach (var item in allItems)
            if (!item.IsCaptured) return;

        allTrashNotified = true;
        Debug.Log("[GameFlow] Semua sampah terkumpul!");
        StageManager.Instance.NotifyAllTrashCollected();
    }

    public void EnterSortingPhase()
    {
        if (CurrentState == GameState.Sorting) return;

        CurrentState = GameState.Sorting;

        IrisTransition.Instance.PlayTransition(
            onMidpoint: () =>
            {
                PauseGameplay();
                sortingUI.SetActive(true);
                OnStateChanged?.Invoke(CurrentState);
            }
        );
    }

    public void ExitSortingPhase()
    {
        if (CurrentState == GameState.Driving) return;

        IrisTransition.Instance.PlayTransition(
            onMidpoint: () =>
            {
                sortingUI.SetActive(false);
                playerRespawn.Respawn();
                StageManager.Instance.AdvanceStage();
                CurrentState = GameState.Driving;
                allTrashNotified = false;

                if (checkpoint != null)
                    checkpoint.ResetCheckpoint();

                OnStateChanged?.Invoke(CurrentState);
            },
            onComplete: () =>
            {
                ResumeGameplay();
                Timer.Instance?.ResetTimer(60f);
            }
        );
    }

    private void PauseGameplay()
    {
        if (playerCar == null) return;
        playerCar.CanControl = false;
        Rigidbody2D rb = playerCar.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void ResumeGameplay()
    {
        if (playerCar == null) return;
        Rigidbody2D rb = playerCar.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Dynamic;
        playerCar.CanControl = true;
    }

    private void EndGame()
    {
        if (CurrentState == GameState.Sorting) return;
        StageManager.Instance.SaveProgress();
        IrisTransition.Instance.CloseOnly(onComplete: () =>
        {
            // TODO: Game Over UI
        });
    }
}
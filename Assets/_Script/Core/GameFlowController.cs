using UnityEngine;
using System;

public enum GameState
{
    Driving,
    Sorting
}

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CarController playerCar;
    [SerializeField] private PlayerRespawn playerRespawn;

    [Header("Sorting UI")]
    [SerializeField] private GameObject sortingUI;

    public GameState CurrentState { get; private set; } = GameState.Driving;

    public event Action<GameState> OnStateChanged;

    private bool sortingStarted = false;

    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Pastikan Sorting UI mati ketika game dimulai
        if (sortingUI != null)
        {
            sortingUI.SetActive(false);
        }
    }

    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        if (Timer.Instance != null)
        {
            Timer.Instance.OnTimeUp += EndGame;
        }

        if (StageManager.Instance != null)
        {
            StageManager.Instance.OnAllTrashCollected += EnterSortingPhase;
        }

        Debug.Log("[GameFlow] GameFlowController READY.");
        Debug.Log("[GameFlow] Sorting UI = " +
                  (sortingUI != null ? sortingUI.name : "NULL"));
    }

    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        // Removed polling to use StageManager events instead.
    }

    // =========================================================
    // ENTER SORTING
    // =========================================================

    public void EnterSortingPhase()
    {
        if (sortingStarted)
        {
            Debug.LogWarning(
                "[GameFlow] Sorting sudah pernah dimulai."
            );

            return;
        }

        if (CurrentState == GameState.Sorting)
        {
            Debug.LogWarning(
                "[GameFlow] CurrentState sudah Sorting."
            );

            return;
        }

        // =====================================================
        // VALIDATE UI
        // =====================================================

        if (sortingUI == null)
        {
            Debug.LogError(
                "[GameFlow] ERROR: Sorting UI belum di-assign!"
            );

            return;
        }

        // =====================================================
        // VALIDATE INVENTORY
        // =====================================================

        if (PlayerInventory.Instance == null)
        {
            Debug.LogError(
                "[GameFlow] ERROR: PlayerInventory.Instance NULL!"
            );

            return;
        }

        // =====================================================
        // START SORTING
        // =====================================================

        sortingStarted = true;
        CurrentState = GameState.Sorting;

        Debug.Log(
            "[GameFlow] ENTER SORTING PHASE"
        );

        // =====================================================
        // PAUSE TIMER
        // =====================================================

        if (Timer.Instance != null)
        {
            Timer.Instance.PauseTimer();

            Debug.Log(
                "[GameFlow] Timer paused."
            );
        }

        // =====================================================
        // PAUSE PLAYER
        // =====================================================

        PauseGameplay();

        // =====================================================
        // OPEN UI
        // =====================================================

        sortingUI.SetActive(true);

        Debug.Log(
            $"[GameFlow] Sorting UI Active = " +
            $"{sortingUI.activeSelf}"
        );

        Debug.Log(
            $"[GameFlow] Sorting UI Hierarchy Active = " +
            $"{sortingUI.activeInHierarchy}"
        );

        OnStateChanged?.Invoke(CurrentState);
    }

    // =========================================================
    // EXIT SORTING
    // =========================================================

    public void ExitSortingPhase()
    {
        if (CurrentState != GameState.Sorting)
        {
            Debug.LogWarning(
                "[GameFlow] Tidak sedang berada di Sorting."
            );

            return;
        }

        Debug.Log(
            "[GameFlow] EXIT SORTING PHASE"
        );

        // =====================================================
        // HIDE UI
        // =====================================================

        if (sortingUI != null)
        {
            sortingUI.SetActive(false);
        }

        // =====================================================
        // RESPAWN
        // =====================================================

        if (playerRespawn != null)
        {
            playerRespawn.Respawn();
        }

        // =====================================================
        // NEXT STAGE
        // =====================================================

        if (StageManager.Instance != null)
        {
            StageManager.Instance.AdvanceStage();
        }

        // =====================================================
        // CHANGE STATE
        // =====================================================

        CurrentState = GameState.Driving;

        sortingStarted = false;

        OnStateChanged?.Invoke(CurrentState);

        // =====================================================
        // RESUME
        // =====================================================

        ResumeGameplay();

        if (Timer.Instance != null)
        {
            Timer.Instance.ResetTimer(60f);
        }

        Debug.Log(
            "[GameFlow] Kembali ke Driving."
        );
    }

    // =========================================================
    // PAUSE GAMEPLAY
    // =========================================================

    private void PauseGameplay()
    {
        if (playerCar == null)
        {
            Debug.LogWarning(
                "[GameFlow] playerCar NULL."
            );

            return;
        }

        playerCar.CanControl = false;

        Rigidbody2D rb =
            playerCar.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.bodyType =
                RigidbodyType2D.Kinematic;
        }

        Debug.Log(
            "[GameFlow] Gameplay PAUSED."
        );
    }

    // =========================================================
    // RESUME GAMEPLAY
    // =========================================================

    private void ResumeGameplay()
    {
        if (playerCar == null)
        {
            Debug.LogWarning(
                "[GameFlow] playerCar NULL."
            );

            return;
        }

        Rigidbody2D rb =
            playerCar.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.bodyType =
                RigidbodyType2D.Dynamic;
        }

        playerCar.CanControl = true;

        Debug.Log(
            "[GameFlow] Gameplay RESUMED."
        );
    }

    // =========================================================
    // GAME OVER
    // =========================================================

    private void EndGame()
    {
        if (CurrentState == GameState.Sorting)
            return;

        Debug.Log(
            "[GameFlow] TIME UP."
        );

        if (StageManager.Instance != null)
        {
            StageManager.Instance.SaveProgress();
        }

        if (IrisTransition.Instance != null)
        {
            IrisTransition.Instance.CloseOnly(
                onComplete: () =>
                {
                    Debug.Log(
                        "[GameFlow] GAME OVER."
                    );

                    // TODO:
                    // Game Over UI
                }
            );
        }
    }

    // =========================================================
    // DESTROY
    // =========================================================

    private void OnDestroy()
    {
        if (Timer.Instance != null)
        {
            Timer.Instance.OnTimeUp -= EndGame;
        }

        if (StageManager.Instance != null)
        {
            StageManager.Instance.OnAllTrashCollected -= EnterSortingPhase;
        }

        if (Instance == this)
        {
            Instance = null;
        }
    }
}
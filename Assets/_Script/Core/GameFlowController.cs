using UnityEngine;
using System;

public enum GameState { Driving, Sorting }

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CarController playerCar;
    [SerializeField] private PlayerRespawn playerRespawn;
    [SerializeField] private GameObject sortingUI;

    public GameState CurrentState { get; private set; } = GameState.Driving;
    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        if (Timer.Instance != null)
            Timer.Instance.OnTimeUp += EndGame;
    }

    private void OnDestroy()
    {
        if (Timer.Instance != null)
            Timer.Instance.OnTimeUp -= EndGame;
    }

    public void EnterSortingPhase()
    {
        if (CurrentState == GameState.Sorting) return;
        CurrentState = GameState.Sorting;

        IrisTransition.Instance.PlayTransition(
            onMidpoint: () =>
            {
                // Layar hitam — pause gameplay, tampilkan sorting UI
                PauseGameplay();
                sortingUI.SetActive(true);
                OnStateChanged?.Invoke(CurrentState);
            }
        );
    }

    // Dipanggil dari sorting mini-game saat semua item selesai disortir
    public void ExitSortingPhase()
    {
        if (CurrentState == GameState.Driving) return;

        IrisTransition.Instance.PlayTransition(
            onMidpoint: () =>
            {
                // Layar hitam — sembunyikan sorting UI, respawn, naik stage
                sortingUI.SetActive(false);
                playerRespawn.Respawn();
                StageManager.Instance.AdvanceStage();
                CurrentState = GameState.Driving;
                OnStateChanged?.Invoke(CurrentState);
            },
            onComplete: () =>
            {
                // Iris sudah terbuka — resume gameplay
                ResumeGameplay();
            }
        );
    }

    private void PauseGameplay()
    {
        playerCar.CanControl = false;

        // Stop physics tanpa freeze Time.timeScale
        // (biar Timer tetap jalan selama sorting)
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
        Rigidbody2D rb = playerCar.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Dynamic;

        playerCar.CanControl = true;
    }

    private void EndGame()
    {
        StageManager.Instance.SaveProgress();
        IrisTransition.Instance.CloseOnly(onComplete: () =>
        {
            // TODO: tampilkan Game Over UI
        });
    }
}
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

        SortingResultUI.Instance.BeginSession();
        Timer.Instance?.PauseTimer();

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
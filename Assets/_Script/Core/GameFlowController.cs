using UnityEngine;
using System;

public enum GameState { Driving, Sorting }

public class GameFlowController : MonoBehaviour
{
    public static GameFlowController Instance { get; private set; }

    [SerializeField] private CarController playerCar;

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
        playerCar.CanControl = false;
        OnStateChanged?.Invoke(CurrentState);
    }

    // Panggil dari sorting mini-game pas semua item selesai disortir
    public void ExitSortingPhase()
    {
        if (CurrentState == GameState.Driving) return;
        CurrentState = GameState.Driving;
        playerCar.CanControl = true;
        StageManager.Instance.AdvanceStage();
        OnStateChanged?.Invoke(CurrentState);
    }

    private void EndGame()
    {
        StageManager.Instance.SaveProgress();
        // TODO: tampilkan Game Over UI di sini
    }
}
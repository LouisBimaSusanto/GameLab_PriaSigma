using UnityEngine;
using System;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    // =========================================================
    // STAGE
    // =========================================================

    public int CurrentStage { get; private set; } = 1;

    // =========================================================
    // SCORE
    // =========================================================

    public int Score { get; private set; }

    public int SessionScore { get; private set; }

    public int SessionPerfectScore { get; private set; }

    // =========================================================
    // TRASH
    // =========================================================

    public int TotalTrash { get; private set; }

    public int CollectedTrash { get; private set; }

    public bool AllTrashCollected
    {
        get
        {
            return TotalTrash > 0 &&
                   CollectedTrash >= TotalTrash;
        }
    }

    // =========================================================
    // EVENTS
    // =========================================================

    public event Action<int> OnStageChanged;
    public event Action<int> OnScoreChanged;

    public event Action<int, int> OnTrashCountChanged;

    public event Action OnAllTrashCollected;

    // =========================================================
    // PLAYER PREFS
    // =========================================================

    private const string BestStageKey = "BestStage";
    private const string HighScoreKey = "HighScore";

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

        Debug.Log("[StageManager] Initialized.");
    }

    // =========================================================
    // TRASH
    // =========================================================

    public void RegisterTotalTrash(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning(
                "[StageManager] RegisterTotalTrash menerima 0."
            );

            return;
        }

        TotalTrash += amount;

        Debug.Log(
            $"[StageManager] Register Total Trash: " +
            $"{TotalTrash}"
        );

        OnTrashCountChanged?.Invoke(
            CollectedTrash,
            TotalTrash
        );
    }

    public void RegisterTrashCollected()
    {
        if (CollectedTrash >= TotalTrash)
        {
            Debug.LogWarning(
                "[StageManager] CollectedTrash sudah mencapai TotalTrash."
            );

            return;
        }

        CollectedTrash++;

        Debug.Log(
            $"[StageManager] Trash collected: " +
            $"{CollectedTrash}/{TotalTrash}"
        );

        OnTrashCountChanged?.Invoke(
            CollectedTrash,
            TotalTrash
        );

        if (AllTrashCollected)
        {
            Debug.Log(
                "======================================"
            );

            Debug.Log(
                "[StageManager] ALL TRASH COLLECTED!"
            );

            Debug.Log(
                "======================================"
            );

            OnAllTrashCollected?.Invoke();
        }
    }

    public void ResetTrashCounter()
    {
        TotalTrash = 0;
        CollectedTrash = 0;

        Debug.Log(
            "[StageManager] Trash counter RESET."
        );

        OnTrashCountChanged?.Invoke(
            CollectedTrash,
            TotalTrash
        );
    }

    // =========================================================
    // SCORE
    // =========================================================

    public void AddScore(int amount)
    {
        Score += amount;

        OnScoreChanged?.Invoke(Score);
    }

    public void BeginSortingSession()
    {
        SessionScore = 0;
        SessionPerfectScore = 0;
    }

    public void RegisterItemToSession(int scoreValue)
    {
        SessionPerfectScore += scoreValue;
    }

    public void AddSessionScore(int amount)
    {
        SessionScore += amount;
        AddScore(amount);
    }

    // =========================================================
    // STAGE
    // =========================================================

    public void AdvanceStage()
    {
        CurrentStage++;

        Debug.Log(
            $"[StageManager] Advance to Stage {CurrentStage}"
        );

        OnStageChanged?.Invoke(CurrentStage);
    }

    // =========================================================
    // SAVE
    // =========================================================

    public void SaveProgress()
    {
        if (CurrentStage >
            PlayerPrefs.GetInt(BestStageKey, 0))
        {
            PlayerPrefs.SetInt(
                BestStageKey,
                CurrentStage
            );
        }

        if (Score >
            PlayerPrefs.GetInt(HighScoreKey, 0))
        {
            PlayerPrefs.SetInt(
                HighScoreKey,
                Score
            );
        }

        PlayerPrefs.Save();

        Debug.Log(
            "[StageManager] Progress saved."
        );
    }

    // =========================================================
    // GET SAVE DATA
    // =========================================================

    public int GetBestStage()
    {
        return PlayerPrefs.GetInt(
            BestStageKey,
            0
        );
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(
            HighScoreKey,
            0
        );
    }
}
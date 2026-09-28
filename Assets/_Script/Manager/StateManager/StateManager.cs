using UnityEngine;
using System;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public int CurrentStage { get; private set; } = 1;
    public int Score { get; private set; } = 0;

    public int SessionScore { get; private set; } = 0;
    public int SessionPerfectScore { get; private set; } = 0;

    // Trash counter
    public int TotalTrash { get; private set; } = 0;
    public int CollectedTrash { get; private set; } = 0;

    public event Action<int> OnStageChanged;
    public event Action<int> OnScoreChanged;
    public event Action OnAllTrashCollected;

    private const string BestStageKey = "BestStage";
    private const string HighScoreKey = "HighScore";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // =========================================================
    // TRASH COUNTER
    // =========================================================

    public void ResetTrashCounter()
    {
        TotalTrash = 0;
        CollectedTrash = 0;
        Debug.Log("[StageManager] Trash counter di-reset.");
    }

    public void RegisterTotalTrash(int total)
    {
        TotalTrash = total;
        Debug.Log($"[StageManager] Total trash terdaftar: {TotalTrash}");
    }

    public void RegisterTrashCollected()
    {
        CollectedTrash++;
        Debug.Log($"[StageManager] Trash collected: {CollectedTrash}/{TotalTrash}");

        if (TotalTrash > 0 && CollectedTrash >= TotalTrash)
        {
            Debug.Log("[StageManager] Semua sampah terkumpul!");
            NotifyAllTrashCollected();
        }
    }

    public void NotifyAllTrashCollected()
    {
        OnAllTrashCollected?.Invoke();
    }

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

    public void AdvanceStage()
    {
        CurrentStage++;
        OnStageChanged?.Invoke(CurrentStage);
    }

    public void SaveProgress()
    {
        if (CurrentStage > PlayerPrefs.GetInt(BestStageKey, 0))
            PlayerPrefs.SetInt(BestStageKey, CurrentStage);
        if (Score > PlayerPrefs.GetInt(HighScoreKey, 0))
            PlayerPrefs.SetInt(HighScoreKey, Score);
        PlayerPrefs.Save();
    }

    public int GetBestStage() => PlayerPrefs.GetInt(BestStageKey, 0);
    public int GetHighScore() => PlayerPrefs.GetInt(HighScoreKey, 0);
}
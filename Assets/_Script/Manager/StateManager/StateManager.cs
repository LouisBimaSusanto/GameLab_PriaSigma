using UnityEngine;
using System;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public int CurrentStage { get; private set; } = 1;
    public int Score { get; private set; } = 0;

    public event Action<int> OnStageChanged;
    public event Action<int> OnScoreChanged;

    private const string BestStageKey = "BestStage";
    private const string HighScoreKey = "HighScore";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
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
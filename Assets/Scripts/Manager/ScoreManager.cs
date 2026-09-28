using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score { get; private set; }
    public int bestScore { get; private set; }
    public int moves { get; private set; }
    public Action<int> onMovesChanged;
    public System.Action<int> onScoreChanged;
    public System.Action<int> onScoreSet;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
    private void Start()
    {
        UpdateBestScore();
    }
    public void AddScore(int s)
    {
        score += s * 10;
        onScoreChanged?.Invoke(score);
    }
    public void ReduceMoves()
    {
        if (moves <= 0) return;
        moves--;
        onMovesChanged?.Invoke(moves);
    }
    public void UpdateBestScore()
    {
        bestScore = PlayerPrefs.GetInt("BESTSCORE");
        if (bestScore < score)
        {
            bestScore = score;
            SaveManager.Instance.SaveBestScore(bestScore);
        }
    }
    public void SetScore(int s)
    {
        score = s;
        onScoreSet?.Invoke(score);
    }
    public void SetMoves(int m)
    {
        moves = m;
        onMovesChanged?.Invoke(moves);
    }
    public int GetScore()
    {
        return score;
    }
    public int GetBestScore()
    {
        return bestScore;
    }
    public int GetMoves()
    {
        return moves;
    }
}

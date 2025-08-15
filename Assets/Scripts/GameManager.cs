using UnityEngine;
using System;
using Unity.VisualScripting;
using NUnit.Framework;

public enum GameState
{
    StartingScreen,
    GameStart,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnHighScoreChanged;
    public static event Action OnGameStart;
    public static event Action OnGameOver;
    public static event Action OnScreenStart;

    [Header("Game States")]
    [field: SerializeField] public bool StartingScreen { get; private set; }
    [field: SerializeField] public bool GameStart { get; private set; }
    [field: SerializeField] public bool GameOver { get; private set; }

    [Header("Shared Data")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float stepSize;

    [field: SerializeField] public int Score { get; private set; } = 0;
    [field: SerializeField] public int HighScore { get; private set; } = 0;


    public void IncrementScore()
    {
        Score += 1;
        OnScoreChanged?.Invoke(Score);
    }

    public void ResetScore()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }

    public void StartGame()
    {
        StartingScreen = false;
        GameStart = true;
        GameOver = false;
        OnGameStart?.Invoke();
    }

    public void EndGame()
    {
        StartingScreen = false;
        GameStart = false;
        GameOver = true;
        if (Score > HighScore)
        {
            HighScore = Score;
            OnHighScoreChanged?.Invoke(HighScore);
        }
        ResetScore();
        OnGameOver?.Invoke();
    }

    public void StartScreen()
    {
        StartingScreen = true;
        GameStart = false;
        GameOver = false;
        OnScreenStart?.Invoke();
    }

    public void SetMinMaxXY(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        Bounds bounds = spriteRenderer.bounds;
        minX = bounds.min.x;
        maxX = bounds.max.x;
        minY = bounds.min.y;
        maxY = bounds.max.y;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

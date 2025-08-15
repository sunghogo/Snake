// ScoreUI.cs
using UnityEngine;
using TMPro;

public class ScoreTMP : MonoBehaviour
{
    TMP_Text scoreTMP;

    void Awake()
    {
        scoreTMP = GetComponent<TextMeshProUGUI>();
        if (gameObject.CompareTag("Score"))
        {
            if (GameManager.Instance) scoreTMP.text = GameManager.Instance.Score.ToString();
            GameManager.OnScoreChanged += UpdateScore;
        }
        else if (gameObject.CompareTag("High Score"))
        {
            if (GameManager.Instance) scoreTMP.text = GameManager.Instance.HighScore.ToString();
            GameManager.OnHighScoreChanged += UpdateScore;
        }
        GameManager.OnGameOver += HandleGameOver;
        GameManager.OnGameStart += HandleGameStart;
    }

    void OnDestroy()
    {
        if (gameObject.CompareTag("Score"))  GameManager.OnScoreChanged -= UpdateScore;
        else if (gameObject.CompareTag("High Score")) GameManager.OnHighScoreChanged -= UpdateScore;
        GameManager.OnGameOver -= HandleGameOver;
        GameManager.OnGameStart -= HandleGameStart;
    }

    void HandleGameStart()
    {
        scoreTMP.enabled = true;
    }

    void HandleGameOver()
    {
        scoreTMP.enabled = false;
    }

    void UpdateScore(int newScore)
    {
        scoreTMP.text = newScore.ToString();
    }
}

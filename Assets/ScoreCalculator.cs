using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ScoreCalculator : MonoBehaviour
{

    public int Score { get; private set; } = 0;
    public int HighScore { get; private set; } = 0;

    [SerializeField] private Text currentScoreText;
    [SerializeField] private Text highScoreText;

    void Start()
    {
        Score = 0;
        HighScore = LoadScores();
        currentScoreText.text = Score.ToString();
        highScoreText.text = $"HighScore: {HighScore.ToString()}";
    }



    public void AddScore(int score)
    {
        Score += score;
        currentScoreText.text = Score.ToString();

        if ( Score >= HighScore)
        {
            HighScore = Score;
            highScoreText.text = $"HighScore: {HighScore.ToString()}";

        }
    }
    private int LoadScores()
    {
        int highScore = 0;
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
        return highScore;
    }
    private void SaveScores()
    {
        PlayerPrefs.SetInt("HighScore", HighScore);
    }

    private void OnDestroy()
    {
        SaveScores();
    }
    [MenuItem(("3D Magic Tower/Handy Tools/Reset High Scores"))]
    public static void ResetHighScores()
    {
        PlayerPrefs.SetInt("HighScore", 0);
    }
}

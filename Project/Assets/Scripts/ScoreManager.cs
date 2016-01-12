using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
    private static int Score = 0;
    private static int Treshold = 100;
    private static int HiScore = 0;
    private static Text _scoreText;

    void Start()
    {
        Reset();
        HiScore = PlayerPrefs.GetInt("HighScore");
        _scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        _scoreText.text = string.Format("Score: {0}; Hi-Score: {1}",Score, HiScore);
        
    }
    void Reset()
    {
        Score = 0;
        Treshold = 100;
        HiScore = 0;
    }
    public static void AddScore(int amount)
    {
        Score += amount;
        if (Score >= Treshold && GameManager.Speed > 0.2f)
        {
            GameManager.Speed -= 0.1f;
            Treshold += Treshold;
        }
        if (Score > HiScore)
        {
            PlayerPrefs.SetInt("HighScore", Score);
            HiScore = Score;
        }
        _scoreText.text = string.Format("Score: {0}; Hi-Score: {1}", Score, HiScore);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    Question question;

    [HideInInspector]
    public static int score;
    int point = 10;

    void Start()
    {
        question = FindObjectOfType<Question>();
        score = 0;
        scoreText.text = "Score: " + score.ToString();
    }

    public void CalculateScore()
    {
        if (question.isAnswerTrue)
        {
            score += point;
            scoreText.text = "Score: " + score.ToString();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private int getScore = 0;
    private int questionsSeen = 0;

    public int GetCorrectAnswerScore()
    {
        return getScore;
    }

    public void IncrementCorrectAnswers()
    {
        getScore++;
    }

    public int GetQuestionsSeen()
    {
        return GetQuestionsSeen();
    }

    public void IncrementQuestionsSeen()
    {
        questionsSeen++;
    }

    public int CalculateScore()
    {
        return Mathf.RoundToInt(getScore / (float) questionsSeen * 100);
    }
}

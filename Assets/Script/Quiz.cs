using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] private List<QuestionSO> questions;
    QuestionSO currentQuestion;
    
    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;
    
    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")] 
    [SerializeField] private TextMeshProUGUI scoreText;
    private ScoreKeeper scoreKeeper;

    [Header("ProgressBar")] [SerializeField]
    private Slider progressBar;

    public bool isComplete;
    
    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    private void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        
        // If timer loop says we can load next question (timer ran out and we're not answering a question)
        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }
            
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        // If timer runs out and player has not answered, make the choice for player.
        else if (!hasAnsweredEarly && !timer.getIsAnsweringQuestion())
        {
            DisplayAnswer(-1);
            SetButtonsState(false);
        }
    }

    private void DisplayQuestion()
    {
        questionText.text = currentQuestion.getQuestion();
        
        for (var i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.getAnswer(i);
        }

        
    }

    private void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonsState(true);
            ResetButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    private void GetRandomQuestion()
    {
        var index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        
        if (questions.Contains(currentQuestion)) questions.Remove(currentQuestion);
    }

    private void SetButtonsState(bool state)
    {
        foreach (var answerButton in answerButtons)
        {
            answerButton.GetComponent<Button>().interactable = state;
        }
    }

    private void ResetButtonSprites()
    {
        foreach (var answerButton in answerButtons)
        {
            answerButton.GetComponent<Image>().sprite = defaultAnswerSprite;
            answerButton.GetComponent<Image>().color = new Color(255, 255, 255);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonsState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
        
    }

    private void DisplayAnswer(int index)
    {
        if (index == currentQuestion.getCorrectAnswerIndex())
        {
            questionText.text = "Correct answer!";
            scoreKeeper.IncrementCorrectAnswers();
            ShowCorrectAnswer(index);
        }
        else
        {
            questionText.text = "Sorry - The correct answer was:\n"
                                + currentQuestion.getAnswer(currentQuestion.getCorrectAnswerIndex());
            HighlightWrongAnswer(index);
            ShowCorrectAnswer(currentQuestion.getCorrectAnswerIndex());
        }

    }

    private void ShowCorrectAnswer(int index)
    {
        Image buttonImage = answerButtons[index].GetComponent<Image>();
        buttonImage.sprite = correctAnswerSprite;
    }
    
    private void HighlightWrongAnswer(int index)
    {
        Image buttonImage = answerButtons[index].GetComponent<Image>();
        buttonImage.sprite = correctAnswerSprite;
        buttonImage.color = new Color(255, 0, 0);
    }
    
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz Question", order = 0)]
public class QuestionSO : ScriptableObject 
{
    [TextArea(2,6)]
    [SerializeField] string question = "Enter new question";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex;

    public QuestionSO() {
        if (correctAnswerIndex > 3) {
            throw new UnityException("Answer index out of range");
        }
    }

    public string getQuestion() {
        return question;
    }

    public int getCorrectAnswerIndex() {
        return correctAnswerIndex;
    }

    public string getAnswer(int index) {
        return answers[index];
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizResult : ScriptableObject
{
    public string question;
    public string category;
    public string[] answers;
    public int correctAnswerIndex;
    public bool isCorrect;
}

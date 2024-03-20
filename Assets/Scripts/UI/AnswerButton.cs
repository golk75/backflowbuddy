using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class AnswerButton : MonoBehaviour
{
    private bool isCorrect;
    private Button answerButton;


    public void SetAnswer(string answerText)
    {
        answerButton.text = answerText;
    }

    public void SetIsCorrect(bool newBool)
    {
        isCorrect = newBool;
    }
    // Start is called before the first frame update
    void Start()
    {
        answerButton.clicked += CheckAnswer;
    }

    private void CheckAnswer()
    {
        if (isCorrect)
        {
            Debug.Log($"Correct answer selected");
        }
        else
        {
            Debug.Log($"Incorrect answer selected");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "QuestionObject/Question")]
public class QuestionData : ScriptableObject
{
    public string question;
    public string category;
    public string[] answers;

}

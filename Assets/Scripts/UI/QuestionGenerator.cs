
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionGenerator : MonoBehaviour
{
    //visual element strings
    const string QuestionContainerString = "QuestionContiner";

    [SerializeField]
    private List<QuestionData> questions;
    private QuestionData currentQuestion;
    private Label questionLabel;
    public UIDocument root;
    [SerializeField]
    private List<VisualElement> Buttons;
    private List<VisualElement> answerButtons;
    [SerializeField]
    private VisualElement correctAnswerButton;
    [SerializeField]
    private string correctAnswerText;
    // [SerializeField]
    // private List<AnswerButton> answerButtons;
    private int correctAnswer;


    void Awake()
    {
        GetQuestionData();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetVisualElements();


        SelectNewQuestion();
        SetQuestionLabel();
        SetAnswerLabels();
        RegisterCallBacks();
    }

    private void RegisterCallBacks()
    {
        foreach (var ele in answerButtons)
        {
            ele.RegisterCallback<PointerDownEvent>(
                evt =>
                {
                    //Correct answer selection
                    if (evt.target == correctAnswerButton)
                    {
                        Debug.Log($"CORRECT ANSWER!");
                    }
                    //Incorrect answer selection
                    else
                    {
                        Debug.Log($"WRONG ANSWER!");
                    }

                }, TrickleDown.TrickleDown);
        }
    }
    private void SelectNewQuestion()
    {
        //Get random question
        int randomIndex = Random.Range(0, questions.Count);

        //set currentquestion to randIndex
        currentQuestion = questions[randomIndex];

        //remove this question to avoid repeating
        questions.RemoveAt(randomIndex);
    }
    private void SetQuestionLabel()
    {
        questionLabel.text = currentQuestion.question;
    }

    private void SetAnswerLabels()
    {
        List<string> answers = RandomizeAnswers(new List<string>(currentQuestion.answers));
        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].Q<Button>(className: "unity-button").text = answers[i];
        }
    }
    private List<string> RandomizeAnswers(List<string> originalList)
    {
        bool correctAnswerSelected = false;
        List<string> newList = new List<string>();
        for (int i = 0; i < answerButtons.Count; i++)
        {
            int random = Random.Range(0, originalList.Count);
            if (random == 0 && !correctAnswerSelected)
            {
                correctAnswer = i;
                // Debug.Log($"answerButtons[i]: {answerButtons[correctAnswer].name}");
                correctAnswerButton = answerButtons[correctAnswer];
                correctAnswerSelected = true;
            }

            newList.Add(originalList[random]);
            originalList.RemoveAt(random);
        }
        return newList;
    }
    private void GetQuestionData()
    {
        questions = new List<QuestionData>(Resources.LoadAll<QuestionData>("ScriptableObjects/Quiz"));
    }
    private void SetVisualElements()
    {


        questionLabel = root.rootVisualElement.Q<VisualElement>(QuestionContainerString).Q<Label>(className: "unity-label");

        answerButtons = root.rootVisualElement.Query(className: "answer-button").ToList();

        foreach (var button in answerButtons)
        {
            Debug.Log($"{button.name}");
            // answerButtons.Add((Button)button);


        }




    }

    // private void AnswerButtonClicked(PointerDownEvent evt)
    // {
    //     if (evt.target == correctAnswerButton)
    //     {

    //     }

    // }

    // private void SelectNewQuestion()
    // {
    //     //Get random question
    //     int randomIndex = Random.Range(0, questions.Count);

    //     //set currentquestion to randIndex
    //     currentQuestion = questions[randomIndex];

    //     //remove this question to avoid repeating
    //     questions.RemoveAt(randomIndex);
    // }

    // private void SetQuestionVisualElement()
    // {
    //     questionLabel.text = currentQuestion.question;
    // }

    // private void SetAnswerData()
    // {
    //     //randomize answer button order
    //     List<string> answers = RandomizeAnswers(new List<string>(currentQuestion.answers));

    //     //set answer buttons
    //     for (int i = 0; i < answerButtons.Count; i++)
    //     {
    //         bool isCorrect = false;

    //         if (i == correctAnswer)
    //         {
    //             isCorrect = true;
    //             correctAnswerButton = answerButtons[i];
    //             correctAnswerButton.text = correctAnswerText;
    //         }
    //         else
    //         {
    //             answerButtons[i].text = answers[i];
    //         }
    //         // answerButtons[i].SetIsCorrect(isCorrect);
    //         // answerButtons[i].SetAnswer(answers[i]);

    //     }


    // }

    // private List<string> RandomizeAnswers(List<string> originalList)
    // {
    //     bool correctAnswerSelected = false;
    //     List<string> newList = new List<string>();
    //     for (int i = 0; i < answerButtons.Count; i++)
    //     {
    //         int random = Random.Range(0, originalList.Count);
    //         if (random == 0 && !correctAnswerSelected)
    //         {
    //             correctAnswer = i;
    //             correctAnswerText = originalList[random];
    //             correctAnswerSelected = true;
    //         }
    //         newList.Add(originalList[random]);
    //         originalList.RemoveAt(random);
    //     }
    //     return newList;
    // }



    // Update is called once per frame
    void Update()
    {

    }
}

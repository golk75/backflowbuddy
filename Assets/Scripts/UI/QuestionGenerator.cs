
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class QuestionGenerator : MonoBehaviour
{
    //visual element strings
    const string QuestionContainerString = "QuestionContiner";
    const string EndOfQuizPanelString = "EndOfQuizPanelScreen";
    const string ReviewResultsString = "ReviewResults";
    const string QuestionAndAnswerString = "QuestionAndAnswer";
    public VisualTreeAsset m_ResultsEntryListTemplate;



    [SerializeField]
    private List<QuestionData> questions;
    private QuestionData currentQuestion;
    private Label questionLabel;
    public UIDocument root;
    [SerializeField]
    private List<VisualElement> Buttons;
    private List<VisualElement> answerButtons;
    private VisualElement endOfQuizPanel;
    private VisualElement ReviewResultsScreen;
    private VisualElement QuestionAndAnswerPanel;
    [SerializeField]
    private VisualElement correctAnswerButton;
    [SerializeField]
    private string correctAnswerText;
    private float totalQuestionCount;
    private float totalCorrect;
    private float totalIncorrect;

    // [SerializeField]
    // private List<AnswerButton> answerButtons;

    private int correctAnswer;
    private List<QuestionData> AnsweredQuestions;
    private List<QuestionData> FalggedQuestions;
    //[SerializeField]
    // private List<QuestionData> IncorrectlyAnsweredQuestions;

    public List<QuizResult> QuizResults;
    private static string resultsPath = "Assets/Resources/QuizResults";


    void Awake()
    {
        GetQuestionData();
        totalQuestionCount = questions.Count;
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
        QuizResult resultsData;

        foreach (var ele in answerButtons)
        {
            ele.RegisterCallback<PointerUpEvent>(
               evt =>
               {



                   //Correct answer selection
                   if (evt.target == correctAnswerButton)
                   {
                       totalCorrect += 1;

                       Debug.Log($"CORRECT ANSWER!");
                       resultsData = ScriptableObject.CreateInstance<QuizResult>();
                       resultsData.category = currentQuestion.category;
                       resultsData.question = currentQuestion.question;
                       resultsData.correctAnswerIndex = correctAnswer;
                       resultsData.isCorrect = true;
                       QuizResults.Add(resultsData);
                       SelectNewQuestion();
                       SetQuestionLabel();
                       SetAnswerLabels();

                   }
                   //Incorrect answer selection
                   else
                   {
                       Debug.Log($"WRONG ANSWER!");
                       resultsData = ScriptableObject.CreateInstance<QuizResult>();
                       resultsData.category = currentQuestion.category;
                       resultsData.question = currentQuestion.question;
                       resultsData.correctAnswerIndex = correctAnswer;
                       resultsData.isCorrect = false;
                       QuizResults.Add(resultsData);
                       SelectNewQuestion();
                       SetQuestionLabel();
                       SetAnswerLabels();
                   }
               }, TrickleDown.TrickleDown);

        }

    }

    private void SelectNewQuestion()
    {
        if (questions.Count == 0)
        {
            //custom controller QuizManager.cs listening

            Actions.EndOfQuizQuestions?.Invoke(QuizResults, m_ResultsEntryListTemplate, totalCorrect / totalQuestionCount * 100);

            return;
        }

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
                //set correct answer to a button
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
        questions = new List<QuestionData>(Resources.LoadAll<QuestionData>("ScriptableObjects/QuizQuestions"));
        Debug.Log($"questions.Count = {questions.Count}");
    }
    private void SetVisualElements()
    {


        questionLabel = root.rootVisualElement.Q<VisualElement>(QuestionContainerString).Q<Label>(className: "unity-label");

        answerButtons = root.rootVisualElement.Query(className: "answer-button").ToList();

        endOfQuizPanel = root.rootVisualElement.Q(EndOfQuizPanelString);

        ReviewResultsScreen = root.rootVisualElement.Q(ReviewResultsString);

        QuestionAndAnswerPanel = root.rootVisualElement.Q(QuestionAndAnswerString);

    }

    void Update()
    {
        // Debug.Log($"GetScore()= {GetScore()}");
    }
}

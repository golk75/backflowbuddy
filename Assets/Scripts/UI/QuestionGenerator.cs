using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionGenerator : MonoBehaviour
{
    public UIDocument root;

    //visual element strings
    const string QuestionContainerString = "QuestionContiner";
    const string ReviewResultsString = "ReviewResults";


    public VisualTreeAsset m_ResultsEntryListTemplate;
    [SerializeField]
    private List<QuestionData> questions;
    private List<VisualElement> answerButtons;
    private List<Button> answerButtons11;
    public List<QuizResult> QuizResults;
    private string[] currentAnswers;

    private VisualElement ReviewResultsScreen;
    private Label questionLabel;
    private VisualElement correctAnswerButton;
    private ListView listView;
    private VisualElement QandAScreen;
    private Label m_quesitonTracker;
    private Button m_BackToResultsButton;
    private float totalQuestionCount;
    private float totalCorrect;
    private int correctAnswer;
    private int questionNumber;
    private int chosenAnswer;
    // private List<QuestionData> FalggedQuestions;    
    private QuestionData currentQuestion;
    private StyleTranslate scrollBarPreviousPos;
    private Length draggerYPos;
    private QuizResult currentQuestionResult;
    int resultsListSelectedIndex;


    private bool isQuizComplete;



    void OnEnable()
    {
        Actions.GenerateResultsQuestionReview += ReviewQuestion;
    }
    void OnDisable()
    {
        Actions.GenerateResultsQuestionReview -= ReviewQuestion;
    }

    void Awake()
    {
        GetQuestionData();
        totalQuestionCount = questions.Count;


    }

    // Start is called before the first frame update
    void Start()
    {

        SetVisualElements();
        m_BackToResultsButton.style.display = DisplayStyle.None;

        SelectNewQuestion();
        SetQuestionLabel();
        SetAnswerLabels();
        RegisterCallBacks();


    }

    private void RegisterCallBacks()
    {
        m_BackToResultsButton.clicked += ReturnToResults;


        foreach (var ele in answerButtons)
        {

            ele.RegisterCallback<PointerUpEvent>(
               evt =>
               {

                   //Correct answer selection
                   if (evt.target == correctAnswerButton)
                   {
                       totalCorrect += 1;

                       //    Debug.Log($"CORRECT ANSWER!");

                       SetResultData(true, chosenAnswer);

                   }
                   //Incorrect answer selection
                   else
                   {
                       //    Debug.Log($"WRONG ANSWER!");

                       SetResultData(false, chosenAnswer);

                   }
               }, TrickleDown.TrickleDown);

        }

    }

    private void DisableAnswerButtons()
    {
        for (int i = 0; i < answerButtons.Count; i++)
        {
            Invoke(nameof(DelayAnswerButtonDisable), 0.01f);
            answerButtons[i].pickingMode = PickingMode.Ignore;
        }
    }
    void DelayAnswerButtonDisable()
    {
        // Debug.Log($"delaying answer button disable...");
    }
    private void EnableAnswerButtons()
    {
        for (int i = 0; i < answerButtons.Count; i++)
        {
            Invoke(nameof(DelayAnswerButtonEnable), 0.01f);
            answerButtons[i].pickingMode = PickingMode.Ignore;
        }
    }
    void DelayAnswerButtonEnable()
    {
        // Debug.Log($"delaying answer button enable...");
    }
    private void ReturnToResults()
    {
        ReviewResultsScreen.style.display = DisplayStyle.Flex;
        for (int i = 0; i < answerButtons.Count; i++)
        {
            // answerButtons[i].Q<Button>(className: "unity-button").text = currentQuestionResult.answers[i];
            if (i == currentQuestionResult.correctAnswerIndex)
            {

                answerButtons[i].AddToClassList(className: "answer-button");
                answerButtons[i].RemoveFromClassList(className: "incorrect-answer");
                answerButtons[i].RemoveFromClassList(className: "correct-answer");


            }
            if (i == currentQuestionResult.chosenAnswer)
            {

                answerButtons[i].AddToClassList(className: "answer-button");
                answerButtons[i].RemoveFromClassList(className: "correct-answer");
                answerButtons[i].RemoveFromClassList(className: "incorrect-answer");

            }


        }
        EnableAnswerButtons();
        Actions.returnScrollHandle?.Invoke(resultsListSelectedIndex);
    }

    private void SetResultData(bool correctness, int selectedAnswer)
    {

        if (!isQuizComplete)
        {
            if (questionNumber < totalQuestionCount)
                questionNumber += 1;
            m_quesitonTracker.text = questionNumber.ToString() + "/" + totalQuestionCount.ToString();

            QuizResult resultsData;
            resultsData = ScriptableObject.CreateInstance<QuizResult>();
            resultsData.category = currentQuestion.category;
            resultsData.questionNumber = questionNumber - 1;
            resultsData.question = currentQuestion.question;
            resultsData.answers = currentAnswers;
            resultsData.correctAnswerIndex = correctAnswer;
            resultsData.isCorrect = correctness;
            resultsData.chosenAnswer = selectedAnswer;
            QuizResults.Add(resultsData);
            SelectNewQuestion();
            SetQuestionLabel();
            SetAnswerLabels();
        }
        else
        {


        }



    }
    //this will display after a question is selected in the quiz results screen
    private void ReviewQuestion(QuizResult quizResult, int selectedIndex)
    {
        resultsListSelectedIndex = selectedIndex;
        currentQuestionResult = quizResult;
        DisableAnswerButtons();
        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].Q<Button>(className: "unity-button").text = quizResult.answers[i];
            if (i == quizResult.correctAnswerIndex)
            {

                answerButtons[i].RemoveFromClassList(className: "answer-button");
                answerButtons[i].RemoveFromClassList(className: "incorrect-answer");
                answerButtons[i].AddToClassList(className: "correct-answer");


            }
            if (i == quizResult.chosenAnswer && !quizResult.isCorrect)
            {

                answerButtons[i].RemoveFromClassList(className: "answer-button");
                answerButtons[i].RemoveFromClassList(className: "correct-answer");
                answerButtons[i].AddToClassList(className: "incorrect-answer");

            }


        }

        questionLabel.text = quizResult.question;
        m_quesitonTracker.text = quizResult.questionNumber.ToString() + "/" + totalQuestionCount.ToString();

        // answerButtons[quizResult.correctAnswerIndex].style.backgroundColor = Color.green;
        // answerButtons[quizResult.chosenAnswer].style.backgroundColor = Color.red;
        // Debug.Log(quizResult.answers);



    }
    private void SelectNewQuestion()
    {
        if (questions.Count == 0)
        {
            //custom controller QuizManager.cs listening
            isQuizComplete = true;
            m_BackToResultsButton.style.display = DisplayStyle.Flex;
            Actions.EndOfQuizQuestions?.Invoke(QuizResults, m_ResultsEntryListTemplate, totalCorrect / totalQuestionCount * 100);
            //stop removing questions
            return;
        }
        //animate buttons reveal
        // for (int i = 0; i < answerButtons.Count; i++)
        // {
        //     answerButtons[i].AddToClassList("answer-button-in");
        //     answerButtons[i].RemoveFromClassList("answer-button-out");
        // }
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
        currentAnswers = new string[4];
        List<string> answers = RandomizeAnswers(new List<string>(currentQuestion.answers));
        for (int i = 0; i < answerButtons.Count; i++)
        // for (int i = 0; i < answerButtons11.Count; i++)
        {
            answerButtons[i].Q<Button>(className: "unity-button").text = answers[i];
            // answerButtons11[i].Q<Button>(className: "unity-button").text = answers[i];
            currentAnswers[i] = answers[i];
        }
    }
    private List<string> RandomizeAnswers(List<string> originalList)
    {
        bool correctAnswerSelected = false;
        List<string> newList = new List<string>();
        for (int i = 0; i < answerButtons.Count; i++)
        // for (int i = 0; i < answerButtons11.Count; i++)
        {
            int random = Random.Range(0, originalList.Count);
            if (random == 0 && !correctAnswerSelected)
            {
                correctAnswer = i;
                //set correct answer to a button
                correctAnswerButton = answerButtons[correctAnswer];
                // correctAnswerButton = answerButtons11[correctAnswer];
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

    }
    private void SetVisualElements()
    {
        m_BackToResultsButton = root.rootVisualElement.Q<Button>("back-to-results");

        questionLabel = root.rootVisualElement.Q<VisualElement>(QuestionContainerString).Q<Label>(className: "unity-label");

        answerButtons = root.rootVisualElement.Query(className: "answer-button").ToList();
        answerButtons11 = root.rootVisualElement.Query<Button>(className: "answer-button").ToList();
        m_quesitonTracker = root.rootVisualElement.Q<VisualElement>("question-tracking").Q<Label>(className: "unity-label");
        questionNumber = 1;
        m_quesitonTracker.text = questionNumber.ToString() + "/" + totalQuestionCount.ToString();

        ReviewResultsScreen = root.rootVisualElement.Q(ReviewResultsString);
        QandAScreen = root.rootVisualElement.Q("QuestionAndAnswer");
        listView = ReviewResultsScreen.Q<ListView>("question-list");

    }

    void Update()
    {

        // Debug.Log($"GetScore()= {GetScore()}");
    }
}



using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuizManager : VisualElement
{
    private const string ResumeQuizButtonString = "resume-quiz";
    private const string QuitToMenuButtonString = "quit-quiz";
    private const string QuizMenuString = "QuizMenuScreen";
    VisualElement m_ReviewResults;
    VisualTreeAsset m_QuestionListEntryTemplate;
    ListView m_QuestionList;
    VisualElement listEntry;
    Label resultPercent;
    ListView m_ResultsListView;
    VisualElement m_QuestionAndAnswerScreen;
    VisualElement m_QuizSelectionScreen;
    VisualElement m_AnswerContainer;
    StyleTranslate scrollBarPos;


    private Button m_QuitToMenuButton;
    private Button m_ResumeQuizButton;
    private Button m_QuizMenuCloseButton;
    private Button m_QuizSelectionButton1;
    private Button m_QuizSelectionButton2;
    private Button m_QuizSelectionButton3;
    private Button m_BackToResultsButton;
    private Button m_BackToMainMenu;
    private VisualElement m_QuizMenuModal;

    public new class UxmlFactory : UxmlFactory<QuizManager, UxmlTraits> { }

    public QuizManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);

    }


    void OnGeometryChange(GeometryChangedEvent evt)
    {

        SetVisualElements();
        RegisterCallBacks();




        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void SetVisualElements()
    {
        m_ReviewResults = this.Q("ReviewResults");
        m_ResultsListView = m_ReviewResults.Q<ListView>("question-list");
        m_QuestionAndAnswerScreen = this.Q("QuestionAndAnswer");
        m_QuizSelectionScreen = this.Q("QuizSelection");
        m_AnswerContainer = this.Q("AnswersContainer");
        m_QuizMenuModal = this.Q(QuizMenuString);
        m_QuitToMenuButton = this.Q<Button>(QuitToMenuButtonString);
        m_ResumeQuizButton = this.Q<Button>(ResumeQuizButtonString);
        m_QuizMenuCloseButton = this.Q("QuizMenuScreen").Q<Button>("close-button");
        m_QuizSelectionButton1 = this.Q<Button>("quiz-1");
        m_QuizSelectionButton2 = this.Q<Button>("quiz-2");
        m_QuizSelectionButton3 = this.Q<Button>("quiz-3");
        m_BackToResultsButton = m_QuestionAndAnswerScreen.Q<Button>("back-to-results");
        m_BackToMainMenu = m_QuizSelectionScreen.Q<Button>("back-button");


    }
    private void EntrySelectionChanged(IEnumerable<object> enumerable)
    {
        var selectedEntry = m_ResultsListView.selectedItem as QuizResult;


        m_ReviewResults.style.display = DisplayStyle.None;
        m_QuestionAndAnswerScreen.style.display = DisplayStyle.Flex;
        Actions.GenerateResultsQuestionReview?.Invoke(selectedEntry, m_ResultsListView.selectedIndex);

    }
    private void RegisterCallBacks()
    {
        m_ResultsListView.selectionChanged += EntrySelectionChanged;
        m_QuizSelectionButton1.RegisterCallback<ClickEvent>(evt => Quiz1Selected());
        m_QuizSelectionButton2.RegisterCallback<ClickEvent>(evt => Quiz2Selected());
        m_QuizSelectionButton3.RegisterCallback<ClickEvent>(evt => Quiz3Selected());
        m_ResumeQuizButton.RegisterCallback<ClickEvent>(evt => ResumeQuiz());
        m_QuitToMenuButton.RegisterCallback<ClickEvent>(evt => QuitToMainMenu());
        m_QuizMenuCloseButton.RegisterCallback<ClickEvent>(evt => ResumeQuiz());
        m_BackToResultsButton.RegisterCallback<ClickEvent>(evt => ReturnToResults());
        m_BackToMainMenu.RegisterCallback<ClickEvent>(evt => QuitToMainMenu());

    }

    private void ReturnToResults()
    {
        //only dealing with the q&a screen => Results screen is being dealth with in QuestionGenerator class


    }

    private void Quiz3Selected()
    {
        Actions.onQuizSelection?.Invoke(100);
        m_QuestionAndAnswerScreen.style.display = DisplayStyle.Flex;
        m_QuizSelectionScreen.style.display = DisplayStyle.None;
    }

    private void Quiz2Selected()
    {
        Actions.onQuizSelection?.Invoke(50);
        m_QuestionAndAnswerScreen.style.display = DisplayStyle.Flex;
        m_QuizSelectionScreen.style.display = DisplayStyle.None;
    }

    private void Quiz1Selected()
    {
        // Actions.onQuizSelection?.Invoke(25);
        Actions.onQuizSelection?.Invoke(25);
        m_QuestionAndAnswerScreen.style.display = DisplayStyle.Flex;
        m_QuizSelectionScreen.style.display = DisplayStyle.None;


    }
    private void QuitToMainMenu()
    {

        SceneManager.LoadScene("MainMenu");
    }
    private void ResumeQuiz()
    {
        m_QuizMenuModal.style.display = DisplayStyle.None;
    }

}
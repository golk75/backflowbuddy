using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuizManager : VisualElement
{
    private const string ResumeQuizButtonString = "resume-quiz";
    private const string QuitToMenuButtonString = "quit-quiz";
    private const string QuizMenuString = "MenuModal";
    VisualElement m_ReviewResults;
    VisualTreeAsset m_QuestionListEntryTemplate;
    ListView m_QuestionList;
    VisualElement listEntry;
    Label resultPercent;
    ListView m_ResultsListView;
    VisualElement m_QuestionAndAnswerScreen;
    VisualElement m_AnswerContainer;
    StyleTranslate scrollBarPos;


    private Button m_QuitToMenuButton;
    private Button m_ResumeQuizButton;
    private Button m_QuizMenuCloseButton;
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
        m_AnswerContainer = this.Q("AnswersContainer");
        m_QuizMenuModal = this.Q(QuizMenuString);
        m_QuitToMenuButton = this.Q<Button>(QuitToMenuButtonString);
        m_ResumeQuizButton = this.Q<Button>(ResumeQuizButtonString);
        m_QuizMenuCloseButton = this.Q("GameMenuScreen").Q<Button>("close-button");
    }
    private void EntrySelectionChanged(IEnumerable<object> enumerable)
    {
        var selectedEntry = m_ResultsListView.selectedItem as QuizResult;


        m_ReviewResults.style.display = DisplayStyle.None;

        Actions.GenerateResultsQuestionReview?.Invoke(selectedEntry, m_ResultsListView.selectedIndex);

    }
    private void RegisterCallBacks()
    {
        m_ResultsListView.selectionChanged += EntrySelectionChanged;
        m_ResumeQuizButton.RegisterCallback<ClickEvent>(evt => ResumeQuiz());
        m_QuitToMenuButton.RegisterCallback<ClickEvent>(evt => QuitToMainMenu());
        m_QuizMenuCloseButton.RegisterCallback<ClickEvent>(evt => ResumeQuiz());
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
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuestionAndAnswer : VisualElement
{


    private const string MenuModalString = "QuizMenuScreen";
    private const string MenuButtonString = "quiz-menu-button";

    Button m_QuizMenuButton;
    VisualElement m_MenuModal;
    Button LastButtonToAnimate;
    public List<VisualElement> AnswerButtons { get; private set; }
    Label QuestionLabel;
    public bool animateComplete;
    public bool isQuizFinished;

    public new class UxmlFactory : UxmlFactory<QuestionAndAnswer, UxmlTraits> { }



    public QuestionAndAnswer()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);


    }



    // private void ResultsListEntrySelect(IEnumerable<object> enumerable)
    // {
    //     throw new NotImplementedException();
    // }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        // m_ReviewScreen = this.parent.Q("ReviewResults");
        // m_ResultsList = m_ReviewScreen.Q<ListView>("question-list");
        // m_ResultsList.selectionChanged += ResultsListEntrySelect;
        m_QuizMenuButton = this.Q<Button>(MenuButtonString);
        m_MenuModal = this.Q<VisualElement>(MenuModalString);
        Actions.onResultsReveal += QuizCompletionStatus;
        m_QuizMenuButton.RegisterCallback<ClickEvent>(evt => EnableMenuModal());
        AnswerButtons = this.Query(className: "answer-button-out").ToList();
        LastButtonToAnimate = this.Q<Button>("Answer-D");
        LastButtonToAnimate.RegisterCallback<TransitionEndEvent>(evt => TransitionLeftAnimationEnd());
        QuestionLabel = this.Q<Label>(className: "question-label");
        foreach (var button in AnswerButtons)
        {
            button.RemoveFromClassList("answer-button-out");
            button.AddToClassList("answer-button");
            button.RegisterCallback<ClickEvent>(evt => AnimateQuestionChange());
        }
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void QuizCompletionStatus()
    {


        isQuizFinished = true;
        Actions.onResultsReveal -= QuizCompletionStatus;

    }

    private void TransitionLeftAnimationEnd()
    {


        if (isQuizFinished)
            return;
        QuestionLabel.RemoveFromClassList("question-label-out");
        foreach (var button in AnswerButtons)
        {

            button.RemoveFromClassList("answer-button-out");
            button.AddToClassList("answer-button");

            // this event is firing on the results review portion when checking list view questions/answers
        }

        Debug.Log($"quizCompletionStatus: {isQuizFinished}");
        // Actions.onAnswerAnimateComplete?.Invoke(true);
    }

    private void AnimateQuestionChange()
    {
        QuestionLabel.AddToClassList("question-label-out");

        foreach (var button in AnswerButtons)
        {
            button.RemoveFromClassList("answer-button");
            button.AddToClassList("answer-button-out");
        }

        // Actions.onAnswerAnimateComplete?.Invoke(false);
    }

    private void EnableMenuModal()
    {
        m_MenuModal.style.display = DisplayStyle.Flex;

    }




}
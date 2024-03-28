using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
public class QuizManager : VisualElement
{
    bool isShowingResults;
    VisualElement m_ReviewResults;
    VisualTreeAsset m_QuestionListEntryTemplate;

    ListView m_QuestionList;
    VisualElement listEntry;
    Label resultPercent;
    ListView m_ResultsListView;
    VisualElement m_QuestionAndAnswerScreen;
    VisualElement m_AnswerContainer;
    StyleTranslate scrollBarPos;


    public new class UxmlFactory : UxmlFactory<QuizManager, UxmlTraits> { }

    public QuizManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);

    }


    void OnGeometryChange(GeometryChangedEvent evt)
    {

        SetVisualElements();

        m_ResultsListView.selectionChanged += EntrySelectionChanged;



        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void SetVisualElements()
    {
        m_ReviewResults = this.Q("ReviewResults");
        m_ResultsListView = m_ReviewResults.Q<ListView>("question-list");
        m_QuestionAndAnswerScreen = this.Q("QuestionAndAnswer");
        m_AnswerContainer = this.Q("AnswersContainer");
    }
    private void EntrySelectionChanged(IEnumerable<object> enumerable)
    {
        var selectedEntry = m_ResultsListView.selectedItem as QuizResult;


        m_ReviewResults.style.display = DisplayStyle.None;

        Actions.GenerateResultsQuestionReview?.Invoke(selectedEntry, m_ResultsListView.selectedIndex);

    }


}
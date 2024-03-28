using System;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UIElements;

public class ReviewResults : VisualElement
{

    ListView m_ResultsListView;
    VisualElement m_QuestionAndAnswerScreen;
    int scrollReturnIndex;

    public new class UxmlFactory : UxmlFactory<ReviewResults, UxmlTraits> { }



    public ReviewResults()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }



    void OnGeometryChange(GeometryChangedEvent evt)
    {
        // m_QuestionAndAnswerScreen = parent.parent.Q("QuestionAndAnswer");
        // m_ResultsListView = this.Q<ListView>("question-list");


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);

    }



}
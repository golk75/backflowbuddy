using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ReviewResults : VisualElement
{


    public new class UxmlFactory : UxmlFactory<ReviewResults, UxmlTraits> { }
    ListView m_ResultsListView;


    public ReviewResults()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }



    void OnGeometryChange(GeometryChangedEvent evt)
    {
        m_ResultsListView = this.Q<ListView>("question-list");
        m_ResultsListView.selectionChanged += EntrySelectionChanged;
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void EntrySelectionChanged(IEnumerable<object> enumerable)
    {
        var selectedCharacter = m_ResultsListView.selectedItem as QuizResult;
        Debug.Log($"Selected Result Entry selected = {selectedCharacter.question}");

    }
}
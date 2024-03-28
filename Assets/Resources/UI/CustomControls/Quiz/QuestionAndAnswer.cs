using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuestionAndAnswer : VisualElement
{

    private VisualElement m_ReviewScreen;
    private ListView m_ResultsList;




    public new class UxmlFactory : UxmlFactory<QuestionAndAnswer, UxmlTraits> { }



    public QuestionAndAnswer()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        // m_ReviewScreen = this.parent.Q("ReviewResults");
        // m_ResultsList = m_ReviewScreen.Q<ListView>("question-list");
        // m_ResultsList.selectionChanged += ResultsListEntrySelect;


    }

    // private void ResultsListEntrySelect(IEnumerable<object> enumerable)
    // {
    //     throw new NotImplementedException();
    // }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }






}
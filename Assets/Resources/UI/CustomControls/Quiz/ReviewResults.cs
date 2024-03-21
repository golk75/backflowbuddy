using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class ReviewResults : VisualElement
{

    private const string EndOfQuizPanelString = "EndOfQuizPanel";


    VisualElement endOfQuizPanel;
    VisualElement QuestionAndAnswer;
    // List<Button> Buttons;
    public new class UxmlFactory : UxmlFactory<ReviewResults, UxmlTraits> { }



    public ReviewResults()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);


    }


    void OnGeometryChange(GeometryChangedEvent evt)
    {
        SetVisualElements();


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }



    private void OnDetachFromPanel(DetachFromPanelEvent evt)
    {
        // Debug.Log($"Detached/end");
        // Actions.EndOfQuizQuestions -= EnableEndOfQuizPanel;
        // endOfQuizPanel.style.display = DisplayStyle.None;

    }
    private void SetVisualElements()
    {
        // endOfQuizPanel = this.Q(EndOfQuizPanelString);
        // endOfQuizPanel.style.display = DisplayStyle.None;
        // Debug.Log($"endOfQuizPanel: {endOfQuizPanel}");

    }
    private void RegisterCallbacks()
    {

        // Actions.EndOfQuizQuestions += EnableEndOfQuizPanel;
    }

    public void EnableEndOfQuizPanel()
    {
        // endOfQuizPanel.style.display = DisplayStyle.Flex;
        // endOfQuizPanel.BringToFront();


        // Debug.Log($"displaying end of quiz panel");

    }



}
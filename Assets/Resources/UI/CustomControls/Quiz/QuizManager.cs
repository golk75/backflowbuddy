using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuizManager : VisualElement
{


    public new class UxmlFactory : UxmlFactory<QuizManager, UxmlTraits> { }

    public QuizManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);

    }

    private void OnAttachToPanel(AttachToPanelEvent evt)
    {


    }

    private void OnEndOfQuiz()
    {
        Debug.Log($"end of quiz");
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void EnableEndOfQuizPanel()
    {

    }
}
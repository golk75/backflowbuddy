using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuizSelection : VisualElement
{
    Button m_QuizSelectionButton1;
    Button m_QuizSelectionButton2;
    Button m_QuizSelectionButton3;

    public new class UxmlFactory : UxmlFactory<QuizSelection, UxmlTraits> { }

    public QuizSelection()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        // m_QuizSelectionButton1 = this.Q<Button>("quiz-1");
        // m_QuizSelectionButton2 = this.Q<Button>("quiz-2");
        // m_QuizSelectionButton3 = this.Q<Button>("quiz-3");


        // m_QuizSelectionButton1.RegisterCallback<ClickEvent>(evt => Quiz1Selected());
        // m_QuizSelectionButton2.RegisterCallback<ClickEvent>(evt => Quiz2Selected());
        // m_QuizSelectionButton3.RegisterCallback<ClickEvent>(evt => Quiz3Selected());
        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void Quiz3Selected()
    {
        Actions.onQuizSelection?.Invoke(100);
    }

    private void Quiz2Selected()
    {
        Actions.onQuizSelection?.Invoke(50);
    }

    private void Quiz1Selected()
    {
        // Actions.onQuizSelection?.Invoke(25);
        Actions.onQuizSelection?.Invoke(5);
        style.display = DisplayStyle.None;

    }
}
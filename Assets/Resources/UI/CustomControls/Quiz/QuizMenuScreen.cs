using System;
using System.ComponentModel.Design.Serialization;
using System.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuizMenuScreen : VisualElement
{
    private Button m_QuitToMenuButton;
    private Button m_ResumeQuizButton;
    private Button m_QuizMenuCloseButton;

    public new class UxmlFactory : UxmlFactory<QuizMenuScreen, UxmlTraits> { }

    public QuizMenuScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        // m_QuitToMenuButton = this.Q<Button>("quit-quiz");
        // m_ResumeQuizButton = this.Q<Button>("resume-quiz");
        // m_QuizMenuCloseButton = this.Q<Button>("close-button");


        // m_ResumeQuizButton.RegisterCallback<ClickEvent>(evt => ResumeQuiz());
        // m_QuitToMenuButton.RegisterCallback<ClickEvent>(evt => QuitToMainMenu());
        // m_QuizMenuCloseButton.RegisterCallback<ClickEvent>(evt => ResumeQuiz());

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ResumeQuiz()
    {
        style.display = DisplayStyle.None;
    }
}
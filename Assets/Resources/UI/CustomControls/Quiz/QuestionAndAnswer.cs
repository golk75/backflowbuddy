using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuestionAndAnswer : VisualElement
{


    private const string MenuModalString = "MenuModal";
    private const string MenuButtonString = "quiz-menu-button";

    Button m_QuizMenuButton;
    VisualElement m_MenuModal;



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

        m_QuizMenuButton.RegisterCallback<ClickEvent>(evt => EnableMenuModal());

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void EnableMenuModal()
    {
        m_MenuModal.style.display = DisplayStyle.Flex;
        Debug.Log(m_MenuModal.style.display);
    }




}
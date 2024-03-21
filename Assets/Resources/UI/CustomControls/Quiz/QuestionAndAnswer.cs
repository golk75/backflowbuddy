using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuestionAndAnswer : VisualElement
{

    private const string EndOfQuizPanelString = "EndOfQuizPanel";



    public new class UxmlFactory : UxmlFactory<QuestionAndAnswer, UxmlTraits> { }



    public QuestionAndAnswer()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);



    }


    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }






}
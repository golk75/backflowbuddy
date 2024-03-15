using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuizNavigate : VisualElement
{




    public new class UxmlFactory : UxmlFactory<QuizNavigate, UxmlTraits> { }

    public QuizNavigate()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }





}
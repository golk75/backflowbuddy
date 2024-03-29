using System;
using System.ComponentModel.Design.Serialization;
using System.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class QuizGameMenuScreen : VisualElement
{




    public new class UxmlFactory : UxmlFactory<QuizGameMenuScreen, UxmlTraits> { }

    public QuizGameMenuScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }



}
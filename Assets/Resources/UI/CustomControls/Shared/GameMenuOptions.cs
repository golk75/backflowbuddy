using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameMenuOptions : VisualElement
{

    public new class UxmlFactory : UxmlFactory<GameMenuOptions, UxmlTraits> { }

    public GameMenuOptions()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {



        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }


}
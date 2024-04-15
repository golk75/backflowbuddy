using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayScreen : VisualElement
{


    public new class UxmlFactory : UxmlFactory<PlayScreen, UxmlTraits> { }

    public PlayScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

}
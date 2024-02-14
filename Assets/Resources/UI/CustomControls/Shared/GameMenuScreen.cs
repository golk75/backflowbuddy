using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameMenuScreen : VisualElement
{
    public VisualElement m_GameMenuOptions;
    public VisualElement m_GameMenuHome;
    public new class UxmlFactory : UxmlFactory<GameMenuScreen, UxmlTraits> { }

    public GameMenuScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {

        this?.Q("quick-tour-button")?.RegisterCallback<ClickEvent>(evt => StartQuickTour());

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void StartQuickTour()
    {

        PlayerPrefs.SetInt("Skip Tutorial", 0);
        if (SceneManager.GetActiveScene().name == "RPZPlayScene")
        {
            SceneManager.LoadSceneAsync("RPZPlayScene");
        }
        else if (SceneManager.GetActiveScene().name == "DCPlayScene")
        {
            SceneManager.LoadSceneAsync("DCPlayScene");
        }

    }
}
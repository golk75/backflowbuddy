using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class DCPlayMenuScreen : VisualElement
{


    public VisualElement m_GameMenuOptions;
    public VisualElement m_GameMenuScreen;

    public new class UxmlFactory : UxmlFactory<DCPlayMenuScreen, UxmlTraits> { }

    public DCPlayMenuScreen()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        m_GameMenuScreen = this.Q("GameMenuScreen");
        m_GameMenuOptions = this.Q("GameMenuOptions");
        m_GameMenuScreen.Q("options-button")?.RegisterCallback<ClickEvent>(evt => OpenOptions());
        m_GameMenuOptions?.Q("close-button")?.RegisterCallback<ClickEvent>(evt => CloseMenu());
        m_GameMenuOptions?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => ShowMenu());


        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void ShowMenu()
    {
        m_GameMenuScreen.style.display = DisplayStyle.Flex;
        m_GameMenuOptions.style.display = DisplayStyle.None;
    }

    private void CloseMenu()
    {
        m_GameMenuOptions.style.display = DisplayStyle.None;
    }

    private void OpenOptions()
    {
        m_GameMenuScreen.style.display = DisplayStyle.None;
        m_GameMenuOptions.style.display = DisplayStyle.Flex;

    }



    // void SetVisualElements()
    // {
    //     m_DCTestScreen = GetComponent<UIDocument>();
    //     VisualElement root = m_DCTestScreen.rootVisualElement;
    //     m_MenuScreen = root.Q(m_MenuScreenName);
    //     m_OptionsScreen = root.Q(m_OptionsScreenName);
    //     m_MenuButton = root.Q<Button>(MenuButtonString);
    //     m_ResumeButton = root.Q<Button>(ResumeButtonString);
    //     m_OptionsButton = root.Q<Button>(OptionsButtonString);
    //     m_CloseMenuButton = root.Q<Button>(CloseMenuButtonString);
    //     m_QuitToMenuButton = root.Q<Button>(QuitToMenuButtonString);

    // }
    // void RegisterButtonCallBacks()
    // {
    //     m_MenuButton?.RegisterCallback<ClickEvent>(ShowGameMenuScreen);
    //     m_ResumeButton?.RegisterCallback<ClickEvent>(ResumeGame);
    //     m_OptionsButton?.RegisterCallback<ClickEvent>(ShowOptions);
    //     m_CloseMenuButton?.RegisterCallback<ClickEvent>(ResumeGame);
    //     m_QuitToMenuButton?.RegisterCallback<ClickEvent>(QuitGame);

    // }

    // private void ShowOptions(ClickEvent evt)
    // {
    //     ShowVisualElement(m_OptionsScreen, true);
    //     ShowVisualElement(m_MenuScreen, false);
    // }

    // private void ResumeGame(ClickEvent evt)
    // {
    //     // GameResumed?.Invoke();

    //     ShowVisualElement(m_MenuScreen, false);

    // }

    // private void ShowGameMenuScreen(ClickEvent evt)
    // {
    //     ShowVisualElement(m_MenuScreen, true);

    // }
    // void QuitGame(ClickEvent evt)
    // {


    //     SceneManager.LoadSceneAsync(m_MenuMenuSceneName);
    //     // _bannerViewController.DestroyAd();

    // }
    // void ShowVisualElement(VisualElement visualElement, bool state)
    // {
    //     if (visualElement == null)
    //         return;

    //     visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;

    // }
}
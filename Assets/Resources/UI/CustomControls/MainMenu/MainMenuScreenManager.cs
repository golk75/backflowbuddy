using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScreenManager : VisualElement
{
    public VisualElement m_MainMenuScreen;
    public VisualElement m_LearnScreen;
    public VisualElement m_PlayScreen;
    public VisualElement m_PlayScreenQuickTour;
    public VisualElement m_QuickTourContainer;
    public VisualElement m_DeviceSelectionScreen;
    public VisualElement m_LearnScreenRpzPopup;
    private VisualElement m_LearnScreenComingSoonPopup;
    public VisualElement m_PlayScreenRpzPopup;


    private const string MainMenuPlayButtonString = "play-button";
    private const string MainMenuLearnButtonString = "learn-button";
    private const string MainMenuQuitButtonString = "quit-button";
    private const string MainMenuQuizButtonString = "quiz-button";
    private const string RpzPopupString = "rpz-pop-up";
    private const string RpzBackButtonString = "rpz-popup-back-button";

    private const string QuickTourContainerString = "QuickTourContainer";
    private const string DeviceSelectionScreenString = "DeviceSelectionScreen";

    private const string DCTestSceneString = "DCPlayScene";


    private const string TutorialPlayerPrefString = "Skip Tutorial";

    private List<VisualElement> ScreensToHide;
    private List<VisualElement> ScreensToShow;
    private List<VisualElement> PlayScreenButtons;
    private List<VisualElement> MainMenuScreenButtons;
    private List<VisualElement> QuizSelectionScreenButtons;

    public new class UxmlFactory : UxmlFactory<MainMenuScreenManager, UxmlTraits> { }

    public MainMenuScreenManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        m_MainMenuScreen = this.Q("MainMenuScreen");
        m_PlayScreen = this.Q("PlayScreen");
        m_PlayScreenQuickTour = this.Q("PlayScreenQuickTour");
        m_QuickTourContainer = this.Q(QuickTourContainerString);
        m_DeviceSelectionScreen = this.Q(DeviceSelectionScreenString);
        m_PlayScreenRpzPopup = m_PlayScreen.Q(RpzPopupString);
        PlayScreenButtons = m_PlayScreen.Query(className: "unity-button").ToList();
        MainMenuScreenButtons = m_MainMenuScreen.Query(className: "unity-button").ToList();
        QuizSelectionScreenButtons = m_MainMenuScreen.Query(className: "unity-button").ToList();

        //main menu
        m_MainMenuScreen?.Q(MainMenuPlayButtonString)?.RegisterCallback<ClickEvent>(evt => EnablePlayScreen());
        m_MainMenuScreen?.Q(MainMenuLearnButtonString)?.RegisterCallback<ClickEvent>(evt => EnableLearnScreen());
        m_MainMenuScreen?.Q(MainMenuQuitButtonString)?.RegisterCallback<ClickEvent>(evt => QuitApplication());
        m_MainMenuScreen.Q(MainMenuQuizButtonString)?.RegisterCallback<ClickEvent>(evt => EnableLearnScreen());
        //play screen
        m_PlayScreen?.Q("rpz-button").RegisterCallback<ClickEvent>(evt => EnableRPZPlayScreenAndScene());
        m_PlayScreen?.Q("double-check-button").RegisterCallback<ClickEvent>(evt => EnableDoubleCheckPlayScreenAndScene());
        m_PlayScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());
        //play screen
        m_PlayScreen?.Q("rpz-button").RegisterCallback<ClickEvent>(evt => EnableRPZPlayScreenAndScene());
        m_PlayScreen?.Q("double-check-button").RegisterCallback<ClickEvent>(evt => EnableDoubleCheckPlayScreenAndScene());
        m_PlayScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());



        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
    private void EnableRPZPlayScreenAndScene()
    {
        SceneManager.LoadScene("RPZPlayScene");
    }
    private void EnableDoubleCheckPlayScreenAndScene()
    {
        SceneManager.LoadScene("DCPlayScene");
    }

    private void EnablePlayScreen()
    {
        m_MainMenuScreen.style.display = DisplayStyle.None;

        m_PlayScreen.style.display = DisplayStyle.Flex;

        foreach (var button in PlayScreenButtons)
        {
            button.RemoveFromClassList("main-menu-button-out");
            button.AddToClassList("main-menu-button");
        }
        foreach (var button in MainMenuScreenButtons)
        {
            button.RemoveFromClassList("main-menu-button");
            button.AddToClassList("main-menu-button-out");
        }

    }

    private void EnableTitleScreen()
    {

        m_MainMenuScreen.style.display = DisplayStyle.Flex;
        m_PlayScreen.style.display = DisplayStyle.None;
        //  m_LearnScreen.style.display = DisplayStyle.None;
        foreach (var button in PlayScreenButtons)
        {
            button.RemoveFromClassList("main-menu-button");
            button.AddToClassList("main-menu-button-out");
        }
        foreach (var button in MainMenuScreenButtons)
        {
            button.RemoveFromClassList("main-menu-button-out");
            button.AddToClassList("main-menu-button");
        }


    }

    private void EnableLearnScreen()
    {
        // m_LearnScreen.style.display = DisplayStyle.None;
        m_MainMenuScreen.style.display = DisplayStyle.None;
        m_PlayScreen.style.display = DisplayStyle.None;
        SceneManager.LoadScene("Quiz");

    }

    private void QuitApplication()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
#endif
            Application.Quit();

    }


}
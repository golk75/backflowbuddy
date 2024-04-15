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
    public new class UxmlFactory : UxmlFactory<MainMenuScreenManager, UxmlTraits> { }

    public MainMenuScreenManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        m_MainMenuScreen = this.Q("MainMenuScreen");
        // m_LearnScreen = this.Q("LearnScreen");
        m_PlayScreen = this.Q("PlayScreen");
        m_PlayScreenQuickTour = this.Q("PlayScreenQuickTour");
        m_QuickTourContainer = this.Q(QuickTourContainerString);
        m_DeviceSelectionScreen = this.Q(DeviceSelectionScreenString);
        //  m_LearnScreenRpzPopup = m_LearnScreen.Q(RpzPopupString);
        //m_LearnScreenComingSoonPopup = this.Q("learn-coming-soon-pop-up");
        m_PlayScreenRpzPopup = m_PlayScreen.Q(RpzPopupString);
        //Main Menu screen buttons
        m_MainMenuScreen?.Q(MainMenuPlayButtonString)?.RegisterCallback<ClickEvent>(evt => EnablePlayScreen());
        m_MainMenuScreen?.Q(MainMenuLearnButtonString)?.RegisterCallback<ClickEvent>(evt => EnableLearnScreen());
        m_MainMenuScreen?.Q(MainMenuQuitButtonString)?.RegisterCallback<ClickEvent>(evt => QuitApplication());
        // m_MainMenuScreen.Q(MainMenuLearnButtonString)?.RegisterCallback<ClickEvent>(evt => EnableLearnScreen());
        m_MainMenuScreen.Q(MainMenuQuizButtonString)?.RegisterCallback<ClickEvent>(evt => EnableLearnScreen());
        // m_LearnScreenComingSoonPopup?.Q("learn-popup-back-button")?.RegisterCallback<ClickEvent>(evt => CloseLearnPopup());
        // m_LearnScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());
        m_PlayScreen?.Q("rpz-button").RegisterCallback<ClickEvent>(evt => EnableRPZPlayScreenAndScene());
        m_PlayScreen?.Q("double-check-button").RegisterCallback<ClickEvent>(evt => EnableDoubleCheckPlayScreenAndScene());
        m_PlayScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());
        PlayScreenButtons = m_PlayScreen.Query(className: "unity-button").ToList();
        MainMenuScreenButtons = m_MainMenuScreen.Query(className: "unity-button").ToList();



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
    private void CloseLearnPopup()
    {
        // m_LearnScreenComingSoonPopup.style.display = DisplayStyle.None;
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


    // private void ChangeSceneAndScreen(string sceneToLoad, int playerPref, List<VisualElement> listToShow, List<VisualElement> listToHide)
    // {

    //     //Async Load Scene--> prevents ui from changing until scene is loaded up
    //     //DO NOT CHANGE THE ORDER IN THIS---->
    //     {
    //         AsyncOperation sceneLoadAsync = SceneManager.LoadSceneAsync(sceneToLoad);


    //         // #if UNITY_EDITOR
    //         //             PlayerPrefs.SetInt(TutorialPlayerPrefString, 0);
    //         // #endif


    //         //skipping quick tour

    //         //wait for scene to load before switching Ui
    //         if (sceneLoadAsync.isDone)
    //         {

    //             ChangeScreenStatus(listToShow, listToHide);
    //         }


    //     }


    // }



    // private void ChangeScreenStatus(List<VisualElement> listOfScreensToShow, List<VisualElement> listOfScreensToHide)
    // {
    //     if (listOfScreensToShow == null || listOfScreensToHide == null)
    //     {
    //         return;
    //     }

    //     foreach (var screenToHide in listOfScreensToHide)
    //     {
    //         screenToHide.style.display = DisplayStyle.None;
    //     }
    //     foreach (var screenToShow in listOfScreensToShow)
    //     {

    //         screenToShow.style.display = DisplayStyle.Flex;
    //     }

    //     ScreensToShow.Clear();
    //     ScreensToHide.Clear();
    // }


    private void QuitApplication()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
#endif
            Application.Quit();

    }


}
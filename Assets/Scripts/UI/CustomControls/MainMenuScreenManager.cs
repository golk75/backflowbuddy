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
    public VisualElement m_PlayScreenRpzPopup;


    private const string MainMenuPlayButtonString = "play-button";
    private const string MainMenuLearnButtonString = "learn-button";
    private const string MainMenuQuitButtonString = "quit-button";
    private const string DoubleCheckButtonString = "double-check-button";
    private const string RpzButtonString = "rpz-button";
    private const string RpzPopupString = "rpz-pop-up";
    private const string RpzBackButtonString = "rpz-popup-back-button";

    private const string QuickTourContainerString = "QuickTourContainer";
    private const string DeviceSelectionScreenString = "DeviceSelectionScreen";

    private const string DCTestSceneString = "DCPlayScene";


    private const string TutorialPlayerPrefString = "Skip Tutorial";

    private List<VisualElement> ScreensToHide;
    private List<VisualElement> ScreensToShow;

    public new class UxmlFactory : UxmlFactory<MainMenuScreenManager, UxmlTraits> { }

    public MainMenuScreenManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {



        m_MainMenuScreen = this.Q("MainMenuScreen");
        m_LearnScreen = this.Q("LearnScreen");
        m_PlayScreen = this.Q("PlayScreen");
        m_PlayScreenQuickTour = this.Q("PlayScreenQuickTour");
        m_QuickTourContainer = this.Q(QuickTourContainerString);
        m_DeviceSelectionScreen = this.Q(DeviceSelectionScreenString);
        m_LearnScreenRpzPopup = m_LearnScreen.Q(RpzPopupString);
        m_PlayScreenRpzPopup = m_PlayScreen.Q(RpzPopupString);





        //Main Menu screen buttons
        m_MainMenuScreen?.Q(MainMenuPlayButtonString)?.RegisterCallback<ClickEvent>(evt => EnablePlayScreen());
        m_MainMenuScreen?.Q(MainMenuLearnButtonString)?.RegisterCallback<ClickEvent>(evt => EnableLearnScreen());
        m_MainMenuScreen?.Q(MainMenuQuitButtonString)?.RegisterCallback<ClickEvent>(evt => QuitApplication());

        m_LearnScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());
        m_PlayScreen?.Q("back-button")?.RegisterCallback<ClickEvent>(evt => EnableTitleScreen());

        // m_LearnScreen.style.display = DisplayStyle.None;
        // m_PlayScreen.style.display = DisplayStyle.None;

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }


    private void EnablePlayScreen()
    {
        m_MainMenuScreen.style.display = DisplayStyle.None;
        m_LearnScreen.style.display = DisplayStyle.None;
        m_PlayScreen.style.display = DisplayStyle.Flex;
    }

    private void EnableTitleScreen()
    {
        m_MainMenuScreen.style.display = DisplayStyle.Flex;
        m_PlayScreen.style.display = DisplayStyle.None;
        m_LearnScreen.style.display = DisplayStyle.None;
    }

    private void EnableLearnScreen()
    {
        m_MainMenuScreen.style.display = DisplayStyle.None;
        m_LearnScreen.style.display = DisplayStyle.Flex;
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
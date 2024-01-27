using System;
using System.Collections;
using System.Collections.Generic;
// using GoogleMobileAds.Sample;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;



public class MenuScreen : MonoBehaviour
{

    public static event Action GamePlayed;
    public static event Action GameQuit;

    const string MainMenuPlayButtonString = "PlayButton";
    const string MainMenuQuitButtonString = "QuitButton";
    private const string TutorialPlayerPrefString = "Skip Tutorial";

    //visual elements
    Button m_MainMenuPlayButton;
    Button m_MainMenuQuitButton;

    //scene management
    [SerializeField] string m_DCTestScene_tutorial = "DCTestScene_tutorial";
    [SerializeField] string m_DCTestScene = "DCTestScene";

    UIDocument m_MainMenuScreen;


    void SetVisualElements()
    {
        m_MainMenuScreen = GetComponent<UIDocument>();
        var root = m_MainMenuScreen.rootVisualElement;
        m_MainMenuPlayButton = root.Q<Button>(MainMenuPlayButtonString);
        m_MainMenuQuitButton = root.Q<Button>(MainMenuQuitButtonString);
    }
    void RegisterButtonCallBacks()
    {
        m_MainMenuPlayButton.RegisterCallback<ClickEvent>(PlayGame);
        m_MainMenuQuitButton.RegisterCallback<ClickEvent>(QuitGame);
        // m_MainMenuPlayButton.RegisterCallback<PointerDownEvent>(PlayGame);
        m_MainMenuQuitButton.RegisterCallback<ClickEvent>(QuitGame);
    }

    private void QuitGame(ClickEvent evt)
    {

#if UNITY_EDITOR
        if (Application.isPlaying)
#endif
            Application.Quit();


    }

    void PlayGame(ClickEvent evt)
    {
#if UNITY_EDITOR
        PlayerPrefs.SetInt(TutorialPlayerPrefString, 0);
#endif
        if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 0)
        {
            SceneManager.LoadSceneAsync(m_DCTestScene_tutorial);
        }
        else
        {
            SceneManager.LoadSceneAsync(m_DCTestScene);
        }



    }

    void Start()
    {
        SetVisualElements();
        RegisterButtonCallBacks();
    }

}

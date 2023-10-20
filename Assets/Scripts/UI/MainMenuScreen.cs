using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;



public class MainMenuScreen : MonoBehaviour
{

    public static event Action GamePlayed;
    public static event Action GameQuit;

    const string MainMenuPlayButtonString = "PlayButton";
    const string MainMenuQuitButtonString = "QuitButton";
    [SerializeField] string m_DCTestScene = "DCTestScene";
    Button m_MainMenuPlayButton;
    Button m_MainMenuQuitButton;

    UIDocument m_MainMenuScreen;

    private void OnEnable()
    {


    }
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
        // GameQuit?.Invoke();
    }

    void PlayGame(ClickEvent evt)
    {
        // GamePlayed?.Invoke();

        SceneManager.LoadSceneAsync(m_DCTestScene);
    }
    // void PlayGame(PointerDownEvent evt)
    // {
    //     GamePlayed?.Invoke();
    // }
    private void OnDisable()
    {

    }
    void Start()
    {
        SetVisualElements();
        RegisterButtonCallBacks();
    }

}

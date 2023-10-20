using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{

    //Actions
    public static event Action GameQuit;
    public static event Action<float> GamePaused;
    public static event Action GameResumed;



    [Header("Menu Screen elements")]
    [Tooltip("String IDs to query Visual Elements")]
    [SerializeField] string m_MenuScreenName = "GameMenuScreen";
    [SerializeField] string m_MenuMenusceneName = "MainMenu";

    [Header("Blur")]
    [SerializeField] Volume m_Volume;


    // string Ids
    const string MenuButtonString = "MenuButton";
    const string ResumeButtonString = "GameMenuScreen_resume-button";
    const string CloseMenuButtonString = "GameMenuScreen_close-button";
    const string QuitToMenuButtonString = "GameMenuScreen_main-menu-button";



    // string IDs
    // references to functional UI elements (buttons and screens)
    VisualElement m_MenuScreen;
    Button m_MenuButton;
    Button m_ResumeButton;
    Button m_CloseMenuButton;
    Button m_QuitToMenuButton;


    UIDocument m_DCTestScreen;


    private void OnEnable()
    {
        // SetVisualElements();

    }

    private void OnDisable()
    {

    }
    void SetVisualElements()
    {
        m_DCTestScreen = GetComponent<UIDocument>();
        VisualElement root = m_DCTestScreen.rootVisualElement;
        m_MenuScreen = root.Q(m_MenuScreenName);
        m_MenuButton = root.Q<Button>(MenuButtonString);
        m_ResumeButton = root.Q<Button>(ResumeButtonString);
        m_CloseMenuButton = root.Q<Button>(CloseMenuButtonString);
        m_QuitToMenuButton = root.Q<Button>(QuitToMenuButtonString);




    }
    void RegisterButtonCallBacks()
    {
        m_MenuButton?.RegisterCallback<ClickEvent>(ShowGameMenuScreen);
        m_ResumeButton?.RegisterCallback<ClickEvent>(ResumeGame);
        m_CloseMenuButton?.RegisterCallback<ClickEvent>(ResumeGame);
        m_QuitToMenuButton?.RegisterCallback<ClickEvent>(QuitGame);

    }

    private void ResumeGame(ClickEvent evt)
    {
        // GameResumed?.Invoke();

        ShowVisualElement(m_MenuScreen, false);

    }

    private void ShowGameMenuScreen(ClickEvent evt)
    {
        ShowVisualElement(m_MenuScreen, true);


    }
    void QuitGame(ClickEvent evt)
    {
        // AudioManager.PlayDefaultButtonSound();
        // GameQuit?.Invoke();
        SceneManager.LoadSceneAsync(m_MenuMenusceneName);

    }
    void ShowVisualElement(VisualElement visualElement, bool state)
    {
        if (visualElement == null)
            return;

        visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;

    }


    // Start is called before the first frame update
    void Start()
    {
        SetVisualElements();
        RegisterButtonCallBacks();
    }

    // Update is called once per frame
    void Update()
    {

    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{

    //Actions
    public static event Action GameQuit;

    // notify listeners to pause after delay in seconds
    public static event Action<float> GamePaused;
    public static event Action GameResumed;



    [Header("Menu Screen elements")]
    [Tooltip("String IDs to query Visual Elements")]
    [SerializeField] string m_MenuScreenName = "GameMenuScreen";

    [Header("Blur")]
    [SerializeField] Volume m_Volume;


    // string Ids
    const string MenuButtonString = "MenuButton";
    const string ResumeButtonString = "GameMenuScreen_resume-button";
    const string CloseMenuButton = "GameMenuScreen_close-button";



    // string IDs
    // references to functional UI elements (buttons and screens)
    VisualElement m_MenuScreen;
    Button m_MenuButton;
    Button m_ResumeButton;
    Button m_CloseMenuButton;


    UIDocument m_DCTestScreen;


    private void OnEnable()
    {
        SetVisualElements();
        RegisterButtonCallBacks();
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
        m_CloseMenuButton = root.Q<Button>(CloseMenuButton);



    }
    void RegisterButtonCallBacks()
    {
        m_MenuButton?.RegisterCallback<ClickEvent>(ShowGameMenuScreen);
        m_ResumeButton?.RegisterCallback<ClickEvent>(ResumeGame);
        m_CloseMenuButton?.RegisterCallback<ClickEvent>(ResumeGame);
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

    void ShowVisualElement(VisualElement visualElement, bool state)
    {
        if (visualElement == null)
            return;

        visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}

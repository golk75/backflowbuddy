using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GameScreen : MonoBehaviour
{



    // notify listeners to pause after delay in seconds
    public static event Action<float> GamePaused;
    public static event Action GameResumed;


    [Header("Menu Screen elements")]
    [Tooltip("String IDs to query Visual Elements")]
    [SerializeField] string m_MenuScreenName = "GameMenuScreen";

    [Header("Blur")]
    [SerializeField] Volume m_Volume;


    // string Ids
    const string MenuButtonString = "MenuButtonString";



    // string IDs
    // references to functional UI elements (buttons and screens)
    VisualElement m_MenuScreen;
    Button m_MenuButton;
    UIDocument m_DCTestScreen;


    private void OnEnable()
    {
        SetVisualElements();
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

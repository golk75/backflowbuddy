using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TutorialPopupTrigger : MonoBehaviour
{
    //visual element constants
    private const string QuickTourContainerString = "QuickTourContainer";
    private const string TutorialNextButtonString = "Tutotial_next_button";
    private const string TutorialPrevButtonString = "Tutotial_previous_button";
    private const string TutorialSkipButtonString = "Tutotial_skip_button";
    private const string TutorialPopupHeaderString = "TutorialPopup_header";
    private const string TutorialPopupContentString = "TutorialPopup_content";
    private const string OptionsTutorialButtonString = "OptionsMenuScreen_tutorial_button";
    private const string TutorialPlayerPrefString = "Skip Tutorial";
    private const string TutorialButtonContainerString = "TutorialPopup_button_container";

    //style classes
    private const string StartQuickTourButtonString = "tutorial-popup-button-start";
    private const string QuickTourContainerDarkStyle = "quick-tour-container-light";
    private const string QuickTourContainerLightStyle = "quick-tour-container-dark";



    //visual elements
    public VisualElement m_QuickTourContainer;
    private VisualElement m_PopupHeader;
    private VisualElement m_PopupContent;
    private VisualElement m_ButtonContainer;
    private Button m_NextButton;
    private Button m_PreviousButton;
    private Button m_SkipButton;
    private Button m_OptionsTutorialButton;


    //scene management
    [SerializeField] string m_DCTestScene_tutorial = "DCTestScene_tutorial";
    [SerializeField] string m_DCTestScene = "DCTestScene";



    //gameobjects
    public GameObject m_GameUi;


    //root
    private UIDocument root;


    public TutorialPopUpScriptableObject[] PopupScriptableObjects;


    //throw away
    public int popupIndex = 0;
    public int skipTutPlayerPrefTestInt = 0;


    private void Awake()
    {
        root = m_GameUi.GetComponent<UIDocument>();

        AssignVisualElements();
        RegisterCallbacks();
        popupIndex = 0;

        if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 0)
        {
            m_QuickTourContainer.style.display = DisplayStyle.Flex;

        }

    }

    private void AssignVisualElements()
    {
        m_NextButton = root.rootVisualElement.Q<Button>(TutorialNextButtonString);
        m_PreviousButton = root.rootVisualElement.Q<Button>(TutorialPrevButtonString);
        m_SkipButton = root.rootVisualElement.Q<Button>(TutorialSkipButtonString);
        m_OptionsTutorialButton = root.rootVisualElement.Q<Button>(OptionsTutorialButtonString);
        m_QuickTourContainer = root.rootVisualElement.Q<VisualElement>(QuickTourContainerString);
        m_PopupHeader = root.rootVisualElement.Q<VisualElement>(TutorialPopupHeaderString);
        m_PopupContent = root.rootVisualElement.Q<VisualElement>(TutorialPopupContentString);
        m_ButtonContainer = root.rootVisualElement.Q<VisualElement>(TutorialButtonContainerString);

    }

    //register call backs
    private void RegisterCallbacks()
    {
        m_NextButton.clicked += OnNextButtonClicked;
        m_SkipButton.clicked += OnSkipButtonClicked;
        m_PreviousButton.clicked += OnPrevButtonClicked;
        m_OptionsTutorialButton.clicked += OnOptionsTutorialButtonClicked;
    }


    private void OnOptionsTutorialButtonClicked()
    {
        SaveTutorialPrefs(0);
        SceneManager.LoadSceneAsync(m_DCTestScene_tutorial);

    }


    private void OnNextButtonClicked()
    {
        if (popupIndex < PopupScriptableObjects.Length - 1)
        {
            popupIndex++;
        }

    }


    private void OnPrevButtonClicked()
    {
        if (popupIndex >= 1)
        {
            popupIndex--;
        }
    }


    private void OnSkipButtonClicked()
    {
        SaveTutorialPrefs(1);

    }


    public void SaveTutorialPrefs(int pref)
    {
        PlayerPrefs.SetInt(TutorialPlayerPrefString, pref);
    }


    void OnDisable()
    {
        UnRegisterCallbacks();
    }


    private void UnRegisterCallbacks()
    {
        m_NextButton.clicked -= OnNextButtonClicked;
        m_SkipButton.clicked -= OnSkipButtonClicked;
        m_PreviousButton.clicked -= OnPrevButtonClicked;
        m_OptionsTutorialButton.clicked -= OnOptionsTutorialButtonClicked;
    }


    private void UpdatePopup()
    {
        // if (popupIndex == 1)
        // {
        //     TutorialSystem.Show(PopupScriptableObjects[popupIndex].content, PopupScriptableObjects[popupIndex].header);
        // }
        TutorialSystem.Show(PopupScriptableObjects[popupIndex].content, PopupScriptableObjects[popupIndex].header);
        if (popupIndex == 0)
        {

            m_PopupContent.style.display = DisplayStyle.None;
            m_NextButton.AddToClassList(StartQuickTourButtonString);
            m_NextButton.text = "Start A Quick Tour";
            m_ButtonContainer.style.flexDirection = FlexDirection.ColumnReverse;
            m_PreviousButton.style.display = DisplayStyle.None;

        }
        else if (popupIndex == PopupScriptableObjects.Length - 1)
        {
            m_NextButton.style.display = DisplayStyle.None;
            m_SkipButton.RemoveFromClassList("skip-button");
        }
        else
        {
            m_NextButton.RemoveFromClassList(StartQuickTourButtonString);
            m_NextButton.text = "Next";
            m_ButtonContainer.style.flexDirection = FlexDirection.Row;
            m_PreviousButton.style.display = DisplayStyle.Flex;
            m_PopupContent.style.display = DisplayStyle.Flex;
        }
        if (popupIndex == PopupScriptableObjects.Length - 1)
        {

            m_NextButton.style.display = DisplayStyle.None;
            m_PopupHeader.style.display = DisplayStyle.None;

            m_SkipButton.text = "Close";
        }
        else
        {
            m_NextButton.style.display = DisplayStyle.Flex;
            m_PopupHeader.style.display = DisplayStyle.Flex;
        }
        if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 1)
            SceneManager.LoadScene(m_DCTestScene);
    }


    void Update()
    {
        UpdatePopup();


    }
//     using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UIElements;
// using DG.Tweening;
// public class MainMenuScreen : MonoBehaviour
// {
//     [SerializeField] private UIDocument m_uIDocument;
//     [SerializeField] private StyleSheet m_styleSheet;
//     Button animateButton;
//     VisualElement testBox;
//     Button stopAnimateBtn;
//     // Quaternion initTestBoxRot;
//     Vector3 initTestBoxPos;
//     bool stopAnimationButtonPressed;
//     VisualElement root;
//     Tween GrowTween;
//     Tween ShrinkTween;

//     public Coroutine OnAnimateGrow { get; private set; }

//     private void Start()
//     {
//         root = m_uIDocument.rootVisualElement;
//         animateButton = root.Q<Button>("animate-button");
//         stopAnimateBtn = root.Q<Button>("stop-animate");
//         testBox = root.Q<VisualElement>("test-box");

//         animateButton.RegisterCallback<ClickEvent>(ClickedAnimate);
//         testBox.RegisterCallback<ClickEvent>(ClickedAnimate);
//         stopAnimateBtn.RegisterCallback<ClickEvent>(StopAnimation);

//         initTestBoxPos = testBox.transform.scale;


//     }

//     private void StopAnimation(ClickEvent evt)
//     {
//         stopAnimationButtonPressed = true;
//         DOTween.KillAll();
//         testBox.transform.scale = initTestBoxPos;
//     }

//     private void ClickedAnimate(ClickEvent evt)
//     {
//         GrowTween = DOTween.To(()
//            => testBox.transform.scale,
//            x => testBox.transform.scale = x,
//            new Vector3(2f, 2f, 2f), 0.25f)
//            .SetEase(Ease.Linear);

//         ShrinkTween = DOTween.To(()
//                         => testBox.transform.scale,
//                         x => testBox.transform.scale = x,
//                         new Vector3(1f, 1f, 1f), 0.25f)
//                         .SetEase(Ease.Linear);


//         StartCoroutine(AnimateUI());
//     }


//     private IEnumerator AnimateUI()
//     {

//         // Tween outerTween = DOTween.To(()
//         //              => testBox.worldTransform.rotation.eulerAngles,
//         //              x => testBox.transform.rotation = Quaternion.Euler(x),
//         //              new Vector3(0, 0, 360), 5 / 0.5f)
//         //              .SetEase(Ease.Linear);  
//         Sequence mySequence = DOTween.Sequence();
//         mySequence.Append(GrowTween).Append(ShrinkTween).SetLoops(-1);

//         yield return mySequence.WaitForKill();


//     }





//     //Wait until tweens finish (+1 extra second for display purposes) 


//     //Disable the visiblity

// }






}

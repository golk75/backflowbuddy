using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;

using DG.Tweening;
// using Unity.VisualScripting;
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
    private const string MenuButtonString = "MenuButton";
    private const string FillButtonString = "FillButton";
    private const string ResetButtonString = "ResetButton";
    private const string PauseButtonString = "PauseButton";
    private const string PlayButtonString = "PlayButton";
    private const string SupplyPanelString = "SupplyPressure__panel";
    private const string Zone2PanelString = "PressureZone__two_panel";
    private const string Zone3PanelString = "PressureZone3__three_panel";
    //visual elements to animate 
    private const string MenuButtonFlashingString = "MenuButton-flash";
    private const string FillButtonFlashingString = "FillButton-flash";
    private const string ResetButtonFlashingString = "ResetButton-flash";
    private const string PauseButtonFlashingString = "PauseButton-flash";
    private const string PlayButtonFlashingString = "PlayButton-flash";
    private const string SupplyPanelFlashingString = "SupplyPressure__panel-flash";
    private const string PressureZone2FlashingString = "PressureZone__two_panel-flash";
    private const string PressureZone3FlashingString = "PressureZone__three_panel-flash";


    private const string TestFillButtonString = "TestFillButton";

    //style classes
    private const string StartQuickTourButtonString = "tutorial-popup-button-start";
    private const string QuickTourContainerDarkStyle = "quick-tour-container-light";
    private const string QuickTourContainerLightStyle = "quick-tour-container-dark";



    //visual elements
    public VisualElement m_QuickTourContainer;
    private VisualElement m_PopupHeader;
    private VisualElement m_PopupContent;
    private VisualElement m_ButtonContainer;
    private VisualElement m_SupplyPanel;
    private VisualElement m_PressureZone2Panel;
    private VisualElement m_PressureZone3Panel;
    private Button m_NextButton;
    private Button m_PreviousButton;
    private Button m_SkipButton;
    private Button m_OptionsTutorialButton;
    private Button m_MenuButton;
    private Button m_FillButton;
    private Button m_ResetButton;
    private Button m_PauseButton;
    private Button m_PlayButton;
    private Button m_MenuButton_flashing;
    private Button m_FillButton_flashing;
    private Button m_ResetButton_flashing;
    private Button m_PauseButton_flashing;
    private Button m_PlayButton_flashing;
    private VisualElement m_SupplyPanel_flashing;
    private VisualElement m_Zone2_flashing;
    private VisualElement m_Zone3_flashing;

    private VisualElement originalElementToCopy;
    private VisualElement elementToAnimate;





    //scene management
    [SerializeField] string m_DCTestScene_tutorial = "DCTestScene_tutorial";
    [SerializeField] string m_DCTestScene = "DCTestScene";



    //gameobjects
    public GameObject m_GameUi;


    //root
    private UIDocument root;

    //arrays
    public TutorialPopUpScriptableObject[] PopupScriptableObjects;
    public VisualElement[] ElemensToAnimate;

    //Vectors

    Vector2 originalElementToCopyPos;

    //throw away
    public int popupIndex = 0;
    public int skipTutPlayerPrefTestInt = 0;


    //DOTween
    Tween GrowTween;
    Tween ShrinkTween;


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
        m_MenuButton = root.rootVisualElement.Q<Button>(MenuButtonString);
        m_FillButton = root.rootVisualElement.Q<Button>(FillButtonString);
        m_ResetButton = root.rootVisualElement.Q<Button>(ResetButtonString);
        m_PauseButton = root.rootVisualElement.Q<Button>(PauseButtonString);
        m_PlayButton = root.rootVisualElement.Q<Button>(PlayButtonString);
        m_SupplyPanel = root.rootVisualElement.Q<VisualElement>(SupplyPanelString);
        m_PressureZone2Panel = root.rootVisualElement.Q<VisualElement>(Zone2PanelString);
        m_PressureZone3Panel = root.rootVisualElement.Q<VisualElement>(Zone3PanelString);

        //flashing elements
        m_MenuButton_flashing = root.rootVisualElement.Q<Button>(MenuButtonFlashingString);
        m_FillButton_flashing = root.rootVisualElement.Q<Button>(FillButtonFlashingString);
        m_ResetButton_flashing = root.rootVisualElement.Q<Button>(ResetButtonFlashingString);
        m_PauseButton_flashing = root.rootVisualElement.Q<Button>(PauseButtonFlashingString);
        m_PlayButton_flashing = root.rootVisualElement.Q<Button>(PlayButtonFlashingString);

        m_SupplyPanel_flashing = root.rootVisualElement.Q<VisualElement>(SupplyPanelFlashingString);
        m_Zone2_flashing = root.rootVisualElement.Q<VisualElement>(PressureZone2FlashingString);
        m_Zone3_flashing = root.rootVisualElement.Q<VisualElement>(PressureZone3FlashingString);



    }

    private void PositionFlashingElement(VisualElement originalEle, VisualElement flashingEle)
    {
        if (originalEle == null || flashingEle == null)
            return;
        var pos = originalEle.parent.parent.LocalToWorld(originalEle.transform.position);
        // m_TestFillButton.transform.position = new Vector2(10, 10);
        flashingEle.transform.position = originalEle.LocalToWorld(pos);


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

    //proceed to next popup window
    private void OnNextButtonClicked()
    {
        DOTween.KillAll();
        if (elementToAnimate != null)
        {
            elementToAnimate.transform.scale = new Vector3(1, 1, 1);
            elementToAnimate.style.display = DisplayStyle.None;
        }
        if (popupIndex < PopupScriptableObjects.Length - 1)
        {
            //move to next scriptable object
            popupIndex++;



            //change 'element to animate' data to Tween within AnimateUI() (Coroutine)
            switch (popupIndex)
            {


                case 1:
                    elementToAnimate = m_FillButton_flashing;
                    originalElementToCopy = m_FillButton;
                    break;
                case 2:
                    elementToAnimate = m_PauseButton_flashing;
                    originalElementToCopy = m_PauseButton;
                    break;
                case 3:
                    elementToAnimate = m_PlayButton_flashing;
                    originalElementToCopy = m_PlayButton;
                    break;
                case 4:
                    elementToAnimate = m_ResetButton_flashing;
                    originalElementToCopy = m_ResetButton;
                    break;
                case 5:
                    elementToAnimate = m_MenuButton_flashing;
                    originalElementToCopy = m_MenuButton;
                    break;
                case 6:
                    elementToAnimate = m_SupplyPanel_flashing;
                    originalElementToCopy = m_SupplyPanel;
                    break;
                case 7:
                    elementToAnimate = m_Zone2_flashing;
                    originalElementToCopy = m_PressureZone2Panel;

                    break;
                case 8:
                    elementToAnimate = null;
                    break;

            }

            //re-position flashing element to match origianl element position
            PositionFlashingElement(originalElementToCopy, elementToAnimate);

            //hide origianl element
            if (elementToAnimate != null)
            {
                elementToAnimate.style.display = DisplayStyle.Flex;

                // Debug.Log($"elementToAnimate.style.display: {elementToAnimate.style.display}");


                //Tween scale (may explore other properties later)

                GrowTween = DOTween.To(()
                   => elementToAnimate.transform.scale,
                   x => elementToAnimate.transform.scale = x,
                   new Vector3(1.2f, 1.2f, 1.2f), 0.5f)
                   .SetEase(Ease.Linear);

                ShrinkTween = DOTween.To(()
                                => elementToAnimate.transform.scale,
                                x => elementToAnimate.transform.scale = x,
                                new Vector3(1f, 1f, 1f), 0.5f)
                                .SetEase(Ease.Linear);







                //animate flashing element    
                StartCoroutine(AnimateUi());
            }
        }

    }


    //go back to previous popup window
    private void OnPrevButtonClicked()
    {
        if (popupIndex >= 1)
        {


            popupIndex--;

        }
    }

    //skip and close popup window
    private void OnSkipButtonClicked()
    {
        //save prefs to device to not show tutorial on scene open (unless opened through options)
        SaveTutorialPrefs(1);
        DOTween.KillAll();
    }

    //cache tutorial skipping preference to device memory
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

    //update popp window style and text (text is taken from scriptable objects array)
    private void UpdatePopup()
    {
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
            DOTween.KillAll();
            m_SkipButton.text = "Close";
        }
        else
        {
            m_NextButton.style.display = DisplayStyle.Flex;
            m_PopupHeader.style.display = DisplayStyle.Flex;
        }

        //highlight/focus buttons referenced in popup window(s)




        if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 1)
        {
            DOTween.KillAll();
            SceneManager.LoadScene(m_DCTestScene);

        }
    }


    private IEnumerator AnimateUi()
    {

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(GrowTween).Append(ShrinkTween).SetLoops(-1).SetEase(Ease.Linear);

        yield return mySequence.WaitForKill();

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
    //            new Vector3(2f, 2f, 2f), 0.5f)
    //            .SetEase(Ease.Linear);

    //         ShrinkTween = DOTween.To(()
    //                         => testBox.transform.scale,
    //                         x => testBox.transform.scale = x,
    //                         new Vector3(1f, 1f, 1f), 0.5f)
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

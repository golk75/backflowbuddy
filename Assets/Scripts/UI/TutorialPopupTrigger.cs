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
    private const string QuickTourButtonString = "quick-tour-button";
    private const string TutorialPlayerPrefString = "Skip Tutorial";
    private const string TutorialButtonContainerString = "TutorialPopup_button_container";
    private const string MenuButtonString = "MenuButton";
    private const string FillButtonString = "FillButton";
    private const string ResetButtonString = "ResetButton";
    private const string PauseButtonString = "PauseButton";
    private const string PlayButtonString = "PlayButton";
    private const string SupplyPanelString = "SupplyPressureZone__panel";
    private const string Zone2PanelString = "PressureZone__two_panel";
    private const string Zone3PanelString = "PressureZone__three_panel";
    //visual elements to animate 
    private const string MenuButtonFlashingString = "MenuButton-flash";
    private const string FillButtonFlashingString = "FillButton-flash";
    private const string ResetButtonFlashingString = "ResetButton-flash";
    private const string PauseButtonFlashingString = "PauseButton-flash";
    private const string PlayButtonFlashingString = "PlayButton-flash";
    private const string SupplyPanelFlashingString = "SupplyPressure__panel-flash";
    private const string PressureZone2FlashingString = "PressureZone__two_panel-flash";
    private const string PressureZone3FlashingString = "PressureZone__three_panel-flash";


    //style classes
    private const string StartQuickTourButtonString = "tutorial-popup-button-start";


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
    private Button m_QuickTourButton;
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


    //scene management
    [SerializeField] string m_DCTestScene_tutorial = "DCTestScene_tutorial";
    [SerializeField] string m_DCPlayScene = "DCPlayScene";



    //gameobjects
    public GameObject m_GameUi;


    //root
    private UIDocument root;

    //arrays
    public TutorialPopUpScriptableObject[] PopupScriptableObjects;
    public VisualElement[,] ElementsToAnimateArr;

    //lists\
    public List<VisualElement> ElementsToAnimate = new();
    public List<VisualElement> OriginalElementsToAnimateCopy = new();


    public int m_popupIndex = 0;


    //DOTween
    Tween GrowTween;
    Tween ShrinkTween;
    Tween GrowTween2;
    Tween ShrinkTween2;
    Tween GrowTween3;
    Tween ShrinkTween3;
    public Ease GrowEase;
    public Ease ShrinkEase;
    public Vector3 tweenScaleUp = new Vector3(1.2f, 1.2f, 1.2f);
    public float tweenScaleUpSpeed;


    private void Awake()
    {
        root = m_GameUi.GetComponent<UIDocument>();

        AssignVisualElements();
        RegisterCallbacks();
        m_popupIndex = 0;

        if (PlayerPrefs.GetInt(TutorialPlayerPrefString) == 0)
        {
            m_QuickTourContainer.style.display = DisplayStyle.Flex;

        }

        ElementsToAnimateArr = new VisualElement[2, 2];



    }

    private void AssignVisualElements()
    {




        m_NextButton = root.rootVisualElement.Q<Button>(TutorialNextButtonString);
        m_PreviousButton = root.rootVisualElement.Q<Button>(TutorialPrevButtonString);
        m_SkipButton = root.rootVisualElement.Q<Button>(TutorialSkipButtonString);
        m_QuickTourButton = root.rootVisualElement.Q<Button>(QuickTourButtonString);
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

    private void PositionFlashingElement(List<VisualElement> originalElementList, List<VisualElement> flashingElementList)
    {


        if (originalElementList.Count <= 0 || flashingElementList.Count <= 0)
            return;
        Vector2 o_pos;

        for (int i = 0; i < originalElementList.Count; i++)
        {

            o_pos = originalElementList[i].parent.parent.LocalToWorld(originalElementList[i].transform.position);

            for (int j = 0; j < flashingElementList.Count; j++)
            {
                flashingElementList[j].transform.position = originalElementList[j].LocalToWorld(o_pos);
            }


        }

    }
    //register call backs
    private void RegisterCallbacks()
    {
        m_NextButton.clicked += OnNextButtonClicked;
        m_SkipButton.clicked += OnSkipButtonClicked;
        m_PreviousButton.clicked += OnPrevButtonClicked;
        m_QuickTourButton.clicked += QuickTourButtonClicked;
    }


    private void QuickTourButtonClicked()
    {
        SaveTutorialPrefs(0);
        // SceneManager.LoadSceneAsync(m_DCTestScene_tutorial);

    }

    //proceed to next popup window
    private void OnNextButtonClicked()
    {
        if (m_popupIndex < PopupScriptableObjects.Length - 1)
        {
            m_popupIndex++;
            UpdateQuickTourAnimations(m_popupIndex);
        }

    }

    private void StageTween(List<VisualElement> elements)
    {

        if (elements.Count == 1)
        {
            GrowTween = DOTween.To(()
                    => elements[0].transform.scale,
                    x => elements[0].transform.scale = x,
                    tweenScaleUp, tweenScaleUpSpeed)
                    .SetEase(GrowEase);

            ShrinkTween = DOTween.To(()
                            => elements[0].transform.scale,
                            x => elements[0].transform.scale = x,
                            new Vector3(1f, 1f, 1f), tweenScaleUpSpeed)
                            .SetEase(ShrinkEase);
        }
        else if (elements.Count == 2)
        {
            GrowTween = DOTween.To(()
                    => elements[0].transform.scale,
                    x => elements[0].transform.scale = x,
                    tweenScaleUp, tweenScaleUpSpeed)
                    .SetEase(GrowEase);

            ShrinkTween = DOTween.To(()
                            => elements[0].transform.scale,
                            x => elements[0].transform.scale = x,
                            new Vector3(1f, 1f, 1f), tweenScaleUpSpeed)
                            .SetEase(ShrinkEase);

            GrowTween2 = DOTween.To(()
                               => elements[1].transform.scale,
                               x => elements[1].transform.scale = x,
                               tweenScaleUp, tweenScaleUpSpeed)
                               .SetEase(GrowEase);

            ShrinkTween2 = DOTween.To(()
                              => elements[1].transform.scale,
                              x => elements[1].transform.scale = x,
                              new Vector3(1f, 1f, 1f), tweenScaleUpSpeed)
                              .SetEase(ShrinkEase);

        }
        else if (elements.Count == 3)
        {

            GrowTween = DOTween.To(()
                    => elements[0].transform.scale,
                    x => elements[0].transform.scale = x,
                    tweenScaleUp, tweenScaleUpSpeed)
                    .SetEase(GrowEase);

            ShrinkTween = DOTween.To(()
                            => elements[0].transform.scale,
                            x => elements[0].transform.scale = x,
                            new Vector3(1f, 1f, 1f), tweenScaleUpSpeed)
                            .SetEase(ShrinkEase);

            GrowTween2 = DOTween.To(()
                               => elements[1].transform.scale,
                               x => elements[1].transform.scale = x,
                               tweenScaleUp, tweenScaleUpSpeed)
                               .SetEase(GrowEase);

            ShrinkTween2 = DOTween.To(()
                              => elements[1].transform.scale,
                              x => elements[1].transform.scale = x,
                              new Vector3(1f, 1f, 1f), tweenScaleUpSpeed)
                              .SetEase(ShrinkEase);

            GrowTween3 = DOTween.To(()
                              => elements[2].transform.scale,
                              x => elements[2].transform.scale = x,
                              tweenScaleUp, tweenScaleUpSpeed)
                              .SetEase(GrowEase);

            ShrinkTween3 = DOTween.To(()
                              => elements[2].transform.scale,
                              x => elements[2].transform.scale = x,
                              new Vector3(1f, 1f, 1f), tweenScaleUpSpeed)
                              .SetEase(ShrinkEase);
        }
    }

    private void UpdateQuickTourAnimations(int popUpIndex)
    {
        DOTween.KillAll();

        foreach (var ele in ElementsToAnimate)
        {
            ele.transform.scale = new Vector3(1, 1, 1);
            ele.style.display = DisplayStyle.None;
        }
        for (int i = 0; i < OriginalElementsToAnimateCopy.Count; i++)
        {
            OriginalElementsToAnimateCopy[i].style.opacity = 1;
        }
        ElementsToAnimate.Clear();
        OriginalElementsToAnimateCopy.Clear();

        if (popUpIndex < PopupScriptableObjects.Length - 1)
        {
            //change 'element to animate' data to Tween within AnimateUI() (Coroutine)
            switch (popUpIndex)
            {


                case 1:
                    ElementsToAnimate.Add(m_FillButton_flashing);
                    OriginalElementsToAnimateCopy.Add(m_FillButton);
                    // ElementsToAnimateArr[0, 0] = m_FillButton_flashing;
                    break;
                case 2:
                    ElementsToAnimate.Add(m_PauseButton_flashing);
                    OriginalElementsToAnimateCopy.Add(m_PauseButton);
                    // ElementsToAnimateArr[0, 0] = m_PauseButton_flashing;
                    break;
                case 3:
                    ElementsToAnimate.Add(m_PlayButton_flashing);
                    OriginalElementsToAnimateCopy.Add(m_PlayButton);
                    // ElementsToAnimateArr[0, 0] = m_FillButton_flashing;
                    break;
                case 4:
                    ElementsToAnimate.Add(m_ResetButton_flashing);
                    OriginalElementsToAnimateCopy.Add(m_ResetButton);
                    // ElementsToAnimateArr[0, 0] = m_ResetButton_flashing;
                    break;
                case 5:
                    ElementsToAnimate.Add(m_MenuButton_flashing);
                    OriginalElementsToAnimateCopy.Add(m_MenuButton);
                    // ElementsToAnimateArr[0, 0] = m_MenuButton_flashing;
                    break;
                case 6:
                    ElementsToAnimate.Add(m_SupplyPanel_flashing);
                    ElementsToAnimate.Add(m_Zone2_flashing);
                    ElementsToAnimate.Add(m_Zone3_flashing);
                    OriginalElementsToAnimateCopy.Add(m_SupplyPanel);
                    OriginalElementsToAnimateCopy.Add(m_PressureZone2Panel);
                    OriginalElementsToAnimateCopy.Add(m_PressureZone3Panel);
                    // ElementsToAnimateArr[0, 0] = m_MenuButton_flashing;
                    break;
                case 7:
                    ElementsToAnimate.Add(m_SupplyPanel_flashing);
                    OriginalElementsToAnimateCopy.Add(m_SupplyPanel);

                    // ElementsToAnimateArr[0, 0] = m_FillButton_flashing;
                    break;
                case 8:
                    ElementsToAnimate.Add(m_Zone2_flashing);
                    ElementsToAnimate.Add(m_Zone3_flashing);
                    OriginalElementsToAnimateCopy.Add(m_PressureZone2Panel);
                    OriginalElementsToAnimateCopy.Add(m_PressureZone3Panel);
                    // ElementsToAnimateArr[0, 0] = m_Zone2_flashing;
                    // ElementsToAnimateArr[1, 0] = m_Zone3_flashing;
                    // ElementsToAnimateArr[0, 1] = m_PressureZone2Panel;
                    // ElementsToAnimateArr[1, 1] = m_PressureZone3Panel;

                    break;
                case 9:
                    ElementsToAnimate.Clear();
                    OriginalElementsToAnimateCopy.Clear();
                    break;

            }

            //re-position flashing element to match origianl element position
            PositionFlashingElement(OriginalElementsToAnimateCopy, ElementsToAnimate);

            //hide origianl element
            for (int i = 0; i < OriginalElementsToAnimateCopy.Count; i++)
            {
                OriginalElementsToAnimateCopy[i].style.opacity = 0;
            }

            //display element to animate
            if (ElementsToAnimate.Count > 0)
            {
                foreach (var ele in ElementsToAnimate)
                {

                    ele.style.display = DisplayStyle.Flex;
                }

                //set up tweens in case there are more than one
                StageTween(ElementsToAnimate);

                //animate flashing element    
                StartCoroutine(AnimateUi());
            }
        }
    }
    //go back to previous popup window
    private void OnPrevButtonClicked()
    {
        if (m_popupIndex >= 1)
        {

            m_popupIndex--;
            UpdateQuickTourAnimations(m_popupIndex);

        }
    }

    //skip and close popup window
    private void OnSkipButtonClicked()
    {
        //save prefs to device to not show tutorial on scene open (unless opened through options)

        SaveTutorialPrefs(1);
        DOTween.KillAll();
        if (SceneManager.GetActiveScene().name == "RPZPlayScene")
        {
            SceneManager.LoadSceneAsync("RPZPlayScene");
        }
        else if (SceneManager.GetActiveScene().name == m_DCPlayScene)
        {
            SceneManager.LoadSceneAsync(m_DCPlayScene);
        }

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
        m_QuickTourButton.clicked -= QuickTourButtonClicked;
    }

    //update popp window style and text (text is taken from scriptable objects array)
    private void UpdatePopup()
    {
        TutorialSystem.Show(PopupScriptableObjects[m_popupIndex].content, PopupScriptableObjects[m_popupIndex].header);
        if (m_popupIndex == 0)
        {

            m_PopupContent.style.display = DisplayStyle.None;
            m_NextButton.AddToClassList(StartQuickTourButtonString);
            m_NextButton.text = "Start A Quick Tour";
            m_ButtonContainer.style.flexDirection = FlexDirection.ColumnReverse;
            m_PreviousButton.style.display = DisplayStyle.None;

        }
        else if (m_popupIndex == PopupScriptableObjects.Length - 1)
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
        if (m_popupIndex == PopupScriptableObjects.Length - 1)
        {

            m_NextButton.style.display = DisplayStyle.None;
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
            m_QuickTourContainer.style.display = DisplayStyle.None;


        }
    }


    private IEnumerator AnimateUi()
    {

        Sequence mySequence;
        Sequence mySequence2;
        Sequence mySequence3;
        if (ElementsToAnimate.Count == 1)
        {

            mySequence = DOTween.Sequence();
            mySequence.Append(GrowTween).Append(ShrinkTween).SetEase(Ease.Linear).SetLoops(-1);

            yield return mySequence.WaitForKill();

        }
        else if (ElementsToAnimate.Count == 2)
        {

            mySequence = DOTween.Sequence();
            mySequence2 = DOTween.Sequence();
            mySequence.Append(GrowTween).Append(ShrinkTween).SetEase(Ease.Linear).SetLoops(-1);
            mySequence2.Append(GrowTween2).Append(ShrinkTween2).SetEase(Ease.Linear).SetLoops(-1);

            yield return mySequence.WaitForKill();

        }
        else if (ElementsToAnimate.Count == 3)
        {

            mySequence = DOTween.Sequence();
            mySequence2 = DOTween.Sequence();
            mySequence3 = DOTween.Sequence();
            mySequence.Append(GrowTween).Append(ShrinkTween).SetEase(Ease.Linear).SetLoops(-1);
            mySequence2.Append(GrowTween2).Append(ShrinkTween2).SetEase(Ease.Linear).SetLoops(-1);
            mySequence3.Append(GrowTween3).Append(ShrinkTween3).SetEase(Ease.Linear).SetLoops(-1);

            yield return mySequence.WaitForKill();

        }




    }



    void Update()
    {
        UpdatePopup();


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode()]
public class TutorialPopup : MonoBehaviour
{


    private const string PopupPanelString = "TutorialPopup_panel";
    private const string PopupHeaderString = "TutorialPopup_header";
    private const string PopupBodyString = "TutorialPopup_body";
    private const string PopupHeaderLabelString = "TutorialPopup_header_label";
    private const string PopupBodyLabelString = "TutorialPopup_body_label";

    public VisualElement m_Popup_panel;
    public Label m_Popup_header_label;
    public Label m_Popup_body_label;
    public GameObject m_GameUi;
    private UIDocument root;

    void Start()
    {
        root = m_GameUi.GetComponent<UIDocument>();
        m_Popup_panel = root.rootVisualElement.Q<VisualElement>(PopupPanelString);
        m_Popup_header_label = root.rootVisualElement.Q<Label>(PopupHeaderLabelString);
        m_Popup_body_label = root.rootVisualElement.Q<Label>(PopupBodyLabelString);
    }
    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            m_Popup_panel.style.display = DisplayStyle.None;
        }
        else
        {
            m_Popup_panel.style.display = DisplayStyle.Flex;

        }

        m_Popup_header_label.text = header;
        m_Popup_body_label.text = content;


    }


    // Update is called once per frame
    void Update()
    {

    }
}

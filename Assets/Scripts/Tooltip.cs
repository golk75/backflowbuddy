using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        contentField.text = content;
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            int headerCharacterLength = headerField.text.Length;
            int contentCharacterLength = headerField.text.Length;
            // layoutElement.enabled = (headerCharacterLength > characterWrapLimit || contentCharacterLength > characterWrapLimit) ? true : false;
            layoutElement.enabled = headerField.preferredWidth > layoutElement.preferredWidth || contentField.preferredWidth > layoutElement.preferredWidth;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
#if UNITY_EDITOR

#endif

#if UNITY_IOS

#endif

#if UNITY_STANDALONE_OSX
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public RectTransform rectTransform;


    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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



    void Update()
    {
        if (Application.isEditor)
        {
            int headerCharacterLength = headerField.text.Length;
            int contentCharacterLength = headerField.text.Length;
            // layoutElement.enabled = (headerCharacterLength > characterWrapLimit || contentCharacterLength > characterWrapLimit) ? true : false;
            layoutElement.enabled = headerField.preferredWidth > layoutElement.preferredWidth || contentField.preferredWidth > layoutElement.preferredWidth;
        }

        Vector2 mousePos = Input.mousePosition;
        transform.position = mousePos;

        float pivotX = mousePos.x / Screen.width;
        float pivotY = mousePos.y / Screen.height;
        // rectTransform.pivot = new Vector2(pivotX - 0.25f, pivotY);
        rectTransform.pivot = new Vector2(pivotX - 0.29f, pivotY);
    }


#endif

#if UNITY_STANDALONE_WIN
 public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public RectTransform rectTransform;


    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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



    void Update()
    {
        if (Application.isEditor)
        {
            int headerCharacterLength = headerField.text.Length;
            int contentCharacterLength = headerField.text.Length;
            // layoutElement.enabled = (headerCharacterLength > characterWrapLimit || contentCharacterLength > characterWrapLimit) ? true : false;
            layoutElement.enabled = headerField.preferredWidth > layoutElement.preferredWidth || contentField.preferredWidth > layoutElement.preferredWidth;
        }

        Vector2 mousePos = Input.mousePosition;
        transform.position = mousePos;

        float pivotX = mousePos.x / Screen.width;
        float pivotY = mousePos.y / Screen.height;
        // rectTransform.pivot = new Vector2(pivotX - 0.25f, pivotY);
        rectTransform.pivot = new Vector2(pivotX - 0.29f, pivotY);
    }

#endif
}

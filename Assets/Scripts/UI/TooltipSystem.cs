
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
#if UNITY_STANDALONE_WIN
    private static TooltipSystem current;
    public Tooltip tooltip;
    [SerializeField]

    void Awake()
    {
        current = this;
    }
    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
        // Debug.Log($"Hiding");
    }
    public static void Show(string content, string header)
    {

        current.tooltip.SetText(content, header);
        current.tooltip.gameObject.SetActive(true);
        // Debug.Log($"Showing");

    }
#endif
#if UNITY_STANDALONE_OSX
    private static TooltipSystem current;
    public Tooltip tooltip;
    [SerializeField]

    void Awake()
    {
        current = this;
    }
    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
        // Debug.Log($"Hiding");
    }
    public static void Show(string content, string header)
    {

        current.tooltip.SetText(content, header);
        current.tooltip.gameObject.SetActive(true);
        // Debug.Log($"Showing");

    }
#endif
}

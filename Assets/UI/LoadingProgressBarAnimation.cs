using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadingProgressBarAnimation : MonoBehaviour
{
    private VisualElement m_Root;
    private VisualElement m_LoadingProgressBar;
    private Label m_LoadingPercentageText;
    private ProgressBar m_ProgressBar;


    void Start()
    {
        //Grab a reference to the root element that is drawn
        m_Root = GetComponent<UIDocument>().rootVisualElement;

        //Search the root for the two dynamic elements that need to be animated
        m_ProgressBar = m_Root.Q<ProgressBar>();
        m_LoadingProgressBar = m_Root.Q<VisualElement>("bar_Progress");
        m_LoadingPercentageText = m_Root.Q<Label>("txt_Percentage");




    }
    private void AnimateLoadingBar()
    {

        // //Grab the final width of the progress bar based on the parent and
        // //remove 25px to account for margins
        // float endWidth = m_LoadingProgressBar.parent.worldBound.width - 25;
        // DOTween.To(() => 5, x => m_LoadingPercentageText.text = $"{x}%",
        //     100, 5f).SetEase(Ease.Linear);

        // DOTween.To(() => m_LoadingProgressBar.worldBound.width, x =>
        //     m_LoadingProgressBar.style.width = x, endWidth, 5f).SetEase(Ease.Linear);
        var barWidth = m_LoadingProgressBar.resolvedStyle.width;
        var progBar = m_Root.Q(className: "unity-progress-bar__progress");

        Debug.Log($"progBar = {progBar}");
        // if (barWidth < 323)
        // {
        //     m_LoadingProgressBar.style.width = m_LoadingProgressBar.resolvedStyle.width + 1;
        // }
        progBar.style.width = Length.Percent(100);

    }

    private void Update()
    {
        // Debug.Log($"{m_LoadingProgressBar.resolvedStyle.width * 0.1f}");
        AnimateLoadingBar();
    }
}

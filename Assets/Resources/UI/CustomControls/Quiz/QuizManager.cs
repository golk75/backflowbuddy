using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class QuizManager : VisualElement
{
    bool isShowingResults;
    VisualElement m_ReviewResults;
    VisualTreeAsset m_QuestionListEntryTemplate;
    List<QuizResult> m_ResultsList;
    ListView m_QuestionList;
    VisualElement listEntry;
    Label resultPercent;



    public new class UxmlFactory : UxmlFactory<QuizManager, UxmlTraits> { }

    public QuizManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);

    }


    void OnGeometryChange(GeometryChangedEvent evt)
    {

        SetVisualElements();





        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void SetVisualElements()
    {
        m_ReviewResults = this.Q("ReviewResults");
    }



    // VisualElement MakeEntry()
    // {
    //     return m_resultsListEntry.CloneTree();
    // }
    // void BindEntry(VisualElement item, int index)
    // {
    //     (item.userData as ResultsListEntryController).SetResultsData(m_ResultsList[index]);
    // }

    // private void DisplayQuizResults(List<QuizResult> quizResults, VisualTreeAsset asset, int score)
    // {

    //     {
    //         m_ResultsList = quizResults;
    //         m_QuestionList = this.Q<ListView>("question-list");
    //         m_QuestionListEntryTemplate = Resources.Load<VisualTreeAsset>(uxmlPath);

    //         foreach (var question in m_ResultsList)
    //         {
    //             // m_ReviewResults.Q("main-container").Add(m_QuestionListEntryTemplate.CloneTree());
    //             m_QuestionList.makeItem = () =>
    //         {
    //             // Instantiate the UXML template for the entry
    //             var newListEntry = m_QuestionListEntryTemplate.Instantiate();

    //             // Instantiate a controller for the data
    //             var newListEntryLogic = new ResultsListEntryController();

    //             // Assign the controller script to the visual element
    //             newListEntry.userData = newListEntryLogic;

    //             // Initialize the controller script
    //             newListEntryLogic.SetVisualElement(newListEntry);

    //             // Return the root of the instantiated visual tree
    //             return newListEntry;
    //         };
    //             m_QuestionList.bindItem = (item, index) =>
    //                {
    //                    (item.userData as ResultsListEntryController).SetResultsData(m_ResultsList[index]);
    //                };

    //             // Set a fixed item height
    //             m_QuestionList.fixedItemHeight = 100;

    //             // Set the actual item's source list/array
    //             m_QuestionList.itemsSource = m_ResultsList;
    //         }





    //     }
    //     Debug.Log(score);
    // }
}
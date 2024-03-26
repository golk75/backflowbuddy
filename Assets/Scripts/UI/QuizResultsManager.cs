using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class QuizResultsManager : MonoBehaviour
{

    public QuestionGenerator questionGenerator;
    private List<QuizResult> m_ResultsList;
    private ListView m_QuestionList;
    public UIDocument root;
    private Label resultPercent;
    private VisualTreeAsset m_QuestionListEntryTemplate;
    string uxmlPath = "UI/uxml/Quiz/ResultsListEntry";
    private VisualElement m_ReviewResults;

    void Start()
    {
        SetVisualElements();



    }
    void OnEnable()
    {
        Actions.EndOfQuizQuestions += ShowResults;
    }
    void OnDisable()
    {
        Actions.EndOfQuizQuestions -= ShowResults;
    }
    private void SetVisualElements()
    {
        m_QuestionList = root.rootVisualElement.Q<ListView>("question-list");
        m_ReviewResults = root.rootVisualElement.Q<VisualElement>("ReviewResults");
        resultPercent = m_ReviewResults.Q<Label>("percent");
    }
    private void ShowResults(List<QuizResult> quizResults, VisualTreeAsset visualTreeAsset, float sum)
    {

        {
            m_ResultsList = quizResults;


            resultPercent.text = sum.ToString();
            m_QuestionListEntryTemplate = Resources.Load<VisualTreeAsset>(uxmlPath);

            foreach (var question in m_ResultsList)
            {
                // m_ReviewResults.Q("main-container").Add(m_QuestionListEntryTemplate.CloneTree());
                m_QuestionList.makeItem = () =>
            {
                // Instantiate the UXML template for the entry
                var newListEntry = m_QuestionListEntryTemplate.Instantiate();

                // Instantiate a controller for the data
                var newListEntryLogic = gameObject.AddComponent<ResultsListEntryController>();

                // Assign the controller script to the visual element
                newListEntry.userData = newListEntryLogic;

                // Initialize the controller script
                newListEntryLogic.SetVisualElement(newListEntry);

                // Return the root of the instantiated visual tree
                return newListEntry;
            };
                m_QuestionList.bindItem = (item, index) =>
                   {
                       (item.userData as ResultsListEntryController).SetResultsData(m_ResultsList[index]);
                   };

                // Set a fixed item height
                m_QuestionList.fixedItemHeight = 100;

                // Set the actual item's source list/array
                m_QuestionList.itemsSource = m_ResultsList;
            }





        }

        m_QuestionList.Q<VisualElement>("unity-tracker").style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/UI/Textures/gradientbar.png"));
        m_ReviewResults.style.display = DisplayStyle.Flex;
    }
}



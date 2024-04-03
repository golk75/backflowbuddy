using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class QuizResultsManager : MonoBehaviour
{

    private List<QuizResult> m_ResultsList;
    private ListView m_QuestionList;
    public UIDocument root;
    private Label resultPercent;
    private VisualTreeAsset m_QuestionListEntryTemplate;
    string uxmlPath = "UI/uxml/Quiz/ResultsListEntry";
    private VisualElement m_ReviewResults;
    private VisualElement m_ScrollBarTracker;
    private VisualElement m_ScrollBarDragger;
    private Button m_RetryButton;
    private Button m_MenuReturnButton;
    Coroutine OnScrollMove;
    void Start()
    {
        SetVisualElements();
        m_RetryButton.clicked += ReloadQuiz;
        m_MenuReturnButton.clicked += ReturnToMainMenu;


    }
    void OnEnable()
    {
        Actions.EndOfQuizQuestions += ShowResults;
        Actions.returnScrollHandle += ScrollReturn;


    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ReloadQuiz()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDisable()
    {
        Actions.EndOfQuizQuestions -= ShowResults;
        Actions.returnScrollHandle -= ScrollReturn;
    }
    private void ScrollReturn(int index)
    {
        //need to have delay for scroll to move since the visual element is registered to a GeometryChangeEvent
        OnScrollMove = StartCoroutine(ScrollDelay(index));
    }

    private IEnumerator ScrollDelay(int index)
    {
        yield return new WaitForSeconds(0.01f);
        m_QuestionList.ScrollToItem(index);

    }


    private void SetVisualElements()
    {
        m_QuestionList = root.rootVisualElement.Q<ListView>("question-list");
        m_ReviewResults = root.rootVisualElement.Q<VisualElement>("ReviewResults");
        resultPercent = m_ReviewResults.Q<Label>("percent");
        m_ScrollBarTracker = m_QuestionList.Q<VisualElement>("unity-tracker");
        m_ScrollBarDragger = m_QuestionList.Q<VisualElement>("unity-dragger");
        m_RetryButton = m_ReviewResults.Q<Button>("retry-quiz");
        m_MenuReturnButton = m_ReviewResults.Q<Button>("return-home");
    }
    public void ShowResults(List<QuizResult> quizResults, VisualTreeAsset visualTreeAsset, float sum)
    {

        {
            m_ResultsList = quizResults;


            resultPercent.text = sum.ToString() + "%";
            m_QuestionListEntryTemplate = Resources.Load<VisualTreeAsset>(uxmlPath);

            foreach (var question in m_ResultsList)
            {
                // m_ReviewResults.Q("main-container").Add(m_QuestionListEntryTemplate.CloneTree());

                m_QuestionList.makeItem = () =>
            {
                // Instantiate the UXML template for the entry
                var newListEntry = m_QuestionListEntryTemplate.Instantiate();

                // Instantiate a controller for the data
                var newListEntryLogic = new ResultsListEntryController();

                // Assign the controller script to the visual element
                newListEntry.userData = newListEntryLogic;

                // Initialize the controller script
                newListEntryLogic.SetVisualElement(newListEntry);

                newListEntry.name = "new-list-entry";




                // Return the root of the instantiated visual tree
                return newListEntry;
            };
                m_QuestionList.bindItem = (item, index) =>
                   {
                       if (m_ResultsList[index].isCorrect)
                       {
                           item.Q<VisualElement>("list-entry").style.backgroundColor = new Color(0, 0.735849f, 0.05642469f);

                           //-->this on is close!!->>
                           //item.style.backgroundColor = Color.green;

                           //    item.AddToClassList(".result-list-entry__correct");
                           //    item.RemoveFromClassList(".result-list-entry__incorrect");
                       }
                       else
                       {
                           item.Q<VisualElement>("list-entry").style.backgroundColor = new Color(0.7735849f, 0.2411385f, 0.1131185f);
                           //-->this on is close!!->>
                           //item.style.backgroundColor = Color.red;

                           //    item.AddToClassList(".result-list-entry__incorrect");
                           //    item.RemoveFromClassList(".result-list-entry__correct");
                       }
                       (item.userData as ResultsListEntryController).SetResultsData(m_ResultsList[index]);


                   };

                // if (question.isCorrect)
                // {

                //     item.AddToClassList(".result-list-entry__correct");
                //     item.RemoveFromClassList(".result-list-entry__incorrect");
                // }
                // else
                // {
                //     item.AddToClassList(".result-list-entry__incorrect");
                //     item.RemoveFromClassList(".result-list-entry__correct");
                // }

                // Set a fixed item height
                if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
                {
                    m_QuestionList.fixedItemHeight = 100;

                }
                else
                {
                    m_QuestionList.fixedItemHeight = 250;
                }


                // Set the actual item's source list/array
                m_QuestionList.itemsSource = m_ResultsList;
            }





        }

        // m_ScrollBarTracker.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/UI/Textures/gradientbar.png"));
        m_QuestionList.Q("unity-low-button").style.display = DisplayStyle.None;
        m_QuestionList.Q("unity-high-button").style.display = DisplayStyle.None;
        m_ScrollBarDragger.AddToClassList("scroll-dragger");
        m_ReviewResults.style.display = DisplayStyle.Flex;
    }
}



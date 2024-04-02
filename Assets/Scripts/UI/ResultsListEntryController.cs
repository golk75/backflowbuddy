using UnityEngine;
using UnityEngine.UIElements;

public class ResultsListEntryController
{
    Label m_ResultQuestionLabel;
    VisualElement m_ResultListEntry;

    //This function retrieves a reference to the 
    //character name label inside the UI element.

    public void SetVisualElement(VisualElement visualElement)
    {
        m_ResultQuestionLabel = visualElement.Q<Label>("question");
        m_ResultListEntry = visualElement.Q<VisualElement>("list-entry");
    }

    //This function receives the character whose name this list 
    //element is supposed to display. Since the elements list 
    //in a `ListView` are pooled and reused, it's necessary to 
    //have a `Set` function to change which character's data to display.

    public void SetResultsData(QuizResult quizResult)
    {


        m_ResultQuestionLabel.text = quizResult.question;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI[] checkListItems;
    public TextMeshProUGUI scoreText;

    public TimerManager timerManager;
    public Button resetButton;

    public string[] correctSequences;
    private int currentStep;
    private int maxStep;
    private int score;

    void Start()
    {
        maxStep = checkListItems.Length - 1;
        resetButton.onClick.AddListener(ResetScore);
        ResetScore();
    }

    void UpdateScoreText(){
        scoreText.text = "Score : " +score;
    }

     void Update()
    {
        // Detect mouse click
        if (Input.GetMouseButtonDown(0) && currentStep <= maxStep)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the clicked object has the tag "Clickable"
                if (hit.transform.CompareTag("Clickable"))
                {
                    // Get the clicked object's name
                    string clickedObjectName = hit.transform.name;

                    // Check if the object clicked matches the current step in the sequence
                    if (clickedObjectName == correctSequences[currentStep])
                    {
                        // Correct object clicked
                        CorrectClick();
                    }
                    else
                    {
                        // Wrong object clicked, deduct score
                        WrongClick();
                    }
                }
            }
        }
    }

    // Handles correct click behavior
    void CorrectClick()
    {
        // Mark the corresponding checklist item as completed
        checkListItems[currentStep].text = "✓ " + correctSequences[currentStep];

        // Increase score
        score += 10;
        UpdateScoreText();

        // Move to the next step in the sequence
        currentStep++;

        // Check if all items are completed
        if (currentStep >= correctSequences.Length)
        {
            Debug.Log("All items completed!");
            timerManager.StopTimer();
        }
    }

    // Handles wrong click behavior
    void WrongClick()
    {
        // Deduct points for the wrong click
        score = score - 5 <= 0 ? 0 : score-5;
        UpdateScoreText();

        Debug.Log("Wrong item clicked!");
    }

    void ResetScore(){
        score = 0;
        currentStep = 0;
        scoreText.text = "Score : "+score;
        foreach (var item in checkListItems)
        {  
           item.text = "...."; 
        }
        timerManager.ResetTimer();
    }
}

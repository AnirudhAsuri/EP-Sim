using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructionsText;
    private PlayerHealth playerHealth;

    private List<string> tutorialMessages = new List<string>();
    private float timeSinceMessage;

    public int currentTextPointer;
    [SerializeField] private Vector3 firstMoveToSpot;
    [SerializeField] private Vector3 secondMoveToSpot;
    [SerializeField] private Vector3 thirdMoveToSpot;
    [SerializeField] private Vector3 fourthMoveToSpot;

    [SerializeField] private GameObject moveToSpot;
    [SerializeField] private GameObject dummyEnemy;
    [SerializeField] private GameObject averageStudentEnemy;

    private void Start()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();

        HandleTutorialMessagesInitialization();
    }

    private void Update()
    {
        HandleTextTriggers();
    }

    public void HandleTutorialMessagesInitialization()
    {
        tutorialMessages.Add("Welcome to Ponchon"); //0
        tutorialMessages.Add("MOVE\nW\nA    S    D\nGo touch the red sphere"); //1
        tutorialMessages.Add("Well Done\nHold Shift while moving to increase acceleration"); //2
        tutorialMessages.Add("Hold Shift while moving to increase acceleration\nGo touch the three red spheres"); //3
        tutorialMessages.Add("Hold Shift while moving to increase acceleration\n1/3 Spheres touched"); //4
        tutorialMessages.Add("Hold Shift while moving to increase acceleration\n2/3 Spheres touched"); //5
        tutorialMessages.Add("Hold Shift while moving to increase acceleration\n3/3 Spheres touched"); //6
        tutorialMessages.Add("Well Done\nNext: Left Click to Left Punch\nRight Click to Right Punch"); //7
        tutorialMessages.Add("Attack the Dummy Enemy\nHigher your speed, greater your damage"); //8
        tutorialMessages.Add("Good Job\nYour Health is Displayed in the upper right corner"); //9
        tutorialMessages.Add("If you lose some health, you can recover it by killing enemies"); //10
        tutorialMessages.Add("Kill the dummy to regain some health"); //11
        tutorialMessages.Add("Great Job!\nYou are back to full health"); //12
        tutorialMessages.Add("Now go fight the real enemy"); //13
        tutorialMessages.Add(""); //14

        currentTextPointer = 0;
        ChangeInstructionText(currentTextPointer);
    }

    public void ChangeInstructionText(int currentPointer)
    {
        instructionsText.text = tutorialMessages[currentPointer];
    }

    public void HandleTextTriggers()
    {
        timeSinceMessage += Time.deltaTime;

        if(currentTextPointer == 0 && timeSinceMessage >= 3f)
        {
            IncrementCurrentTextPointer();
            GameObject newMoveToSpot = Instantiate(moveToSpot, firstMoveToSpot, transform.rotation);
        }

        if(currentTextPointer == 2 && timeSinceMessage >= 3f)
        {
            IncrementCurrentTextPointer();
            Vector3[] spots = { secondMoveToSpot, thirdMoveToSpot, fourthMoveToSpot };
            foreach(Vector3 spot in spots)
            {
                Instantiate(moveToSpot, spot, transform.rotation);
            }
        }

        if(currentTextPointer == 6 && timeSinceMessage >= 3f)
        {
            IncrementCurrentTextPointer();
        }

        if(currentTextPointer == 7 && timeSinceMessage >= 3f)
        {
            IncrementCurrentTextPointer();
            GameObject newDummyEnemy = Instantiate(dummyEnemy, firstMoveToSpot, transform.rotation);
        }

        if(currentTextPointer == 9 && timeSinceMessage>=3f)
        {
            IncrementCurrentTextPointer();
            playerHealth.currentHealth -= 20;
        }

        if(currentTextPointer == 10)
        {
            IncrementCurrentTextPointer();
        }

        if(currentTextPointer == 12 && timeSinceMessage >= 3f)
        {
            IncrementCurrentTextPointer();
        }

        if(currentTextPointer == 13 && timeSinceMessage >= 3f)
        {
            IncrementCurrentTextPointer();
            GameObject newAvgEnemy = Instantiate(averageStudentEnemy, firstMoveToSpot, transform.rotation);
        }
    }

    public void IncrementCurrentTextPointer()
    {
        currentTextPointer++;
        ChangeInstructionText(currentTextPointer);
        timeSinceMessage = 0;
    }
}
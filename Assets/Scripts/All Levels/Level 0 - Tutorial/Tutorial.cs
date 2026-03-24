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
        tutorialMessages.Add("MOVE\nUse the Joystick\nGo touch the red sphere"); //1
        tutorialMessages.Add("Well Done\nToggle the Sprint button while moving to increase acceleration"); //2
        tutorialMessages.Add("Toggle Sprint On while moving to increase acceleration\nGo touch the three red spheres"); //3
        tutorialMessages.Add("Toggle Sprint On while moving to increase acceleration\n1/3 Spheres touched"); //4
        tutorialMessages.Add("Toggle Sprint On while moving to increase acceleration\n2/3 Spheres touched"); //5
        tutorialMessages.Add("Toggle Sprint On while moving to increase acceleration\n3/3 Spheres touched"); //6
        tutorialMessages.Add("Well Done\nNext: Left Attack to Left Punch\nRight Attack to Right Punch"); //7
        tutorialMessages.Add("Attack the Dummy Enemy\nHigher your speed, greater your damage"); //8
        tutorialMessages.Add("Good Job\nYour Health is Displayed in the upper right corner"); //9
        tutorialMessages.Add("If you lose some health, you can recover it by killing enemies"); //10
        tutorialMessages.Add("Kill the dummy to regain some health"); //11
        tutorialMessages.Add("Great Job!\nYou are back to full health"); //12
        tutorialMessages.Add("Now go fight the real enemy\nEnemies can run only for a while before they get tired and can't attack you anymore"); //13
        tutorialMessages.Add("Enemies can run only for a while before they get tired and can't attack you anymore"); //14
        tutorialMessages.Add("Amazing job!! You have finished the tutorial!! \nYou may now exit the tutorial using the Pause Menu at the top left corner"); //15

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
            Instantiate(moveToSpot, firstMoveToSpot, transform.rotation);
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
            Instantiate(dummyEnemy, firstMoveToSpot, transform.rotation);
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
            Instantiate(averageStudentEnemy, firstMoveToSpot, transform.rotation);
        }

        if(currentTextPointer == 14 && FindAnyObjectByType<EnemyHealth>() == null)
        {
            IncrementCurrentTextPointer();
        }
    }

    public void IncrementCurrentTextPointer()
    {
        currentTextPointer++;
        ChangeInstructionText(currentTextPointer);
        timeSinceMessage = 0;
    }
}
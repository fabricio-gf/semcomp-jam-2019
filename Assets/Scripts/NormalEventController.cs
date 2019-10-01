using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class NormalEventController : MonoBehaviour
{
    Animator animator;

    public TextBox questionBox;
    public TextBox resolutionBox;
    //public TextBox resolutionBox2;

    GameEvent gameEvent;

    public float eventStartDelay;

    public TextMeshProUGUI[] player1Texts;
    public TextMeshProUGUI[] player2Texts;

    public Player player1;
    public Player player2;

    bool currentEvent = false;

    Dialogue resolutionDialogue1;
    Dialogue resolutionDialogue2;

    string firstName;
    string secondName;

    public bool p1ConfirmKeyPressed = false;
    public bool p2ConfirmKeyPressed = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        GameFlow.Instance.OnEventStart += StartEvent;
        EventHandler.Instance.OnEventResolved += ShowResolution;
        InputManager.instance.OnPressConfirm += ConfirmKeyDown;
        //InputManager.instance.OnReleaseConfirm += ConfirmKeyUp;
        questionBox.OnDialogueEnded += ShowButtons;
        resolutionBox.OnDialogueEnded += EndEvent;
        //resolutionBox2.OnDialogueEnded += EndEvent;
    }

    private void Update()
    {
        
        if (questionBox.duringDialogue)
        {
            if (p1ConfirmKeyPressed || p2ConfirmKeyPressed)
            {
                if (questionBox.isTyping)
                {
                    p1ConfirmKeyPressed = false;
                    p2ConfirmKeyPressed = false;
                    questionBox.SkipDialogue();
                }
                else
                {
                    p1ConfirmKeyPressed = false;
                    p2ConfirmKeyPressed = false;
                    questionBox.DisplayNextSentence();
                }
            }
        }
        if (resolutionBox.duringDialogue)
        {
            if (resolutionBox.isTyping)
            {
                if(p1ConfirmKeyPressed || p2ConfirmKeyPressed)
                {
                    p1ConfirmKeyPressed = false;
                    p2ConfirmKeyPressed = false;
                    resolutionBox.SkipDialogue();
                }
            }
            else
            {
                if(p1ConfirmKeyPressed && p2ConfirmKeyPressed)
                {
                    p1ConfirmKeyPressed = false;
                    p2ConfirmKeyPressed = false;
                    resolutionBox.DisplayNextSentence();
                }
            }
        }

        if(resolutionBox.duringDialogue && !resolutionBox.isTyping)
        {
            if (p1ConfirmKeyPressed && p2ConfirmKeyPressed)
            {
                p1ConfirmKeyPressed = false;
                p2ConfirmKeyPressed = false;

                EndEvent();
            }
        }
    }

    public void StartEvent(GameEvent _gameEvent)
    {
        gameEvent = _gameEvent;
        if(gameEvent.type == GameEvent.EventType.RIVAL)
        {
            return;
        }

        currentEvent = true;
        StartCoroutine(EventStartDelay());
    }

    IEnumerator EventStartDelay()
    {
        yield return new WaitForSeconds(eventStartDelay);
        WritePlayerOptions();
        animator.SetTrigger("BeginNormalEvent");
    }

    public void StartDialogue()
    {
        questionBox.StartDialogue(gameEvent.question);

        ToggleCanPressConfirm(true);
    }

    public void ShowButtons()
    {
        StartCoroutine(ToggleCanPressWithDelay(0.25f));
        animator.SetTrigger("ShowButtons");
    }

    public void ShowResolution(List<GameEvent.PlayerAnswer> playerAnswer)
    {
        if (!currentEvent)
        {
            return;
        }

        ToggleCanPress(false);
        questionBox.ClearTextBox();

        if (gameEvent.type == GameEvent.EventType.PROACTIVE)
        {
            EndEvent();
            return;
        }
        else if(gameEvent.type == GameEvent.EventType.REACTIVE)
        {
            resolutionDialogue1 = playerAnswer[0].answer.resolution;
            firstName = playerAnswer[0].player.playername;

            resolutionDialogue2 = playerAnswer[1].answer.resolution;
            secondName = playerAnswer[1].player.playername;

        }
        animator.SetTrigger("ShowResolution");
    }

    public void StartResolutionDialogue()
    {
        for(int i = 0; i < resolutionDialogue1.sentences.Length; i++)
        {
            StringBuilder sb = new StringBuilder(resolutionDialogue1.sentences[i]);
            sb.Replace("<Player>", firstName);
            resolutionDialogue1.sentences[i] = sb.ToString();

        }
        for (int i = 0; i < resolutionDialogue2.sentences.Length; i++)
        {
            StringBuilder sb = new StringBuilder(resolutionDialogue2.sentences[i]);
            sb.Replace("<Player>", secondName);
            resolutionDialogue2.sentences[i] = sb.ToString();
        }

        List<string> tempList = new List<string>();
        tempList.AddRange(resolutionDialogue1.sentences);
        tempList.AddRange(resolutionDialogue2.sentences);
        Dialogue finalResolutionDialogue = new Dialogue(tempList.ToArray());

        resolutionBox.StartDialogue(finalResolutionDialogue);

        ToggleCanPressConfirm(true);
    }

    public void EndEvent()
    {
        ToggleCanPressConfirm(false);

        currentEvent = false;
        animator.SetTrigger("EndNormalEvent");

        Debug.Log("Normal Event ended.");
    }

    public void EventEnded()
    {
        resolutionBox.ClearTextBox();
        //resolutionBox2.ClearTextBox();
        GameFlow.Instance.FinishEvent();
    }

    void WritePlayerOptions()
    {
        for(int i = 0; i < player1Texts.Length; i++)
        {
            /*print("Player 1 text at " + i + " :" + player1.avaiableAnswers[i].answerText);
            print("Player 1 resolution at " + i + " :" + player1.avaiableAnswers[i].resolution.sentences[0]);
            print("Player 2 text at " + i + " :" + player2.avaiableAnswers[i].answerText);
            print("Player 2 resolution at " + i + " :" + player2.avaiableAnswers[i].resolution.sentences[0]);*/

            player1Texts[i].text = player1.avaiableAnswers[i].answerText;
            player2Texts[i].text = player2.avaiableAnswers[i].answerText;
        }
    }

    public void ToggleCanPress(bool toggle)
    {
        InputManager.instance.canPress = toggle;
    }

    public void ToggleCanPress()
    {
        InputManager.instance.canPress = !InputManager.instance.canPress;
    }

    IEnumerator ToggleCanPressWithDelay(float delay)
    {
        InputManager.instance.canPress = false;
        yield return new WaitForSeconds(delay);
        InputManager.instance.canPress = true;
    }

    public void ToggleCanPressConfirm(bool toggle)
    {
        p1ConfirmKeyPressed = false;
        p2ConfirmKeyPressed = false;
        InputManager.instance.canPressConfirm = toggle;
    }

    public void ToggleCanPressConfirm()
    {
        p1ConfirmKeyPressed = false;
        p2ConfirmKeyPressed = false;
        InputManager.instance.canPressConfirm = !InputManager.instance.canPressConfirm;
    }

    public void ConfirmKeyDown(int player)
    {
        switch (player)
        {
            case 1:
                p1ConfirmKeyPressed = true;
                break;
            case 2:
                p2ConfirmKeyPressed = true;
                break;
            default:
                break;
        }
    }

    public void ConfirmKeyUp(int player)
    {
        switch (player)
        {
            case 1:
                p1ConfirmKeyPressed = false;
                break;
            case 2:
                p2ConfirmKeyPressed = false;
                break;
            default:
                break;
        }
    }
}

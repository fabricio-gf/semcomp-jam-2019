using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NormalEventController : MonoBehaviour
{
    Animator animator;

    public TextBox questionBox;
    public TextBox resolutionBox;
    public TextBox resolutionBox2;

    GameEvent gameEvent;

    public float eventStartDelay;

    public TextMeshProUGUI[] player1Texts;
    public TextMeshProUGUI[] player2Texts;

    public Player player1;
    public Player player2;

    bool currentEvent = false;

    Dialogue resolutionDialogue1;
    Dialogue resolutionDialogue2;

    public bool p1ConfirmKeyPressed = false;
    public bool p2ConfirmKeyPressed = false;

    int endEventCounter = 0;
    int counterLimit = 2;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        GameFlow.Instance.OnEventStart += StartEvent;
        EventHandler.Instance.OnEventResolved += ShowResolution;
        InputManager.instance.OnPressConfirm += ConfirmKeyDown;
        InputManager.instance.OnReleaseConfirm += ConfirmKeyUp;
        questionBox.OnDialogueEnded += ShowButtons;
        resolutionBox.OnDialogueEnded += EndEvent;
        resolutionBox2.OnDialogueEnded += EndEvent;
    }

    private void Update()
    {
        if (questionBox.duringDialogue)
        {
            if (p1ConfirmKeyPressed || p2ConfirmKeyPressed)
            {
                print("entrou 1");
                if (questionBox.isTyping)
                {
                    p1ConfirmKeyPressed = false;
                    p2ConfirmKeyPressed = false;
                    questionBox.SkipDialogue();
                }
                else
                {
                    questionBox.DisplayNextSentence();
                }
            }
        }
        if (resolutionBox.duringDialogue)
        {
            if (p1ConfirmKeyPressed)
            {
                print("entrou 2");
                if (resolutionBox.isTyping)
                {

                    p1ConfirmKeyPressed = false;
                    resolutionBox.SkipDialogue();
                }
                else
                {
                    resolutionBox.DisplayNextSentence();
                }
            }
        }
        if (resolutionBox2.duringDialogue)
        {
            if (p2ConfirmKeyPressed)
            {
                print("entrou 3");
                if (resolutionBox2.isTyping)
                {
                    p2ConfirmKeyPressed = false;
                    resolutionBox2.SkipDialogue();
                }
                else
                {
                    resolutionBox2.DisplayNextSentence();
                }
            }
        }

        if(resolutionBox.duringDialogue && resolutionBox2.duringDialogue && !resolutionBox.isTyping && !resolutionBox2.isTyping)
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
        ToggleCanPress(true);
    }

    public void ShowButtons()
    {
        animator.SetTrigger("ShowButtons");
    }

    public void ShowResolution(List<GameEvent.PlayerAnswer> playerAnswer)
    {
        if (!currentEvent)
        {
            return;
        }

        questionBox.ClearTextBox();

        if (gameEvent.type == GameEvent.EventType.PROACTIVE)
        {
            EndEvent();
            return;
        }
        else if(gameEvent.type == GameEvent.EventType.REACTIVE)
        {
            resolutionDialogue1 = playerAnswer[0].answer.resolution;
            resolutionDialogue2 = playerAnswer[1].answer.resolution;
        }
        animator.SetTrigger("ShowResolution");
    }

    public void StartResolutionDialogue()
    {
        resolutionBox.StartDialogue(resolutionDialogue1);
        resolutionBox2.StartDialogue(resolutionDialogue2);
        ToggleCanPress(true);
    }

    public void EndEvent()
    {
        endEventCounter++;
        if (endEventCounter >= counterLimit)
        {
            endEventCounter = 0;

            currentEvent = false;
            animator.SetTrigger("EndNormalEvent");

            Debug.Log("Normal Event ended.");
        }
    }

    public void EventEnded()
    {
        resolutionBox.ClearTextBox();
        resolutionBox2.ClearTextBox();
        GameFlow.Instance.OnEventFinished();
    }

    void WritePlayerOptions()
    {
        for(int i = 0; i < player1Texts.Length; i++)
        {
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

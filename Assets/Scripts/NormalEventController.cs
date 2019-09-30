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

    private void Awake()
    {
        animator = GetComponent<Animator>();

        GameFlow.Instance.OnEventStart += StartEvent;
        EventHandler.Instance.OnEventResolved += ShowResolution;
        InputManager.instance.OnPressConfirm += ReadInput;
        questionBox.OnDialogueEnded += ShowButtons;
    }

    private void Update()
    {
        if (questionBox.duringDialogue)
        {
            if(p1ConfirmKeyPressed || p2ConfirmKeyPressed)
            {
                p1ConfirmKeyPressed = false;
                p2ConfirmKeyPressed = false;
                questionBox.SkipDialogue();
            }
        }
        if (resolutionBox.duringDialogue)
        {
            if (p1ConfirmKeyPressed)
            {
                p1ConfirmKeyPressed = false;
                resolutionBox.SkipDialogue();
            }
        }
        if (resolutionBox2.duringDialogue)
        {
            if (p1ConfirmKeyPressed)
            {
                p1ConfirmKeyPressed = false;
                resolutionBox2.SkipDialogue();
                StartCoroutine(ToggleCanPressWithDelay(0.5f));
            }
        }

        if (p1ConfirmKeyPressed && p2ConfirmKeyPressed)
        {
            print("Confirmed");
            p1ConfirmKeyPressed = false;
            p2ConfirmKeyPressed = false;
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
        if(gameEvent.type == GameEvent.EventType.PROACTIVE)
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
    }

    public void EndEvent()
    {
        if (!currentEvent) return;
        currentEvent = false;
        animator.SetTrigger("EndNormalEvent");
        //GameFlow.Instance.OnEventFinished();
        Debug.Log("Normal Event ended.");
    }

    public void EventEnded()
    {
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

    public void ReadInput(int player)
    {
        switch (player)
        {
            case 1:
                p1ConfirmKeyPressed = true;
                break;
            case 2:
                p1ConfirmKeyPressed = true;
                break;
            default:
                break;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class RivalEventController : MonoBehaviour
{
    Animator animator;

    public TextBox questionBox;
    public TextBox resolutionBox;

    GameEvent gameEvent;

    public float eventStartDelay;

    public TextMeshProUGUI[] optionTexts;

    public Player player1;
    public Player player2;

    bool currentEvent = false;

    Dialogue resolutionDialogue;

    public bool p1ConfirmKeyPressed = false;
    public bool p2ConfirmKeyPressed = false;

    string playerWinName;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameFlow.Instance.OnEventStart += StartEvent;
        EventHandler.Instance.OnEventResolved += ShowResolution;
        InputManager.instance.OnPressConfirm += ConfirmKeyDown;
        questionBox.OnDialogueEnded += StartCountdown;
        resolutionBox.OnDialogueEnded += EndEvent;
    }

    private void Update()
    {
        if (questionBox.duringDialogue)
        {
            if (p1ConfirmKeyPressed || p2ConfirmKeyPressed)
            {
                /*if (questionBox.isTyping)
                {
                    p1ConfirmKeyPressed = false;
                    p2ConfirmKeyPressed = false;
                    questionBox.SkipDialogue();
                }
                else*/
                if (!questionBox.isTyping)
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
                if (p1ConfirmKeyPressed || p2ConfirmKeyPressed)
                {
                    p1ConfirmKeyPressed = false;
                    p2ConfirmKeyPressed = false;
                    resolutionBox.SkipDialogue();
                }
            }
            else
            {
                if (p1ConfirmKeyPressed && p2ConfirmKeyPressed)
                {
                    p1ConfirmKeyPressed = false;
                    p2ConfirmKeyPressed = false;
                    resolutionBox.DisplayNextSentence();
                }
            }
        }

        if (resolutionBox.duringDialogue && !resolutionBox.isTyping)
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
        if (gameEvent.type != GameEvent.EventType.RIVAL)
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
        animator.SetTrigger("StartRivalEvent");
    }

    public void StartDialogue()
    {
        questionBox.StartDialogue(gameEvent.question);
    }

    public void StartCountdown()
    {
        animator.SetTrigger("StartCountdown");
    }

    public void ShowResolution(List<GameEvent.PlayerAnswer> playerAnswer)
    {
        if (!currentEvent)
        {
            return;
        }

        ToggleCanPress(false);
        questionBox.ClearTextBox();

        playerWinName = playerAnswer[0].player.playername;

        resolutionDialogue = playerAnswer[0].answer.resolution;
        animator.SetTrigger("ShowResolution");
    }

    public void StartResolutionDialogue()
    {
        StringBuilder sb;

        for (int i = 0; i < resolutionDialogue.sentences.Length; i++)
        {
            sb = new StringBuilder(resolutionDialogue.sentences[i]);
            if(playerWinName == "Charles")
            {
                sb.Replace("<PlayerWin>", "Charles");
                sb.Replace("<PlayerLose>", "Katrina");
            }
            else if(playerWinName == "Katrina")
            {
                sb.Replace("<PlayerWin>", "Katrina");
                sb.Replace("<PlayerLose>", "Charles");
            }
            
            resolutionDialogue.sentences[i] = sb.ToString();
        }
        
        resolutionBox.StartDialogue(resolutionDialogue);

        ToggleCanPressConfirm(true);
    }

    public void EndEvent()
    {
        ToggleCanPressConfirm(false);

        currentEvent = false;
        animator.SetTrigger("EndRivalEvent");
        //GameFlow.Instance.FinishEvent();

        Debug.Log("Rival Event ended.");

    }

    public void EventEnded()
    {
        resolutionBox.ClearTextBox();
        //resolutionBox2.ClearTextBox();
        GameFlow.Instance.FinishEvent();
    }

    void WritePlayerOptions()
    {
        for (int i = 0; i < 2; i++)
        {
            optionTexts[i].text = player1.availableAnswers[i].answerText;
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
                if (resolutionBox.duringDialogue && !resolutionBox.isTyping)
                {
                    resolutionBox.ChangeSprite(0, 1);
                }
                break;
            case 2:
                p2ConfirmKeyPressed = true;
                if (resolutionBox.duringDialogue && !resolutionBox.isTyping)
                {
                    resolutionBox.ChangeSprite(1, 1);
                }
                break;
            default:
                break;
        }
    }

    public void EndCountdown()
    {
        ToggleCanPress(true);
    }
}

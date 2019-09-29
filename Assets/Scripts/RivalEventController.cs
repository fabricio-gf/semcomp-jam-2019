using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    private void Start()
    {
        GameFlow.Instance.OnEventStart += StartEvent;
        EventHandler.Instance.OnEventResolved += ShowResolution;

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

    public void ShowResolution()
    {
        if (!currentEvent)
        {
            return;
        }
        animator.SetTrigger("ShowResolution");
    }

    public void StartResolutionDialogue()
    {
        //resolutionBox.StartDialogue();
    }

    public void EndEvent()
    {
        currentEvent = false;
        animator.SetTrigger("EndRivalEvent");
        GameFlow.Instance.OnEventFinished();
    }

    void WritePlayerOptions()
    {
        for(int i = 0; i < 2; i++)
        {
            optionTexts[i].text = player1.avaiableAnswers[i].answerText;
        }
    }
}

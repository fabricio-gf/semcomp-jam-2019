﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NormalEventController : MonoBehaviour
{
    Animator animator;

    public TextBox questionBox;
    public TextBox resolutionBox;

    GameEvent gameEvent;

    public float eventStartDelay;

    public TextMeshProUGUI[] player1Texts;
    public TextMeshProUGUI[] player2Texts;

    public Player player1;
    public Player player2;

    bool currentEvent = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        GameFlow.Instance.OnEventStart += StartEvent;
        EventHandler.Instance.OnEventResolved += ShowResolution;
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
    }

    public void ShowButtons()
    {
        animator.SetTrigger("ShowButtons");
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
        animator.SetTrigger("EndNormalEvent");
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
}

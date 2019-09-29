using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public float eventTimeoutSeconds = 3;
    public float startEventDelaySeconds = 0.5f;
    public int maxTurns = 24;
    public float conquestWinThreshold = 0.9f;

    public List<GameEvent> avaiableEvents;


    public delegate void EventStart(GameEvent gameEvent);
    public event EventStart OnEventStart;

    public delegate void EventForceEnd();
    public event EventForceEnd OnEventForceEnd;

    static public GameFlow Instance { get; private set; }

    public int Turn { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartEvent();
    }

    private IEnumerator StartEventCoroutine(float startEventDelay)
    {
        yield return new WaitForSeconds(startEventDelay);
        //Choose event from list, considering probabilities
        int rand = -1;
        GameEvent gameEvent = null;
        while (gameEvent == null)
        {
            rand = Random.Range(0, avaiableEvents.Count);
            gameEvent = avaiableEvents[rand];
        }
        avaiableEvents.Remove(gameEvent);

        StartCoroutine(EventTimeout(eventTimeoutSeconds));
        Debug.Log("Starting Event: " + gameEvent.identifier);
        OnEventStart?.Invoke(gameEvent);
    }

    private void StartEvent()
    {
        Turn++;
        StartCoroutine(StartEventCoroutine(eventTimeoutSeconds));
    }



    public void OnEventFinished()
    {
        // Subscribe this to an event
        if (Turn < maxTurns)
        {
            StartEvent();
        }
        else
        {
            //Finish game
            Debug.Log("Game is finished");
        }

    }

    IEnumerator EventTimeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnEventForceEnd?.Invoke();
    }


}

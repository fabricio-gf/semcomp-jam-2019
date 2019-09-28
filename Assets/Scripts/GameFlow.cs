using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFLow : MonoBehaviour
{
    public float eventTimeoutSeconds = 3;
    public float startEventDelaySeconds = 0.5f;
    public int maxTurns = 24;


    public delegate void EventStart(GameEvent gameEvent);
    public event EventStart OnEventStart;

    public delegate void EventForceEnd();
    public event EventForceEnd OnEventForceEnd;

    static public GameFLow Instance { get; private set; }

    public int turn { get; private set; } = 0;

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
        turn++;
        StartEvent();
    }


    private IEnumerator StartEventCoroutine(float startEventDelay)
    {
        yield return new WaitForSeconds(startEventDelay);
        //Choose event from list, considering probabilities
        GameEvent gameEvent = new GameEvent(); //FIXME
        StartCoroutine(EventTimeout(eventTimeoutSeconds));
        OnEventStart?.Invoke(gameEvent);
    }

    private void StartEvent()
    {
        StartCoroutine(StartEventCoroutine(eventTimeoutSeconds));
    }

    public void OnEventResolved()
    {
        // Subscribe this to an event
        StartEvent();
    }

    IEnumerator EventTimeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnEventForceEnd?.Invoke();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{

    private delegate void EventResolved();
    private event EventResolved OnEventResolved;

    public static EventHandler Instance { get; private set; }


    private GameEvent activeEvent;

    private List<GameEvent.PlayerAnswer> answers = new List<GameEvent.PlayerAnswer>(2);

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
        GameFLow.Instance.OnEventStart += OnEventStart;
        GameFLow.Instance.OnEventForceEnd += OnEventForceEnd;
        //OnEventResolved += GameFLow.Instance.OnEventResolved; //FIXME: some UI should handle this instead of the gameflow
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEventStart(GameEvent gameEvent)
    {
        activeEvent = gameEvent;
        answers.Clear();
    }

    void OnEventForceEnd()
    {
        if(activeEvent.type == GameEvent.EventType.RIVAL)
        {
            return;
        }
        // Else, makes resolution based on the taken inputs (answers)
        Resolve();
    }

    public void RecordAnswer(Answer answer, Player player)
    {
        GameEvent.PlayerAnswer a = new GameEvent.PlayerAnswer(answer, player);
        answers.Add(a);
        /* Rival Events: first answer takes effect, event is resolved
         * Other events: waits for two answers, applies results, is resolved
         */
        if (activeEvent.type == GameEvent.EventType.RIVAL)
        {
            Resolve();
        }
        else
        {
            if (answers.Count == 2) //TODO: for expansion, check against playercount
            {
                Resolve();
            }
        }
    }

    private void Resolve()
    {
        //Resolve effects on players and world - should use GameEvent
        activeEvent.Resolve(answers);
        OnEventResolved();
    }

    
}

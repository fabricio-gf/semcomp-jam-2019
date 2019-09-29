using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{

    public delegate void EventResolved(List<GameEvent.PlayerAnswer> answers);
    public event EventResolved OnEventResolved;

    public static EventHandler Instance { get; private set; }


    public GameEvent ActiveEvent { get; private set; }

    public Player player1;
    public Player player2;

    private List<GameEvent.PlayerAnswer> _answers = new List<GameEvent.PlayerAnswer>();

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
    // Start is called before the first frame update
    void Start()
    {
        GameFlow.Instance.OnEventStart += OnEventStart;
        GameFlow.Instance.OnEventForceEnd += OnEventForceEnd;
        //OnEventResolved += GameFLow.Instance.OnEventResolved; //FIXME: some UI should handle this instead of the gameflow
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEventStart(GameEvent gameEvent) // UI needs it
    {
        ActiveEvent = gameEvent;
        _answers.Clear();
        SetPlayersAvaiableAnswers();
    }

    void SetPlayersAvaiableAnswers()
    {
        List<Answer> answers = new List<Answer>(ActiveEvent.answers);
        player1.avaiableAnswers = new List<Answer>();
        while(player1.avaiableAnswers.Count < 4 && answers.Count > 0)
        {
            int rand = Random.Range(0, answers.Count);
            player1.avaiableAnswers.Add(answers[rand]);
            answers.RemoveAt(rand);
        }
        answers = new List<Answer>(ActiveEvent.answers);
        player2.avaiableAnswers = new List<Answer>();
        while (player2.avaiableAnswers.Count < 4 && answers.Count > 0)
        {
            int rand = Random.Range(0, answers.Count);
            player2.avaiableAnswers.Add(answers[rand]);
            answers.RemoveAt(rand);
        }
    }

    void OnEventForceEnd() // UI needs it
    {
        if(ActiveEvent.type == GameEvent.EventType.RIVAL)
        {
            return;
        }
        // Else, makes resolution based on the taken inputs (answers)
        Resolve();
    }

    public void RecordAnswer(Answer answer, Player player) 
    {
        //Make additional checks to see if it is possible to record the answer
        // Avoid multiple inputs
        foreach(GameEvent.PlayerAnswer ans in _answers)
        {
            if (ans.player == player)
            {
                return;
            }
        }
        GameEvent.PlayerAnswer a = new GameEvent.PlayerAnswer(answer, player);
        _answers.Add(a);
        /* Rival Events: first answer takes effect, event is resolved
         * Other events: waits for two answers, applies results, is resolved
         */
        if (ActiveEvent.type == GameEvent.EventType.RIVAL)
        {
            Resolve();
        }
        else
        {
            if (_answers.Count == 2) //TODO: for expansion, check against playercount
            {
                Resolve();
            }
        }
    }

    private void Resolve()
    {
        //Resolve effects on players and world - should use GameEvent
        OnEventResolved?.Invoke(ActiveEvent.Resolve(_answers)); // Warning: resolve does things AND returns more stuff
    }

    
}

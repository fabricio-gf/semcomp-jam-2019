using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public float eventTimeoutSeconds = 3;
    public float startEventDelaySeconds = 0.5f;
    public int maxTurns = 24;
    public float conquestWinThreshold = 0.9f;

    public GameConfig gameConfig;

    public List<GameEvent> avaiableEvents = new List<GameEvent>();

    public delegate void EventStart(GameEvent gameEvent);
    public event EventStart OnEventStart;

    public delegate void EventForceEnd();
    public event EventForceEnd OnEventForceEnd;

    public delegate void EventEnd();
    public event EventForceEnd OnEventEnd;

    public delegate void GameOver(Player player);
    public event GameOver OnGameOver;

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

    private void OnEnable()
    {
        foreach (GameEvent e in gameConfig.avaiableEvents)
        {
            avaiableEvents.Clear();
            avaiableEvents.Add (Instantiate(e));
        }
    }

    private IEnumerator StartEventCoroutine(float startEventDelay)
    {
        yield return new WaitForSeconds(startEventDelay);
        List<Player> players = new List<Player>(FindObjectsOfType<Player>());
        // START Conquest Victory
        // If player can have conquest victory
        //Player attackingPlayer = null;
        //float attackingPlayerPower = 0f;
        //float attackingPlayerPowerIndex = 0;
        //foreach(Player p in players)
        //{
        //    int highestIndex = (int)p.Power.Highest();
        //    float highestValue = p.Power.groups[highestIndex];
        //    if (highestValue >= conquestWinThreshold)
        //    {
        //        if (attackingPlayerPower < highestValue)
        //        {
        //            attackingPlayer = p;
        //            attackingPlayerPower = highestValue;
        //            attackingPlayerPowerIndex = highestIndex;
        //        }
        //    }
        //}
        //if (attackingPlayer != null)
        //{
        //    //Run crisis event to finish
        //}
        //// END Conquest Victory
        //Choose event from list, considering probabilities

        if (avaiableEvents.Count > 0)
        {
            int rand = -1;
            GameEvent gameEvent = null;
            while (gameEvent == null)
            {
                rand = Random.Range(0, avaiableEvents.Count);
                if (avaiableEvents[rand] != null)
                {
                    gameEvent = avaiableEvents[rand];
                }
            }
            avaiableEvents.Remove(gameEvent);

            StartCoroutine(EventTimeout(eventTimeoutSeconds));

            OnEventStart?.Invoke(gameEvent);
        }
    }

    public void StartEvent()
    {
        Turn++;
        StartCoroutine(StartEventCoroutine(startEventDelaySeconds));
    }

    public void FinishEvent()
    {
        Player.NormalizeInfluences(); //FIXME - fora de lugar
        // Subscribe this to an event
        if (Turn < maxTurns)
        {
            StartEvent();
        }
        else
        {
            //Finish game
            List<Player> players = new List<Player>(FindObjectsOfType<Player>());
            // Supremacy victory - turns count expired
            float power1 = players[0].Power.Total();
            float power2 = players[1].Power.Total();
            if (power1 > power2)
            {
                OnGameOver?.Invoke(players[0]);
            }
            else
            {
                OnGameOver?.Invoke(players[1]);
            }
            Debug.Log("Game is finished");
        }
        OnEventEnd?.Invoke();
    }

    IEnumerator EventTimeout(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnEventForceEnd?.Invoke();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playername;

    private static List<Player> players = null;
    public List<Answer> avaiableAnswers; // UI needs it

    public World.PopulationGroups influence;
    public World.PopulationGroups Power { get => World.Instance.groups * influence; }

    public World.PopulationGroups EventStartInfluence { set; get; }



    public static void NormalizeInfluences()
    {
        if (players == null)
        {
            players = new List<Player>(FindObjectsOfType<Player>());
        }

        World.PopulationGroups globalInfluence = new World.PopulationGroups();
        foreach (Player p in players)
        {
            globalInfluence += p.influence; //TODO: review
        }
        foreach (Player p in players)
        {
            p.influence /= globalInfluence;
        }
    }

      

    // Player attributes, infuence, etc

    // Start is called before the first frame update
    void Start()
    {
        GameFlow.Instance.OnEventStart += RecordEventStartInfluence;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PressUP()
    {
        SendAnswer(2);
    }

    public void PressDown()
    {
        SendAnswer(3);
    }

    public void PressLeft()
    {
        SendAnswer(0);
    }

    public void PressRight()
    {
        SendAnswer(1);
    }

    public void SendAnswer(int index)
    {
        if (index >= avaiableAnswers.Count)
        {
            return;
        }
        EventHandler.Instance.RecordAnswer(avaiableAnswers[index], this);
    }

    private void RecordEventStartInfluence(GameEvent e)
    {
        EventStartInfluence = influence;
        Debug.Log("Player " + playername + "'s influence: " + influence );
    }

}

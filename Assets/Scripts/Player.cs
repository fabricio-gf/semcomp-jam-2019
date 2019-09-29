using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static List<Player> players = new List<Player>();
    public List<Answer> avaiableAnswers; // UI needs it

    public static void NormalizeInfluences()
    {
        World.PopulationGroups globalInfluence = new World.PopulationGroups();
        foreach (Player p in players)
        {
            globalInfluence = p.influence*100 + globalInfluence; //TODO: set a proper parameterized multiplier for sum before adding
        }
        foreach (Player p in players)
        {
            p.influence /= globalInfluence;
        }
    }

    public World.PopulationGroups influence;
    

    public World.PopulationGroups Power { get => World.Instance.groups * influence; }

    // Player attributes, infuence, etc

    // Start is called before the first frame update
    void Start()
    {

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

}

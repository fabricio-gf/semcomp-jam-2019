using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static List<Player> players = new List<Player>();

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
    

    public World.PopulationGroups Power { get => World.Instance.Groups * influence; }

    // Player attributes, infuence, etc

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}

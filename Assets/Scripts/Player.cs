using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playername;

    private static List<Player> players = null;
    public List<Answer> availableAnswers; // UI needs it

    public World.PopulationGroups influence = new World.PopulationGroups(0f, 1f);
    public World.PopulationGroups Power { get => World.Instance.groups * influence; }

    public World.PopulationGroups EventStartInfluence { set; get; }

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(RandomizeAnimation());
    }

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
        Debug.Log("Player " + playername + " pressed up.");
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
        if (index >= availableAnswers.Count)
        {
            return;
        }
        EventHandler.Instance.RecordAnswer(availableAnswers[index], this);
    }

    private void RecordEventStartInfluence(GameEvent e)
    {
        EventStartInfluence = influence;
        Debug.Log("Player " + playername + "'s influence: " + influence + "; max: " + influence.MaxValue);
    }

    IEnumerator RandomizeAnimation()
    {
        yield return new WaitForSeconds(Random.Range(10f,20f));
        animator.SetTrigger("PlayAnimation");
        StartCoroutine(RandomizeAnimation());
    }

}

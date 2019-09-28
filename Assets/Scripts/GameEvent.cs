using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    public enum EventType
    {
        PROACTIVE,
        REACTIVE,
        CRISIS,
        RIVAL
    }

    public string identifier;
    public EventType type;
    public string question; //temp
    //public Dialogue question;
    public List<Answer> answers;

    public GameEvent(string _identifier, EventType _type, string _question, List<Answer> _answers)
    {
        identifier = _identifier;
        type = _type;
        question = _question;
        answers = _answers;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    public struct PlayerAnswer
    {
        public Player player;
        public Answer answer;
        public PlayerAnswer(Answer a, Player p)
        {
            player = p;
            answer = a;
        }
    }
    
    public enum EventType
    {
        PROACTIVE,
        REACTIVE,
        CRISIS,
        RIVAL
    }

    public const float STATUS_CHANGE_SCALE = 100f;


    public string identifier;
    public EventType type;
    //public string question; //temp
    public Dialogue question;

    public List<Answer> answers;

    public GameEvent(string _identifier, EventType _type, Dialogue _question, List<Answer> _answers)
    {
        identifier = _identifier;
        type = _type;
        question = _question;
        answers = _answers;
    }

    public GameEvent()
    {

    }

    public virtual List<PlayerAnswer> Resolve(List<PlayerAnswer> answers)
    {
        if (type != EventType.CRISIS)
        {
            foreach (PlayerAnswer ans in answers)
            {
                Debug.Log("Player influence max: " + ans.player.influence.MaxValue);
                ans.player.influence += Answer.AdaptedStatChanges(ans.answer.statChanges) / STATUS_CHANGE_SCALE;
                World.Instance.groups += Answer.AdaptedStatChanges(ans.answer.popChanges) / World.Instance.popMax;
                List<Player> players = new List<Player>(FindObjectsOfType<Player>());
                foreach (Player p in players)
                {
                    if (p != ans.player)
                    {
                        p.influence += Answer.AdaptedStatChanges(ans.answer.rivalStatChanges) / STATUS_CHANGE_SCALE;
                    }
                }
            }
            Player.NormalizeInfluences();
            return new List<PlayerAnswer>(answers);
        }
        else
        {
            //FIXME: implement to get resolution of the crisis
            return null;
        }
        
        
    }

    
}

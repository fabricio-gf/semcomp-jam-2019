﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Answer
{
    public int identifier;
    public string answerText;
    public List<StatChange> statChanges;
    public List<StatChange> rivalStatChanges;
    public List<StatChange> popChanges;
    public Dialogue resolution;

    public Answer(int _identifier, string _answerText, List<StatChange> _statChanges, Dialogue _resolution)
    {
        identifier = _identifier;
        answerText = _answerText;
        statChanges = _statChanges;
        rivalStatChanges = null;
        popChanges = null;
        resolution = _resolution;
    }

    public Answer(int _identifier, string _answerText, List<StatChange> _statChanges, List<StatChange> _popChanges, Dialogue _resolution)
    {
        identifier = _identifier;
        answerText = _answerText;
        statChanges = _statChanges;
        rivalStatChanges = null;
        popChanges = _popChanges;
        resolution = _resolution;
    }

    public Answer(int _identifier, string _answerText, List<StatChange> _statChanges, List<StatChange> _rivalChanges, List<StatChange> _popChanges, Dialogue _resolution)
    {
        identifier = _identifier;
        answerText = _answerText;
        statChanges = _statChanges;
        rivalStatChanges = _rivalChanges;
        popChanges = _popChanges;
        resolution = _resolution;
    }


    public World.PopulationGroups AdaptedStatChanges()
    {
        int size = (int)World.Faction.SIZE;
        List<float> values = new List<float>();
        for (int i = 0; i < size; i++)
        {
            values.Add(0f);
        }
        foreach (StatChange s in statChanges)
        {
            if (s.socialClass != Utilities.SocialClass.NULL)
            {
                values[(int)Utilities.Adapted(s.socialClass)] = s.changeValue;
            }
        }
        return new World.PopulationGroups(values);
    }

    public Answer()
    {

    }
}

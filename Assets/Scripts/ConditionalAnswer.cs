using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionalAnswer : Answer
{
    [System.Serializable]
    public struct Condition
    {
        public Utilities.SocialClass socialClass;
        public int threshold;
    }

    public Condition condition;

    public ConditionalAnswer(int _identifier, string _answerText, List<StatChange> _statChanges, List<StatChange> popChanges, Dialogue _resolution, Condition _condition)
    {
        identifier = _identifier;
        answerText = _answerText;
        statChanges = _statChanges;
        resolution = _resolution;
        condition = _condition;
    }
}

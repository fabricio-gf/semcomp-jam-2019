using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrisisEvent : GameEvent
{
    public List<ConditionalAnswer> conditionalAnswers;

    public struct CrisisResolution
    {
        public int index;
        public Dialogue resolution;
        public List<StatChange> popChanges;
    }

    public List<CrisisResolution> crisisResolutions;

    public CrisisEvent(string _identifier, EventType _type, Dialogue _question, List<ConditionalAnswer> _answers, List<CrisisResolution> _crisisResolutions)
    {
        identifier = _identifier;
        type = _type;
        question = _question;
        conditionalAnswers = _answers;
        crisisResolutions = _crisisResolutions;
    }
}

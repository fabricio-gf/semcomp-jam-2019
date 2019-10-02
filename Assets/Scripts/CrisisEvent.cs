using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrisisEvent : GameEvent
{
    public List<ConditionalAnswer> conditionalAnswers;

    [System.Serializable]
    public struct CrisisResolution
    {
        public int index;
        public Dialogue resolution;
        public List<StatChange> popChanges;
    }

    public List<CrisisResolution> crisisResolutions;

    public int[,] crisisResolutionTable;

    public CrisisEvent(string _identifier, EventType _type, Dialogue _question, List<ConditionalAnswer> _answers, List<CrisisResolution> _crisisResolutions, int[,] _crisisResolutionTable)
    {
        identifier = _identifier;
        type = _type;
        question = _question;
        conditionalAnswers = _answers;
        crisisResolutions = _crisisResolutions;
        crisisResolutionTable = _crisisResolutionTable;
    }

    public override List<PlayerAnswer> Resolve(List<PlayerAnswer> answers)
    {
        Debug.Log("index 1 - " + answers[0].answer.identifier + " index 2 - " + answers[1].answer.identifier);
        int resolutionIndex = crisisResolutionTable[answers[0].answer.identifier, answers[1].answer.identifier];
        Debug.Log("Resolution index - " + resolutionIndex);

        CrisisResolution chosenResolution = crisisResolutions[resolutionIndex];
        Debug.Log("Chosen resolution - " + chosenResolution.resolution);

        PlayerAnswer playerAnswer = new PlayerAnswer(new Answer(chosenResolution.index, null, null, chosenResolution.popChanges, chosenResolution.resolution), null);
        List<PlayerAnswer> finalAnswer = new List<PlayerAnswer>();
        finalAnswer.Add(playerAnswer);

        Debug.Log("Final answer length - " + finalAnswer.Count);
        Debug.Log("Final answer content - " + finalAnswer[0].answer.resolution);

        return new List<PlayerAnswer>(finalAnswer);
    }
}

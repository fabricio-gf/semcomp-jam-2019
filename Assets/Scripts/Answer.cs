using System.Collections.Generic;

[System.Serializable]
public class Answer
{
    public int identifier;
    public string answerText;
    public List<StatChange> statChanges;
    public string resolution;

    public Answer(int _identifier, string _answerText, List<StatChange> _statChanges, string _resolution)
    {
        identifier = _identifier;
        answerText = _answerText;
        statChanges = _statChanges;
        resolution = _resolution;
    }
}

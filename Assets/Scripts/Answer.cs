using System.Collections.Generic;

[System.Serializable]
public class Answer
{
    public int identifier;
    public string answerText;
    public List<StatChange> statChanges;
    public List<StatChange> popChanges;
    public Dialogue resolution;

    public Answer(int _identifier, string _answerText, List<StatChange> _statChanges, List<StatChange> popChanges, Dialogue _resolution)
    {
        identifier = _identifier;
        answerText = _answerText;
        statChanges = _statChanges;
        resolution = _resolution;
    }

    public World.PopulationGroups AdaptedStatChanges()
    {
        List<float> values = new List<float>((int)World.Faction.SIZE);
        foreach (StatChange s in statChanges)
        {
            values[(int)Utilities.Adapted(s.socialClass)] = s.changeValue;
        }
        return new World.PopulationGroups(values);
    }
}

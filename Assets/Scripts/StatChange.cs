using System.Collections.Generic;

[System.Serializable]
public class StatChange
{
    public Utilities.SocialClass socialClass;
    public int changeValue;

    public StatChange(Utilities.SocialClass _socialClass, int _changeValues) {
        socialClass = _socialClass;
        changeValue = _changeValues;
    }
}

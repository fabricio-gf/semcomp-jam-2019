using UnityEngine;

[CreateAssetMenu(fileName ="StoreObject", menuName = "StoreObject", order = 0)]
public class StoreInformationSO : ScriptableObject
{
    public string objectName;
    public string iconPath;
    public string description;
    public int baseCost;
    public int manaCost;

    public StoreInformationSO(string _objectName, string _iconPath, string _description, int _baseCost, int _manaCost)
    {
        objectName = _objectName;
        iconPath = _iconPath;
        description = _description;
        baseCost = _baseCost;
        manaCost = _manaCost;
    }
}

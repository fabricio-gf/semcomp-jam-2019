using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulationCountDisplay : MonoBehaviour
{
    public World.Faction faction;
    public TextMeshProUGUI text;

    

    // Start is called before the first frame update
    void Start()
    {
        text.text = PopulationToString();
        GameFlow.Instance.OnEventEnd += OnEventEnd;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEventEnd()
    {
        text.text = PopulationToString();
    }

    private string PopulationToString()
    {
        return (World.Instance.groups.GetGroupValueAt((int)faction)
            * World.Instance.popMax.GetGroupValueAt((int)faction))
            .ToString();
    }
}

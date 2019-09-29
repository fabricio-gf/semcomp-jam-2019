using UnityEngine;
using TMPro;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class CreateEvents : MonoBehaviour
{
    List<StatChange> statChanges = new List<StatChange>();
    List<StatChange> popChanges = new List<StatChange>();
    string question;

    [SerializeField] TextAsset proactiveEventQuotes = null;
    [SerializeField] TextAsset proactiveEventAnswers = null;
    List<Answer> proactiveAnswers = new List<Answer>();

    [Space(20)]
    [SerializeField] TextAsset[] reactiveEvents = null;
    List<Answer> reactiveAnswers = new List<Answer>();

    [Space(20)]
    [SerializeField] TextAsset[] rivalEvents = null;
    List<Answer> rivalAnswers = new List<Answer>();

    [Space(20)]
    [SerializeField] TextAsset[] crisisEventQuestion = null;
    [SerializeField] TextAsset[] crisisEventResultsTable = null;
    [SerializeField] TextAsset[] crisisEventResolutions = null;

    [Space(20)]

    StringBuilder sb = new StringBuilder();

    string[] lines;

    public void CreateScriptableObjects(bool deleteAll)
    {
        if (deleteAll)
        {
            AssetDatabase.DeleteAsset("Assets/Resources/Events");
            
            AssetDatabase.CreateFolder("Assets/Resources", "Events");
            AssetDatabase.Refresh();
        }
        //Debug.Log("Testing time - Start time: " + Time.time);
        //float now = Time.time;

        // PROACTIVE EVENTS

        //GenerateProactiveEvents();

        //GenerateReactiveEvents();

        GenerateRivalEvents();

        GenerateCrisisEvents();
    }

    private void GenerateProactiveEvents()
    {
        //handle csv file
        lines = proactiveEventAnswers.text.Split('\n');
        statChanges.Clear();
        proactiveAnswers.Clear();

        for(int j = 1; j < lines.Length; j++)
        {
            string[] lineContent = lines[j].Split(',');
            statChanges.Clear();
            for (int i = 1; i < lineContent.Length; i++)
            {
                if (lineContent[i] != "0")
                {
                    statChanges.Add(new StatChange((Utilities.SocialClass)(i - 1), int.Parse(lineContent[i])));
                }
            }
            proactiveAnswers.Add(new Answer(j - 1, lineContent[0], statChanges, null, new Dialogue(new string[0])));
        }

        lines = proactiveEventQuotes.text.Split('\n');
        Dialogue dialogue;
        GameEvent gameEvent;

        for(int i = 0; i < lines.Length; i++)
        {
            if (i >= lines.Length) break;
            dialogue = new Dialogue(lines[i].Split('$'));

            gameEvent = new GameEvent("ProactiveEvent" + i, GameEvent.EventType.PROACTIVE, dialogue, proactiveAnswers);
            CreateAsset(typeof(GameEvent), gameEvent);
        }
    }

    private void GenerateReactiveEvents()
    {
        int eventIndex = 1;
        Dialogue dialogue;

        //handle csv file
        foreach(TextAsset e in reactiveEvents)
        {
            //handle csv file
            lines = e.text.Split('\n');
            statChanges.Clear();
            popChanges.Clear();
            reactiveAnswers.Clear();

            dialogue = new Dialogue(lines[1].Split(',')[0].Split('$'));
            
            for(int i = 2; i < lines.Length; i++)
            {
                string[] lineContent = lines[i].Split(',');
                statChanges.Clear();
                for (int j = 1; j < (lineContent.Length-1) / 2; j++)
                {
                    if (lineContent[j] != "0" && lineContent[j] != "" && !string.IsNullOrEmpty(lineContent[j]))
                    {
                        statChanges.Add(new StatChange((Utilities.SocialClass)(j - 1), int.Parse(lineContent[j])));
                    }
                }
                popChanges.Clear();
                for (int j = (lineContent.Length-1) / 2; j < lineContent.Length-1; j++)
                {
                    if (lineContent[j] != "0" && lineContent[j] != "\r" &&  !string.IsNullOrEmpty(lineContent[j]))
                    {
                        popChanges.Add(new StatChange((Utilities.SocialClass)(j - lineContent.Length / 2), int.Parse(lineContent[j])));
                    }
                }
                reactiveAnswers.Add(new Answer(i - 1, lineContent[0], statChanges, popChanges, new Dialogue(lineContent[lineContent.Length-1].Split('$'))));
            }

            GameEvent gameEvent = new GameEvent("ReactiveEvent" + eventIndex, GameEvent.EventType.REACTIVE, dialogue, reactiveAnswers);
            eventIndex++;
            CreateAsset(typeof(GameEvent), gameEvent);
        }
    }

    private void GenerateRivalEvents()
    {
        int eventIndex = 1;
        Dialogue dialogue;
        foreach (TextAsset e in rivalEvents)
        {
            //handle csv file
            lines = e.text.Split('\n');
            statChanges.Clear();
            popChanges.Clear();

            dialogue = new Dialogue(lines[1].Split(',')[0].Split('$'));

            for(int i = 2; i < lines.Length; i++)
            {
                string[] lineContent = lines[i].Split(',');
                statChanges.Clear();
                for (int j = 1; j < (lineContent.Length-1) / 2; j++)
                {
                    if (lineContent[j] != "0" && lineContent[j] != "" && !string.IsNullOrEmpty(lineContent[j]))
                    {
                        statChanges.Add(new StatChange((Utilities.SocialClass)(j - 1), int.Parse(lineContent[j])));
                    }
                }
                popChanges.Clear();
                for (int j = (lineContent.Length-1) / 2; j < lineContent.Length-1; j++)
                {
                    if (lineContent[j] != "0" && lineContent[j] != "\r" && !string.IsNullOrEmpty(lineContent[j]))
                    {
                        popChanges.Add(new StatChange((Utilities.SocialClass)(j - lineContent.Length / 2), int.Parse(lineContent[j])));
                    }
                }
                rivalAnswers.Add(new Answer(i - 1, lineContent[0], statChanges, popChanges, new Dialogue(lineContent[lineContent.Length-1].Split('$'))));
            }

            GameEvent gameEvent = new GameEvent("RivalEvent" + eventIndex, GameEvent.EventType.RIVAL, dialogue, rivalAnswers);
            eventIndex++;
            CreateAsset(typeof(GameEvent), gameEvent);
        }
    }

    private void GenerateCrisisEvents()
    {

    }

    private static void CreateAsset(System.Type type, GameEvent _gameEvent)
    {
        var asset = ScriptableObject.CreateInstance(type);
        //string[] folder = new string[] { "Assets/Resources/Events" };
        if (AssetDatabase.FindAssets(_gameEvent.identifier).Length != 0) print(AssetDatabase.DeleteAsset("Assets/Resources/Events/" + _gameEvent.identifier + ".asset"));
        asset = _gameEvent;
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets/Resources/Events/";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + _gameEvent.identifier + ".asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    /*public void UpdateStore()
    {
        StoreInformationSO[] storeItems = Resources.LoadAll<StoreInformationSO>("StoreItems");
        //update ui
        int currentIndex = 0;
        foreach (RectTransform child in LayoutGroup)
        {
            if (currentIndex >= lines.Length) break;

            //name
            sb.Clear();
            child.Find("Name").GetComponent<TextMeshProUGUI>().text = sb.Append(storeItems[currentIndex].objectName).ToString();
            //sprite
            sb.Clear();
            sb.Append("StoreIcons/");
            sb.Append(storeItems[currentIndex].iconPath);
            print(sb.ToString());
            Sprite spr = Resources.Load<Sprite>(sb.ToString());
            child.Find("Icon").GetComponent<UnityEngine.UI.Image>().sprite = spr;
            //description
            sb.Clear();
            child.Find("Description").GetComponent<TextMeshProUGUI>().text = sb.Append(storeItems[currentIndex].description).ToString();
            //baseCost
            sb.Clear();
            //child.Find("Button").GetComponent<ButtonUpdater>().ChangeObjectBaseCost(storeItems[currentIndex].baseCost);
            child.Find("Button/CostIcon/Cost").GetComponent<TextMeshProUGUI>().text = sb.Append(storeItems[currentIndex].baseCost).ToString();
            //manaCost
            sb.Clear();
            child.Find("ManaCostIcon/ManaCost").GetComponent<TextMeshProUGUI>().text = sb.Append(storeItems[currentIndex].manaCost).ToString();

            currentIndex++;
        }

        //Debug.Log("Testing time - Final time: " + Time.time);
        //Debug.Log("Elapsed time: " + (Time.time - now));
    }*/
}

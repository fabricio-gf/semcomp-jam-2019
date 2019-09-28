using UnityEngine;
using TMPro;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class CreateEvents : MonoBehaviour
{
    [SerializeField] TextAsset proactiveEventQuotes = null;
    [SerializeField] TextAsset proactiveEventAnswers = null;
    List<Answer> proactiveAnswers = new List<Answer>();

    [Space(20)]
    [SerializeField] TextAsset[] reactiveEvent = null;

    [Space(20)]
    [SerializeField] TextAsset[] rivalEvent = null;

    [Space(20)]
    [SerializeField] TextAsset[] crisisEvent = null;

    [Space(20)]

    [SerializeField] RectTransform LayoutGroup = null;
    StringBuilder sb = new StringBuilder();

    string[] lines;

    public void CreateScriptableObjects(bool deleteAll)
    {
        if (deleteAll)
        {
            print("DELETE");
            AssetDatabase.DeleteAsset("Assets/Resources/Events");
            
            AssetDatabase.CreateFolder("Assets/Resources", "Events");
            AssetDatabase.Refresh();
        }
        //Debug.Log("Testing time - Start time: " + Time.time);
        //float now = Time.time;

        // PROACTIVE EVENTS

        GenerateProactiveEvents();
    }

    private void GenerateProactiveEvents()
    {
        //handle csv file
        lines = proactiveEventAnswers.text.Split('\n');
        List<StatChange> statChanges = new List<StatChange>();
        proactiveAnswers.Clear();

        int currentIndex = 1;
        foreach (var l in lines)
        {
            if (currentIndex >= lines.Length) break;
            string[] lineContent = lines[currentIndex].Split(',');
            statChanges.Clear();
            for (int i = 1; i < lineContent.Length; i++)
            {
                if (lineContent[i] != "0")
                {
                    statChanges.Add(new StatChange((Utilities.SocialClass)(i - 1), int.Parse(lineContent[i])));
                }
            }
            proactiveAnswers.Add(new Answer(currentIndex - 1, lineContent[0], statChanges, ""));
            currentIndex++;
        }

        lines = proactiveEventQuotes.text.Split('\n');
        GameEvent gameEvent;
        currentIndex = 1;
        foreach (var l in lines)
        {
            if (currentIndex >= lines.Length) break;

            gameEvent = new GameEvent("ProactiveEvent" + currentIndex, GameEvent.EventType.PROACTIVE, l, proactiveAnswers);
            CreateAsset(typeof(GameEvent), gameEvent);
            currentIndex++;
        }
    }

    private static void CreateAsset(System.Type type, GameEvent _gameEvent)
    {
        var asset = ScriptableObject.CreateInstance(type);
        string[] folder = new string[] { "Assets/Resources/Events" };
        if (AssetDatabase.FindAssets(_gameEvent.identifier).Length != 0) print(AssetDatabase.DeleteAsset("Assets/Resources/Events/" + _gameEvent.identifier + ".asset"));
        asset = _gameEvent;
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets/Resources/Events";
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

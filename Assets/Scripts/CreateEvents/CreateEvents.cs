using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Text;
using System.Collections.Generic;


public class CreateEvents : MonoBehaviour
{
#if UNITY_EDITOR
    List<StatChange> statChanges = new List<StatChange>();
    List<StatChange> popChanges = new List<StatChange>();
    string question;

    [SerializeField] TextAsset proactiveEventQuotes = null;
    [SerializeField] TextAsset proactiveEventAnswers = null;
    List<Answer> proactiveAnswers = new List<Answer>();

    [Space(20)]
    [SerializeField] TextAsset[] reactiveEventsAssets = null;
    List<Answer> reactiveAnswers = new List<Answer>();

    [Space(20)]
    [SerializeField] TextAsset[] rivalEventsAssets = null;
    List<Answer> rivalAnswers = new List<Answer>();

    [Space(20)]
    [SerializeField] TextAsset[] crisisQuestionsAssets = null;
    [SerializeField] TextAsset[] crisisResultsTableAssets = null;
    [SerializeField] TextAsset[] crisisResolutionsAssets = null;

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

        //GenerateProactiveEvents();

        //GenerateReactiveEvents();

        //GenerateRivalEvents();

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
            string[] lineContent = lines[j].Split(';');
            statChanges.Clear();
            for (int i = 1; i < lineContent.Length; i++)
            {
                if (lineContent[i] != "0")
                {
                    statChanges.Add(new StatChange((Utilities.SocialClass)(i - 1), int.Parse(lineContent[i])));
                }
            }
            proactiveAnswers.Add(new Answer(j - 1, lineContent[0], new List<StatChange> (statChanges), new Dialogue(new string[0])));
        }

        lines = proactiveEventQuotes.text.Split('\n');
        Dialogue dialogue;
        GameEvent gameEvent;

        for(int i = 1; i < lines.Length; i++)
        {
            dialogue = new Dialogue(lines[i].Split('$'));

            gameEvent = new GameEvent("ProactiveEvent" + i, GameEvent.EventType.PROACTIVE, dialogue, new List<Answer>(proactiveAnswers));
            CreateAsset(typeof(GameEvent), gameEvent);
        }
    }

    private void GenerateReactiveEvents()
    {
        int eventIndex = 1;
        Dialogue dialogue;

        //handle csv file
        foreach(TextAsset e in reactiveEventsAssets)
        {
            //handle csv file
            lines = e.text.Split('\n');
            statChanges.Clear();
            popChanges.Clear();
            reactiveAnswers.Clear();

            dialogue = new Dialogue(lines[1].Split(';')[0].Split('$'));

            for(int i = 2; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    print("Empty line");
                    break;
                }
                string[] lineContent = lines[i].Split(';');
                statChanges.Clear();
                for (int j = 1; j < (lineContent.Length) / 2; j++)
                {
                    if (lineContent[j] != "0" && lineContent[j] != "-" && !string.IsNullOrEmpty(lineContent[j]))
                    {
                        statChanges.Add(new StatChange((Utilities.SocialClass)(j - 1), int.Parse(lineContent[j])));
                    }
                }
                popChanges.Clear();
                for (int j = (lineContent.Length) / 2; j < lineContent.Length-1; j++)
                {
                    if (lineContent[j] != "0" && lineContent[j] != "-" &&  !string.IsNullOrEmpty(lineContent[j]))
                    {
                        popChanges.Add(new StatChange((Utilities.SocialClass)(j - lineContent.Length / 2), int.Parse(lineContent[j])));
                    }
                }
                reactiveAnswers.Add(new Answer(i - 1, lineContent[0], new List<StatChange>(statChanges), new List<StatChange>(popChanges), new Dialogue(lineContent[lineContent.Length-1].Split('$'))));
            }

            GameEvent gameEvent = new GameEvent("ReactiveEvent" + eventIndex, GameEvent.EventType.REACTIVE, dialogue, new List<Answer>(reactiveAnswers));
            eventIndex++;
            CreateAsset(typeof(GameEvent), gameEvent);
        }
    }

    private void GenerateRivalEvents()
    {
        int eventIndex = 1;

        List<StatChange> rivalChanges = new List<StatChange>();

        Dialogue dialogue;
        foreach (TextAsset e in rivalEventsAssets)
        {
            //handle csv file
            lines = e.text.Split('\n');
            rivalAnswers.Clear();
            statChanges.Clear();
            rivalChanges.Clear();
            popChanges.Clear();

            dialogue = new Dialogue(lines[1].Split(';')[0].Split('$'));

            for(int i = 2; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    print("Empty line");
                    break;
                }
                string[] lineContent = lines[i].Split(';');
                statChanges.Clear();
                for (int j = 1; j < ((lineContent.Length-1) / 3) +1; j++)
                {
                    if (lineContent[j] != "0" && lineContent[j] != "-" && !string.IsNullOrEmpty(lineContent[j]))
                    {
                        statChanges.Add(new StatChange((Utilities.SocialClass)(j - 1), int.Parse(lineContent[j])));
                    }
                }
                rivalChanges.Clear();
                for (int j = ((lineContent.Length - 1) / 3) + 1; j < (2*(lineContent.Length - 1) / 3)+1; j++)
                {
                    if (lineContent[j] != "0" && lineContent[j] != "-" && !string.IsNullOrEmpty(lineContent[j]))
                    {
                        rivalChanges.Add(new StatChange((Utilities.SocialClass)(j - 7), int.Parse(lineContent[j])));
                    }
                }
                popChanges.Clear();
                for (int j = (2 * (lineContent.Length - 1) / 3) + 1; j < lineContent.Length-1; j++)
                {
                    if (lineContent[j] != "0" && lineContent[j] != "-" && !string.IsNullOrEmpty(lineContent[j]))
                    {
                        popChanges.Add(new StatChange((Utilities.SocialClass)(j - 13), int.Parse(lineContent[j])));
                    }
                }
                rivalAnswers.Add(new Answer(i - 1, lineContent[0], new List<StatChange>(statChanges), new List<StatChange>(rivalChanges), new List<StatChange>(popChanges), new Dialogue(lineContent[lineContent.Length-1].Split('$'))));
            }

            GameEvent gameEvent = new GameEvent("RivalEvent" + eventIndex, GameEvent.EventType.RIVAL, dialogue, new List<Answer>(rivalAnswers));
            eventIndex++;
            CreateAsset(typeof(GameEvent), gameEvent);
        }
    }

    private void GenerateCrisisEvents()
    {
        //MUST CHECK LIST REFERENCES
        
        List<CrisisEvent.CrisisResolution> crisisResolutions = new List<CrisisEvent.CrisisResolution>();
        CrisisEvent.CrisisResolution tempResolution;

        ConditionalAnswer conditionalAnswer;
        List<ConditionalAnswer> crisisAnswers = new List<ConditionalAnswer>();
        Dialogue dialogue;
        popChanges.Clear();
        statChanges.Clear();


        int eventIndex = 1;
        print(crisisQuestionsAssets.Length);
        for(int i = 0; i < crisisQuestionsAssets.Length; i++){

            //resolutions
            lines = crisisResolutionsAssets[i].text.Split('\n');

            crisisResolutions.Clear();
            
            for(int j = 1; j < lines.Length; j++)
            {
                if (string.IsNullOrEmpty(lines[j]))
                {
                    //print("Empty line");
                    break;
                }

                string[] lineContent = lines[j].Split(';');
                tempResolution.resolution = new Dialogue(lineContent[lineContent.Length - 1].Split('$'));

                popChanges.Clear();
                for (int k = 1; k < lineContent.Length-1; k++)
                {
                    if(lineContent[k] != "0" && lineContent[k] != "-" && lineContent[k] != "-0" && !string.IsNullOrEmpty(lineContent[k]))
                    {
                        popChanges.Add(new StatChange((Utilities.SocialClass)(k-1), int.Parse(lineContent[k])));
                    }
                }
                tempResolution.index = int.Parse(lineContent[0]);
                tempResolution.popChanges = new List<StatChange>(popChanges);
                crisisResolutions.Add(tempResolution);
            }

            //possible answers to the question
            lines = crisisQuestionsAssets[i].text.Split('\n');

            dialogue = new Dialogue(lines[1].Split(';')[0].Split('$'));
            Utilities.SocialClass tempClass;
            ConditionalAnswer.Condition tempCondition;

            crisisAnswers.Clear();

            for (int j = 2; j < lines.Length; j++)
            {
                if (string.IsNullOrEmpty(lines[j]))
                {
                    //print("Empty line");
                    break;
                }

                string[] lineContent = lines[j].Split(';');
                
                if(lineContent[0] != "-" && lineContent[0] != "" && lineContent[0] != "-0")
                {
                    switch (lineContent[1])
                    {
                        case "Merchant":
                            tempClass = Utilities.SocialClass.MERCHANT;
                            break;
                        case "Guard":
                            tempClass = Utilities.SocialClass.GUARD;
                            break;
                        case "Commoner":
                            tempClass = Utilities.SocialClass.COMMONER;
                            break;
                        case "Noble":
                            tempClass = Utilities.SocialClass.NOBLE;
                            break;
                        case "Alchemist":
                            tempClass = Utilities.SocialClass.ALCHEMIST;
                            break;
                        case "Clergy":
                            tempClass = Utilities.SocialClass.CLERIC;
                            break;
                        default:
                            tempClass = Utilities.SocialClass.NULL;
                            break;
                    }

                    tempCondition.socialClass = tempClass;

                    if (lineContent[2][0] == '-')
                    {
                        tempCondition.threshold = 10000;
                    }
                    else
                    {
                        tempCondition.threshold = int.Parse(lineContent[2]);
                    }

                    conditionalAnswer = new ConditionalAnswer(j-1, lineContent[0], null, null, null, tempCondition);
                    crisisAnswers.Add(conditionalAnswer);
                }
            }

            //table
            lines = crisisResultsTableAssets[i].text.Split('\n');

            int[,] crisisResolutionTable = new int[lines[0].Split(';').Length-1, lines.Length-2];

            for(int j = 1; j < lines.Length; j++)
            {
                if (string.IsNullOrEmpty(lines[j]))
                {
                    //print("Empty line");
                    continue;
                }

                string[] lineContent = lines[j].Split(';');

                for(int k = 1; k < lineContent.Length; k++)
                {
                    crisisResolutionTable[j - 1, k - 1] = int.Parse(lineContent[k]);
                }
            }

            CrisisEvent gameEvent = new CrisisEvent("CrisisEvent" + eventIndex, GameEvent.EventType.CRISIS, dialogue, new List<ConditionalAnswer>(crisisAnswers), new List<CrisisEvent.CrisisResolution>(crisisResolutions), crisisResolutionTable);
            CreateAsset(typeof(CrisisEvent), gameEvent);
            eventIndex++;
        }
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
#endif
}

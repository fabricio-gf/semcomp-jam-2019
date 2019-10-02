using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public enum Faction
    {
        MERCHANTS,
        NOBILITY,
        GUARD,
        PEASANTS,
        ALCHEMISTS,
        CLERICS,
        SIZE
    }

    [System.Serializable]
    public class PopulationGroups //TODO: extract class - too big to be here
    {
        //Constraints
        public float MinValue { get; private set; } = Mathf.Infinity;
        public float MaxValue { get; private set; } = Mathf.NegativeInfinity;
        //Data
        [SerializeField]
        private List<float> groups
            = new List<float>((int)Faction.SIZE); // should use private set
        //Properties
        public float Merchants {
            get => groups[(int)Faction.MERCHANTS];
            private set => SetGroupValueAt((int)Faction.MERCHANTS, value);
        }
        public float Nobility
        {
            get => groups[(int)Faction.NOBILITY];
            private set => SetGroupValueAt((int)Faction.NOBILITY, value);
        }
        public float Guard
        {
            get => groups[(int)Faction.GUARD];
            private set => SetGroupValueAt((int)Faction.GUARD, value);
        }
        public float Peasants
        {
            get => groups[(int)Faction.PEASANTS];
            private set => SetGroupValueAt((int)Faction.PEASANTS, value);
        }
        public float Alchemists
        {
            get => groups[(int)Faction.ALCHEMISTS];
            private set => SetGroupValueAt((int)Faction.ALCHEMISTS, value);
        }
        public float Clerics
        {
            get => groups[(int)Faction.CLERICS];
            private set => SetGroupValueAt((int)Faction.CLERICS, value);
        }
        //Constructors
        public PopulationGroups(float min = Mathf.NegativeInfinity, float max = Mathf.Infinity)
        {
            Initialize();
            MinValue = min;
            MaxValue = max;
        }
        //Constructors
        public PopulationGroups(List<float> values, float min = Mathf.NegativeInfinity, float max = Mathf.Infinity)
        {
            MinValue = min;
            MaxValue = max;
            groups = new List<float>(values);
            //Initialize();
        }
        public void Initialize()
        {
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                groups.Add(0f);
            }
        }
        //Getters & Setters
        public void SetGroupValueAt(int index, float value)
        {
            if (index >= (int)Faction.SIZE)
            {
                return; // out of range
            }
            Debug.Log("Min: " + MinValue + " | Max: " + MaxValue + " | Value: " + value
                + " | Mathf.Max(MinValue, value): " + Mathf.Max(MinValue, value)
                + " | Mathf.Min(Mathf.Max(MinValue, value), MaxValue): "
                + Mathf.Min(Mathf.Max(MinValue, value), MaxValue));
            groups[index] = Mathf.Min(Mathf.Max(MinValue, value), MaxValue);
        }
        public float GetGroupValueAt(int index)
        {
            return groups[index];
        }
        //Operators
        public static PopulationGroups operator+ (PopulationGroups a, PopulationGroups b)
        {
            Debug.Log("a: " + a + " -|- a.max: " + a.MaxValue + " --||-- b: " + b + " -|- b.max: " + b.MaxValue);
            PopulationGroups p = new PopulationGroups(a.MinValue, a.MaxValue);
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                p.SetGroupValueAt(i, a.groups[i] + b.groups[i]);
            }
            return p;
        }
        public static PopulationGroups operator- (PopulationGroups a, PopulationGroups b)
        {
            PopulationGroups p = new PopulationGroups(a.MinValue, a.MaxValue);
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                p.SetGroupValueAt(i, a.groups[i] - b.groups[i]);
            }
            return p;
        }
        public static PopulationGroups operator* (PopulationGroups a, PopulationGroups b)
        {
            PopulationGroups p = new PopulationGroups(a.MinValue, a.MaxValue);
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                p.SetGroupValueAt(i, a.groups[i] * b.groups[i]);
            }
            return p;
        }
        public static PopulationGroups operator *(PopulationGroups a, float b)
        {
            PopulationGroups p = new PopulationGroups(a.MinValue, a.MaxValue);
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                p.SetGroupValueAt(i, a.groups[i] * b);
            }
            return p;
        }
        public static PopulationGroups operator /(PopulationGroups a, PopulationGroups b)
        {
            PopulationGroups p = new PopulationGroups(a.MinValue, a.MaxValue);
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                p.SetGroupValueAt(i, a.groups[i] / b.groups[i]);
            }
            return p;
        }
        public static PopulationGroups operator /(PopulationGroups a, float b)
        {
            PopulationGroups p = new PopulationGroups(a.MinValue, a.MaxValue);
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                p.SetGroupValueAt(i, a.groups[i] / b);
            }
            return p;
        }
        public Faction Highest()
        {
            float f = -1;
            int index = 0;
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                f = Mathf.Max(f, groups[i]);
                if (groups[i] > f)
                {
                    f = groups[i];
                    index = i;
                }
            }
            return (Faction)index;
        }
        public float Total()
        {
            float f = 0f;
            foreach(float val in groups)
            {
                f += val;
            }
            return f;
        }
        override public string ToString()
        {
            string s =
                "Merchants: " + Merchants + " | " +
                "Nobility: " + Nobility + " | " +
                "Guard: " + Guard + " | " +
                "Commoners: " + Peasants + " | " +
                "Alchemists: " + Alchemists + " | " +
                "Clerics: " + Clerics;
            return s;
        }
    }

    [SerializeField]
    public PopulationGroups groups = new PopulationGroups(0.1f, 1f); // should use private set

    public PopulationGroups popMax = new PopulationGroups();

    public static World Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }   
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

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

    public class PopulationGroups //TODO: extract class - too big to be here
    {
        //Constraints
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }
        //Data
        public List<float> Groups { get; private set; } 
            = new List<float>((int)Faction.SIZE);
        //Properties
        public float Merchants {
            get => Groups[(int)Faction.MERCHANTS];
            private set => Groups[(int)Faction.MERCHANTS] =
                Mathf.Min(Mathf.Max(MinValue, value), MaxValue);
        }
        public float Nobility
        {
            get => Groups[(int)Faction.NOBILITY];
            private set => Groups[(int)Faction.NOBILITY] =
                Mathf.Min(Mathf.Max(MinValue, value), MaxValue);
        }
        public float Guard
        {
            get => Groups[(int)Faction.GUARD];
            private set => Groups[(int)Faction.GUARD] =
                Mathf.Min(Mathf.Max(MinValue, value), MaxValue);
        }
        public float Peasants
        {
            get => Groups[(int)Faction.PEASANTS];
            private set => Groups[(int)Faction.PEASANTS] =
                Mathf.Min(Mathf.Max(MinValue, value), MaxValue);
        }
        public float Alchemists
        {
            get => Groups[(int)Faction.ALCHEMISTS];
            private set => Groups[(int)Faction.ALCHEMISTS] =
                Mathf.Min(Mathf.Max(MinValue, value), MaxValue);
        }
        public float Clerics
        {
            get => Groups[(int)Faction.CLERICS];
            private set => Groups[(int)Faction.CLERICS] =
                Mathf.Min(Mathf.Max(MinValue, value), MaxValue);
        }
        //Constructors
        public PopulationGroups(float min = -Mathf.Infinity, float max = -Mathf.Infinity)
        {
            MinValue = min;
            MaxValue = max;
        }
        //Operators
        public static PopulationGroups operator+ (PopulationGroups a, PopulationGroups b)
        {
            PopulationGroups p = new PopulationGroups();
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                p.Groups[i] = a.Groups[i] + b.Groups[i];
            }
            return p;
        }
        public static PopulationGroups operator* (PopulationGroups a, PopulationGroups b)
        {
            PopulationGroups p = new PopulationGroups();
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                p.Groups[i] = a.Groups[i] * b.Groups[i];
            }
            return p;
        }
        public static PopulationGroups operator *(PopulationGroups a, float b)
        {
            PopulationGroups p = new PopulationGroups();
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                p.Groups[i] = a.Groups[i] * b;
            }
            return p;
        }
        public static PopulationGroups operator /(PopulationGroups a, PopulationGroups b)
        {
            PopulationGroups p = new PopulationGroups();
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                p.Groups[i] = a.Groups[i] / b.Groups[i];
            }
            return p;
        }
        public static Faction Highest(PopulationGroups p)
        {
            float f = -1;
            int index = 0;
            for (int i = 0; i < (int)Faction.SIZE; i++)
            {
                f = Mathf.Max(f, p.Groups[i]);
                if (p.Groups[i] > f)
                {
                    f = p.Groups[i];
                    index = i;
                }
            }
            return (Faction)index;
        }
    }

    public PopulationGroups Groups { get; private set; } = new PopulationGroups(0.1f, 1f);

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

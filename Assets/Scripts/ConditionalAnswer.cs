using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionalAnswer : Answer
{
    public struct Condition
    {
        public Utilities.SocialClass socialClass;
        public int threshold;
    }

    public Condition condition;
}

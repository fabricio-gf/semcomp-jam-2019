using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities //FIXME: undescriptive name
{
    public enum SocialClass
    {
        MERCHANT,
        GUARD,
        COMMONER,
        NOBLE,
        ALCHEMIST,
        CLERIC
    }

    public static World.Faction Adapted(SocialClass s)
    {
        switch (s)
        {
            case SocialClass.MERCHANT:
                return World.Faction.MERCHANTS;
            case SocialClass.GUARD:
                return World.Faction.GUARD;
            case SocialClass.COMMONER:
                return World.Faction.PEASANTS;
            case SocialClass.NOBLE:
                return World.Faction.NOBILITY;
            case SocialClass.ALCHEMIST:
                return World.Faction.ALCHEMISTS;
            case SocialClass.CLERIC:
                return World.Faction.CLERICS;
            default:
                return World.Faction.SIZE;
        }
    }

}

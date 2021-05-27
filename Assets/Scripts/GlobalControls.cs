using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalControls
{
    private static int noStoredWaterTime = 3;
    /* poopTimeLeft and waterTimeLeft should be set to initial values after the quake,
       if the player has stored water, they should have the default 12 hours on the water meter, if not
       only 3; then the meters should be enabled in this script*/
    // metersEnabled is currently set to true for testing purposes
    private static bool metersEnabled = true;
    private static int poopTimeLeft = 24;
    private static int waterTimeLeft = 12;
    private static bool poopTaskCompleted = false;
    private static bool waterTaskCompleted = false;
    private static int currentScene;

    private static int turnNumber = 0;
    private static string currentNPC;
    private static Dictionary<string, string> convoDict = new Dictionary<string, string>()
    {
        {"fred0","fred0"},
        {"dem0", "dem0"},
        {"safi0","safi0"},
        {"rainer0", "rainer0"}
    };
    

    public static bool MetersEnabled
    {
        get
        {
            return metersEnabled;
        }
        set
        {
            metersEnabled = value;
        }
    }

    public static int PoopTimeLeft
    {
        get
        {
            return poopTimeLeft;
        }
        set
        {
            poopTimeLeft = value;
        }
    }

    public static int WaterTimeLeft
    {
        get
        {
            return waterTimeLeft;
        }
        set
        {
            waterTimeLeft = value;
        }
    }

    public static bool PoopTaskCompleted
    {
        get
        {
            return poopTaskCompleted;
        }
        set
        {
            poopTaskCompleted = value;
        }
    }

    public static bool WaterTaskCompleted
    {
        get
        {
            return waterTaskCompleted;
        }
        set
        {
            waterTaskCompleted = value;
        }
    }

    public static int TurnNumber
    {
        get
        {
            return turnNumber;
        }
        set
        {
            turnNumber = value;
        }
    }

    public static string CurrentNPC
    {
        get
        {
            return currentNPC;
        }

        set
        {
            currentNPC = value;
        }
    }

    public static Dictionary<string, string> ConvoDict
    {
        get
        {
            return convoDict;
        }

        set
        {
            
        }
    }

    public static int CurrentScene
    {
        get
        {
            return currentScene;
        }

        set
        {
            currentScene = value;
        }
    }
    public static void SetCheckpoint(string nodeName)
    {
        convoDict[currentNPC] = nodeName;
    }
    
}

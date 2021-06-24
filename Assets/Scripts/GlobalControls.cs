using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private static bool isStrategicMap;

    private static int turnNumber = 0;
    private static string currentNPC;
    
    private static bool safiInteracted;
    private static bool demInteracted;
    private static bool rainerInteracted;
    private static bool fredInteracted;

    private static Dictionary<string, NPC> npcList = new Dictionary<string, NPC>
    {
        {"safi0", new NPC("safi0", "Park", new List<string>{"Dog Collar"}, "safi0", 
            0, false, "Safi needs a Dog Collar")},
        {"dem0", new NPC("dem0", "Park", new List<string>{"Can Opener", "Mask"}, "dem0", 
            0, false, "Demitrius needs a Can Opener and Mask")},
        {"fred0", new NPC("fred0", "School", new List<string>{"Wrench"}, "fred0", 
            0, false, "Fred needs a Wrench")},
        {"rainer0", new NPC("rainer0", "School", new List<string>{"Bucket"}, "rainer0",
            0, false, "Rainer needs a Bucket")},
    };


    public static void Reset()
    {
        npcList = new Dictionary<string, NPC>
        {
            {"safi0", new NPC("safi0", "Park", new List<string>{"Dog Collar"}, "safi0", 
                0, false, "Safi needs a Dog Collar")},
            {"dem0", new NPC("dem0", "Park", new List<string>{"Can Opener", "Mask"}, "dem0", 
                0, false, "Demitrius needs a Can Opener and Mask")},
            {"fred0", new NPC("fred0", "School", new List<string>{"Wrench"}, "fred0", 
                0, false, "Fred needs a Wrench")},
            {"rainer0", new NPC("rainer0", "School", new List<string>{"Bucket"}, "rainer0",
                0, false, "Rainer needs a Bucket")},
        };
        metersEnabled = true;
        poopTimeLeft = 24;
        waterTimeLeft = 12;
        poopTaskCompleted = false;
        waterTaskCompleted = false;
        currentScene = -1;
        isStrategicMap = false;

        turnNumber = 0;
        currentNPC = "";
        
        safiInteracted = false;
        demInteracted = false;
        rainerInteracted = false;
        fredInteracted = false;
    }
    public static bool IsStrategicMap
    {
        get => isStrategicMap;
        set => isStrategicMap = value;
    }


    public static Dictionary<string, NPC> NPCList
    {
        get
        {
            return npcList;
        }
    }

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
        NPCList[currentNPC].node = nodeName;
    }
    
    public static bool SafiInteracted
    {
        get
        {
            return safiInteracted;
        }

        set
        {
            safiInteracted = value;
        }
    }
    
    public static bool DemInteracted
    {
        get
        {
            return demInteracted;
        }

        set
        {
            demInteracted = value;
        }
    }
    
    public static bool RainerInteracted
    {
        get
        {
            return rainerInteracted;
        }

        set
        {
            rainerInteracted = value;
        }
    }
    
    public static bool FredInteracted
    {
        get
        {
            return fredInteracted;
        }

        set
        {
            fredInteracted = value;
        }
    }
    
}







































﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GlobalControls
{
    private static int noStoredWaterTime = 2;
    /* poopTimeLeft and waterTimeLeft should be set to initial values after the quake,
       if the player has stored water, they should have the default 12 hours on the water meter, if not
       only 3; then the meters should be enabled in this script*/
    // metersEnabled is currently set to true for testing purposes
    private static bool tooltipsEnabled = true;
    private static bool adminMode = true;
    private static bool metersEnabled = true;
    private static bool objectivesEnabled = true;
    private static bool keybindsEnabled = true;
    private static int currentObjective;
    private static int poopTimeLeft = 24;
    private static int waterTimeLeft = 12;
    private static bool poopTaskCompleted;
    private static bool waterTaskCompleted;
    private static int currentScene;
    private static bool isStrategicMap;

    private static int turnNumber = 0;
    private static string currentNPC;

    private static bool safiInteracted;
    private static bool demInteracted;
    private static bool rainerInteracted;
    private static bool fredInteracted;

    private static bool apartmentCondition = false;

    private static Dictionary<string, NPC> npcList;
    private static Dictionary<string, string> keybinds;

    static GlobalControls()
    {
        Debug.Log("idk");
        if (apartmentCondition)
        {
            npcList = new Dictionary<string, NPC>
            {
                {"safi0", new NPC("Safi", "Park", new List<string>{"Dog Collar"}, "safi0", 
                    0, false, "Safi needs a Dog Collar", 1)},
                {"dem0", new NPC("Demitrius", "Park", new List<string>{"Can Opener", "Mask"}, "dem0", 
                    0, false, "Demitrius needs a Can Opener and Mask", 2)},
                {"fred0", new NPC("Fred", "School", new List<string>{"Wrench"}, "fred0", 
                    0, false, "Fred needs a Wrench", 1)},
                {"rainer0", new NPC("Rainer", "School", new List<string>{"Tarp"}, "rainer0",
                    0, false, "Rainer needs a Tarp", 1)},
            };
            currentObjective = 2;
        }
        else
        {
            npcList = new Dictionary<string, NPC>
            {
                {"safi0", new NPC("Safi", "Park", new List<string> {"Dog Collar"}, "safi0",
                        0, false, "Safi needs a Dog Collar", 1)},
                {"dem0", new NPC("Demitrius", "Park", new List<string> {"Can Opener", "Mask"}, "dem0",
                        0, false, "Demitrius needs a Can Opener and Mask", 2)},
                {"fred0", new NPC("Fred", "School", new List<string> {"Wrench"}, "fred0",
                        0, false, "Fred needs a Wrench", 1)},
                {"rainer0", new NPC("Rainer", "School", new List<string> {"Bucket"}, "rainer0",
                        0, false, "Rainer needs a Bucket", 1)},
            };
            currentObjective = 1;
        }

        keybinds = new Dictionary<string, string>()
        {
            {"Exploring", "WASD => Move Character \nSPACE => PickUp/Interact \n< > => Move slots \n[ ] => Switch inventory"},
            {"Trading", "< > => select slot. \nSPACE => add item. \n[ ] => change selected inventory. \nENTER => confirm. \nESC => leave interaction."},
            {"Conversing", "< > => Switch Option \nSPACE => Select Option \nESC => Leave Interaction"},
            {"StrategicMap", "< > => Move Locations \nSPACE => Travel to Location"}
        };
        
        metersEnabled = true;
        objectivesEnabled = true;
        tooltipsEnabled = true;
        keybindsEnabled = true;
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
        adminMode = true;
    }


    public static void Reset()
    {
        if (apartmentCondition)
        {
            npcList = new Dictionary<string, NPC>
            {
                {"safi0", new NPC("Safi", "Park", new List<string>{"Dog Collar"}, "safi0", 
                    0, false, "Safi needs a Dog Collar", 1)},
                {"dem0", new NPC("Demitrius", "Park", new List<string>{"Can Opener", "Mask"}, "dem0", 
                    0, false, "Demitrius needs a Can Opener and Mask", 2)},
                {"fred0", new NPC("Fred", "School", new List<string>{"Wrench"}, "fred0", 
                    0, false, "Fred needs a Wrench", 1)},
                {"rainer0", new NPC("Rainer", "School", new List<string>{"Tarp"}, "rainer0",
                    0, false, "Rainer needs a Tarp", 1)},
            };
            currentObjective = 2;
        }
        else
        {
            npcList = new Dictionary<string, NPC>
            {
                {"safi0", new NPC("Safi", "Park", new List<string> {"Dog Collar"}, "safi0",
                    0, false, "Safi needs a Dog Collar", 1)},
                {"dem0", new NPC("Demitrius", "Park", new List<string> {"Can Opener", "Mask"}, "dem0",
                    0, false, "Demitrius needs a Can Opener and Mask", 2)},
                {"fred0", new NPC("Fred", "School", new List<string> {"Wrench"}, "fred0",
                    0, false, "Fred needs a Wrench", 1)},
                {"rainer0", new NPC("Rainer", "School", new List<string> {"Bucket"}, "rainer0",
                    0, false, "Rainer needs a Bucket", 1)},
            };
            currentObjective = 1;
        }

        keybinds = new Dictionary<string, string>()
        {
            {"Exploring", "WASD => Move Character \nSPACE => PickUp/Interact \n< > => Move slots \n[ ] => Switch inventory"},
            {"Trading", "< > => select slot. \nSPACE => add item. \n[ ] => change selected inventory. \nENTER => confirm. \nESC => leave interaction."},
            {"Conversing", "< > => Switch Option \nSPACE => Select Option \nESC => Leave Interaction"},
            {"StrategicMap", "< > => Move Locations \nSPACE => Travel to Location"}
        };
        
        metersEnabled = true;
        tooltipsEnabled = true;
        objectivesEnabled = true;
        keybindsEnabled = true;
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
        adminMode = true;
    }

    public static int NoStoredWaterTime
    {
        get => noStoredWaterTime;
        set => noStoredWaterTime = value;
    }

    public static int CurrentObjective
    {
        get => currentObjective;
        set => currentObjective = value;
    }
    public static bool ApartmentCondition
    {
        get => apartmentCondition;
        set => apartmentCondition = value;
    }
    public static bool KeybindsEnabled
    {
        get => keybindsEnabled;
        set => keybindsEnabled = value;
    }
    public static bool ObjectivesEnabled
    {
        get => objectivesEnabled;
        set => objectivesEnabled = value;
    }
    
    public static bool IsStrategicMap
    {
        get => isStrategicMap;
        set => isStrategicMap = value;
    }

    public static bool TooltipsEnabled
    {
        get => tooltipsEnabled;
        set => tooltipsEnabled = value;
    }
    
    public static bool AdminMode
    {
        get => adminMode;
        set => adminMode = value;
    }

    public static Dictionary<string, NPC> NPCList
    {
        get => npcList;
    }

    public static Dictionary<string, string> Keybinds
    {
        get => keybinds;
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







































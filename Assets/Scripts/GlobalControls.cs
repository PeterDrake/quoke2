using System.Collections;
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
    private static bool[] poopTaskProgress;
    private static int timesShoveled;
    private static int currentScene;
    private static bool isStrategicMap;

    private static int turnNumber = 0;
    private static string currentNPC;

    private static bool safiInteracted;
    private static bool demInteracted;
    private static bool rainerInteracted;
    private static bool fredInteracted;
    private static bool carlosInteracted;
    private static bool bobInteracted;

    private static bool apartmentCondition = false;

    private static bool demActionDone = false;
    private static bool demDrinkDone = false;
    private static bool safiActionDone1 = false;
    private static bool safiActionDone2 = false;
    private static bool safiActionDone3 = false;
    private static bool bobDrinkDone = false;
    private static bool carlosDrinkDone = false;
    private static bool rainerActionDone = false;
    private static bool rainerDrinkDone = false;
    private static bool fredActionDone = false;
    private static bool fredDrinkDone = false;

    private static bool playerHasCleanWater = false;
    
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
                {"carlos0", new NPC("Carlos", "Garden", new List<string> {""}, "carlos0",
                    0, false, "Carlos needs nothing", 0)},
                {"bob0", new NPC("Bob", "Garden", new List<string> {""}, "bob0",
                    0, false, "Bob needs nothing", 0)},
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
                {"carlos0", new NPC("Carlos", "Garden", new List<string> {""}, "carlos0",
                    0, false, "Carlos needs nothing", 0)},
                {"bob0", new NPC("Bob", "Garden", new List<string> {""}, "bob0",
                    0, false, "Bob needs nothing", 0)},
            };
            currentObjective = 1;
        }

        keybinds = new Dictionary<string, string>()
        {
            {"Exploring", "WASD => Move Character \nSPACE => Drop/Interact \n< > => Move slots \n[ ] => Switch inventory"},
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
        poopTaskProgress = new bool[5];
        timesShoveled = 0;
        currentScene = -1;
        isStrategicMap = false;

        turnNumber = 0;
        currentNPC = "";
        
        safiInteracted = false;
        demInteracted = false;
        rainerInteracted = false;
        fredInteracted = false;
        carlosInteracted = false;
        bobInteracted = false;
        adminMode = true;

        demActionDone = false;
        demDrinkDone = false;
        safiActionDone1 = false;
        safiActionDone2 = false;
        safiActionDone3 = false;
        bobDrinkDone = false;
        carlosDrinkDone = false;
        rainerActionDone = false;
        rainerDrinkDone = false;
        fredActionDone = false;
        fredDrinkDone = false;

        playerHasCleanWater = false;
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
                {"carlos0", new NPC("Carlos", "Garden", new List<string> {""}, "carlos0",
                    0, false, "Carlos needs nothing", 0)},
                {"bob0", new NPC("Bob", "Garden", new List<string> {""}, "bob0",
                    0, false, "Bob needs nothing", 0)},
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
                {"carlos0", new NPC("Carlos", "Garden", new List<string> {""}, "carlos0",
                    0, false, "Carlos needs nothing", 0)},
                {"bob0", new NPC("Bob", "Garden", new List<string> {""}, "bob0",
                    0, false, "Bob needs nothing", 0)},
            };
            currentObjective = 1;
        }

        keybinds = new Dictionary<string, string>()
        {
            {"Exploring", "WASD => Move Character \nSPACE => Drop/Interact \n< > => Move slots \n[ ] => Switch inventory"},
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
        poopTaskProgress = new bool[5];
        timesShoveled = 0;
        currentScene = -1;
        isStrategicMap = false;

        turnNumber = 0;
        currentNPC = "";
        
        safiInteracted = false;
        demInteracted = false;
        rainerInteracted = false;
        fredInteracted = false;
        carlosInteracted = false;
        bobInteracted = false;
        adminMode = true;
        
        demActionDone = false;
        demDrinkDone = false;
        safiActionDone1 = false;
        safiActionDone2 = false;
        safiActionDone3 = false;
        bobDrinkDone = false;
        carlosDrinkDone = false;
        rainerActionDone = false;
        rainerDrinkDone = false;
        fredActionDone = false;
        fredDrinkDone = false;

        playerHasCleanWater = false;
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
    
    public static bool[] PoopTaskProgress
    {
        get => poopTaskProgress;
        set => poopTaskProgress = value;
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

    public static Dictionary<string, NPC> NPCList => npcList;

    public static Dictionary<string, string> Keybinds => keybinds;

    public static bool MetersEnabled
    {
        get => metersEnabled;
        set => metersEnabled = value;
    }

    public static int PoopTimeLeft
    {
        get => poopTimeLeft;
        set => poopTimeLeft = value;
    }
    
    public static int TimesShoveled
    {
        get => timesShoveled;
        set => timesShoveled = value;
    }

    public static int WaterTimeLeft
    {
        get => waterTimeLeft;
        set => waterTimeLeft = value;
    }

    public static bool PoopTaskCompleted
    {
        get => poopTaskCompleted;
        set => poopTaskCompleted = value;
    }

    public static bool WaterTaskCompleted
    {
        get => waterTaskCompleted;
        set => waterTaskCompleted = value;
    }

    public static int TurnNumber
    {
        get => turnNumber;
        set => turnNumber = value;
    }

    public static string CurrentNPC
    {
        get => currentNPC;
        set => currentNPC = value;
    }
    

    public static int CurrentScene
    {
        get => currentScene;
        set => currentScene = value;
    }
    public static void SetCheckpoint(string nodeName)
    {
        NPCList[currentNPC].node = nodeName;
    }
    
    public static bool SafiInteracted
    {
        get => safiInteracted;
        set => safiInteracted = value;
    }
    
    public static bool DemInteracted
    {
        get => demInteracted;
        set => demInteracted = value;
    }
    
    public static bool RainerInteracted
    {
        get => rainerInteracted;
        set => rainerInteracted = value;
    }
    
    public static bool FredInteracted
    {
        get => fredInteracted;
        set => fredInteracted = value;
    }
    public static bool CarlosInteracted
    {
        get => carlosInteracted;
        set => carlosInteracted = value;
    }
    public static bool BobInteracted
    {
        get => bobInteracted;
        set => bobInteracted = value;
    }
    
    public static bool DemActionDone
    {
        get => demActionDone;
        set => demActionDone = value;
    }
    
    public static bool DemDrinkDone
    {
        get => demDrinkDone;
        set => demDrinkDone = value;
    }
    
    public static bool SafiActionDone1
    {
        get => safiActionDone1;
        set => safiActionDone1 = value;
    }
    
    public static bool SafiActionDone2
    {
        get => safiActionDone2;
        set => safiActionDone2 = value;
    }
    
    public static bool SafiActionDone3
    {
        get => safiActionDone3;
        set => safiActionDone3 = value;
    }
    
    public static bool BobDrinkDone
    {
        get => bobDrinkDone;
        set => bobDrinkDone = value;
    }
    
    public static bool CarlosDrinkDone
    {
        get => carlosDrinkDone;
        set => carlosDrinkDone = value;
    }
    
    public static bool RainerActionDone
    {
        get => rainerActionDone;
        set => rainerActionDone = value;
    }
    
    public static bool RainerDrinkDone
    {
        get => rainerDrinkDone;
        set => rainerDrinkDone = value;
    }
    
    public static bool FredActionDone
    {
        get => fredActionDone;
        set => fredActionDone = value;
    }
    
    public static bool FredDrinkDone
    {
        get => fredDrinkDone;
        set => fredDrinkDone = value;
    }
    
    public static bool PlayerHasCleanWater
    {
        get => playerHasCleanWater;
        set => playerHasCleanWater = value;
    }

}







































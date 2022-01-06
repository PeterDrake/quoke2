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
    
    private static bool[] poopTaskProgress;
    private static int currentObjective;
    private static int currentPoints = 0;
    private static int poopTimeLeft = 24;
    private static int waterTimeLeft = 12;
    private static int timesShoveled;
    private static int currentScene;

    private static int npcTotalSatisfaction = 3;
    private static int turnNumber = 0;
    private static string currentNPC;

    public static string[] npcNames = { "Safi","Dem","Rainer","Annette","Carlos","Angie" };
    // public static HashSet<string> interacted;
    
    public static Dictionary<string, string> namesList = new Dictionary<string, string>
    {
        {"Safi","Safi"},
        {"Rainer","Rainer"},
        {"Carlos","Carlos"},
        {"Annette","Annette"},
        {"Angie","Angie"},
        {"Demitrius","Demitrius"},
    };

    public static HashSet<string> globalControlsProperties = new HashSet<string>();
    
    public static Dictionary<string, NPC> npcList;
    private static Dictionary<string, string> keybinds;

    private static Dictionary<string, int> points = new Dictionary<string, int>
    {
        {"safirescue", 10},
        {"favors", 5},
        {"tradeneeds", 10},
        {"drink", 5},
        {"sanitationdeath", -100},
        {"crusheddeath", -5},
        {"enteringbuildingdeath", -10},
        {"diedrate", -7},
        {"aftershockdeath", -5},
    };
    static GlobalControls()
    {
        Debug.Log("Starting Global Controls");
        Reset();
    }


    public static void ResetNPCInteracted()
    {
        npcList = new Dictionary<string, NPC>
            {
                {"Safi", new NPC(namesList["Safi"], "Park",
                    new List<string>{""}, new List<bool>{}, "basic_safi_0", 
                    0, false, "Safi needs you to turn off her Gas and Water.", npcTotalSatisfaction,
                    0, new bool[3], new List<string>{"","Wrench","Wrench"})},
                {"Dem", new NPC(namesList["Demitrius"], "Park",
                    new List<string>{"Canned Food", "Can Opener"}, new List<bool>{false, false}, "basic_dem_0",
                    0, false, "Demitrius needs a Can Opener and Canned Food", npcTotalSatisfaction,
                    0, new bool[1], new List<string>{""})},
                {"Annette", new NPC(namesList["Annette"], "School",
                    new List<string>{"Leash", "Dog Crate"}, new List<bool>{false, false}, "basic_annette_0", 
                    0, false, "Annette needs a leash and Dog Crate", npcTotalSatisfaction,
                    0, new bool[1], new List<string>{""})},
                {"Rainer", new NPC(namesList["Rainer"], "School",
                    new List<string>{"Tent", "Blanket"}, new List<bool>{false, false}, "basic_rainer_0",
                    0, false, "Rainer needs a Tent and Blanket", npcTotalSatisfaction,
                    0, new bool[1], new List<string>{""})},
                {"Carlos", new NPC(namesList["Carlos"], "Garden",
                    new List<string> {"Radio", "Batteries"}, new List<bool>{false, false}, "basic_carlos_0",
                    0, false, "Carlos needs a Radio and Batteries", npcTotalSatisfaction,
                    0, new bool[1], new List<string>{"Water Bottle Clean"})},
                {"Angie", new NPC(namesList["Angie"], "Garden",
                    new List<string> {"First Aid Kit", "Epi Pen"}, new List<bool>{false, false}, "basic_angie_0",
                    0, false, "Angie needs a First Aid Kit and Epi Pen", npcTotalSatisfaction,
                    0, new bool[1], new List<string>{"Water Bottle Clean"})},
                
            };
        foreach (string name in npcNames)
        {
            npcList[name].interacted = false;
        }
    }
    
    public static void Reset()
    {
        ResetNPCInteracted();
        
        if (globalControlsProperties.Contains("apartmentCondition"))
        {
            currentObjective = 2;
        }
        else
        {
            currentObjective = 1;
        }

        keybinds = new Dictionary<string, string>()
        {
            {"Exploring", "WASD => Move Character \nSPACE => Drop/Interact \n< > => Move slots \n[ ] => Switch inventory"},
            {"Trading", "< > => select slot. \nSPACE => add item. \n[ ] => change selected inventory. \nENTER => confirm. \nESC => leave interaction."},
            {"Conversing", "< > => Switch Option \nSPACE => Select Option \nESC => Leave Interaction"},
            {"StrategicMap", "< > => Move Locations \nSPACE => Travel to Location"}
        };
        
        globalControlsProperties.Add("metersEnabled");
        globalControlsProperties.Add("tooltipsEnabled");
        globalControlsProperties.Add("objectivesEnabled");
        globalControlsProperties.Add("keybindsEnabled");
       
        poopTimeLeft = 24;
        waterTimeLeft = 12;

        globalControlsProperties.Remove("poopTaskCompleted");
        globalControlsProperties.Remove("waterTaskCompleted");
        
        poopTaskProgress = new bool[5];
        timesShoveled = 0;
        currentScene = -1;
        globalControlsProperties.Remove("isStrategicMap");
        currentPoints = 0;

        turnNumber = 0;
        currentNPC = "";
        
        globalControlsProperties.Add("adminMode");
        
        globalControlsProperties.Remove("demActionDone");
        globalControlsProperties.Remove("demDrinkDone");
        
        globalControlsProperties.Remove("safiWaterActionDone");
        globalControlsProperties.Remove("safiGasActionDone");
        globalControlsProperties.Remove("safiRescued");
        
        globalControlsProperties.Remove("angieDrinkDone");
        
        globalControlsProperties.Remove("carlosDrinkDone");
        
        globalControlsProperties.Remove("rainerActionDone");
        globalControlsProperties.Remove("rainerDrinkDone");
        
        globalControlsProperties.Remove("annetteActionDone");
        globalControlsProperties.Remove("annetteHasLeash");
        globalControlsProperties.Remove("annetteHasDogCrate");
      
        globalControlsProperties.Remove("playerHasCleanWater");
        globalControlsProperties.Remove("playerHasFirstAidKit");
        globalControlsProperties.Remove("playerHasEpiPen");
        globalControlsProperties.Remove("playerHasWrench");

        globalControlsProperties.Remove("angieHasFirstAidKit");
        globalControlsProperties.Remove("angieSeriousDialogue");
        globalControlsProperties.Remove("angieHasEpiPen");
    }

    public static Dictionary<string,int> Points
    {
        get => points;
    }
    
    public static int NoStoredWaterTime
    {
        get => noStoredWaterTime;
        set => noStoredWaterTime = value;
    }
    
    public static int CurrentPoints
    {
        get => currentPoints;
        set => currentPoints = value;
    }

    public static int CurrentObjective
    {
        get => currentObjective;
        set => currentObjective = value;
    }

    public static bool[] PoopTaskProgress
    {
        get => poopTaskProgress;
        set => poopTaskProgress = value;
    }
    
    public static Dictionary<string, string> Keybinds => keybinds;

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
        npcList[currentNPC].node = nodeName;
    }
}


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
    private static int currentPoints = 0;
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
    
    // private static bool apartmentCondition = false;

    // private static bool demActionDone = false;
    // private static bool demDrinkDone = false;
    // private static bool safiWaterActionDone = false;
    // private static bool safiGasActionDone = false;
    // private static bool safiRescued = false;
    // private static bool angieDrinkDone = false;
    // private static bool carlosDrinkDone = false;
    // private static bool rainerActionDone = false;
    // private static bool rainerDrinkDone = false;
    // private static bool annetteActionDone = false;
    // private static bool annetteDrinkDone = false;

    // private static bool playerHasCleanWater = false;
    // private static bool playerHasFirstAidKit = false;
    // private static bool playerHasEpiPen = false;
    // private static bool playerHasWrench = false;
    
    // private static bool angieHasEpiPen = false;
    // private static bool angieHasFirstAidKit = false;
    // private static bool angieSeriousDialogue = false;
    
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

    private static Dictionary<string, string> angieActions = new Dictionary<string, string>
    {
        {"angie0", "action0_angie_8.2"},
        {"angie1", "action0_angie_7.2"},
    };
    private static Dictionary<string, string> safiActions = new Dictionary<string, string>
    {
        //shut of gas, shut of water, and rescue
        {"safi0", ""},
        {"safi1", ""},
        {"safi2", ""},
    };
    private static Dictionary<string, string> demActions = new Dictionary<string, string>
    {
        //sip of water and foraging
        {"dem0", ""},
        {"dem1", ""},
    };
    private static Dictionary<string, string> rainerActions = new Dictionary<string, string>
    {
        //sip of water and comforting words
        {"rainer0", ""},
        {"rainer1", ""},
    };
    private static Dictionary<string, string> annetteActions = new Dictionary<string, string>
    {
        //sip of water and translation
        {"annette0", ""},
        {"annette1", ""},
    };
    private static Dictionary<string, string> carlosActions = new Dictionary<string, string>
    {
        //sip of water
        {"carlos0", ""},
    };



    static GlobalControls()
    {
        Debug.Log("Starting Global Controls");
        Reset();
    }


    public static void Reset()
    {
        if (globalControlsProperties.Contains("apartmentCondition"))
        {
            npcList = new Dictionary<string, NPC>
            {
                {"Safi", new NPC(namesList["Safi"], "Park", new List<string>{""}, "basic_safi_0", 
                    0, false, "Safi needs you to turn off her Gas and Water.", 3, 0, safiActions,
                    new List<string>{"","Wrench","Wrench"})},
                {"Dem", new NPC(namesList["Demitrius"], "Park", new List<string>{"Canned Food", "Can Opener"}, "basic_dem_0",
                    0, false, "Demitrius needs a Can Opener and Canned Food", 4,0, demActions,
                    new List<string>{""})},
                {"Annette", new NPC(namesList["Annette"], "School", new List<string>{"Leash", "Dog Crate"}, "basic_annette_0", 
                    0, false, "Annette needs a leash and Dog Crate", 4,0, annetteActions,
                    new List<string>{""})},
                {"Rainer", new NPC(namesList["Rainer"], "School", new List<string>{"Tent", "Blanket"}, "basic_annette_0",
                    0, false, "Rainer needs a Tent and Blanket", 4, 0, rainerActions,
                    new List<string>{""})},
                {"Carlos", new NPC(namesList["Carlos"], "Garden", new List<string> {"Radio", "Batteries"}, "basic_carlos_0",
                    0, false, "Carlos needs a Radio and Batteries", 4, 0, carlosActions,
                    new List<string>{"Water Bottle Clean"})},
                {"Angie", new NPC(namesList["Angie"], "Garden", new List<string> {"First Aid Kit", "Epi Pen"}, "basic_angie_0",
                    0, false, "Angie needs a First Aid Kit and Epi Pen", 4, 0, angieActions,
                    new List<string>{"Water Bottle Clean"})},
                
            };
            currentObjective = 2;
        }
        else
        {
            npcList = new Dictionary<string, NPC>
            {
                {"Safi", new NPC(namesList["Safi"], "Park", new List<string>{""}, "basic_safi_0", 
                    0, false, "Safi needs you to turn off her Gas and Water.", 3, 0, safiActions,
                    new List<string>{"","Wrench","Wrench"})},
                {"Dem", new NPC(namesList["Demitrius"], "Park", new List<string>{"Canned Food", "Can Opener"}, "basic_dem_0",
                    0, false, "Demitrius needs a Can Opener and Canned Food", 4,0, demActions,
                    new List<string>{""})},
                {"Annette", new NPC(namesList["Annette"], "School", new List<string>{"Leash", "Dog Crate"}, "basic_annette_0", 
                    0, false, "Annette needs a leash and Dog Crate", 4,0, annetteActions,
                    new List<string>{""})},
                {"Rainer", new NPC(namesList["Rainer"], "School", new List<string>{"Tent", "Blanket"}, "basic_annette_0",
                    0, false, "Rainer needs a Tent and Blanket", 4, 0, rainerActions,
                    new List<string>{""})},
                {"Carlos", new NPC(namesList["Carlos"], "Garden", new List<string> {"Radio", "Batteries"}, "basic_carlos_0",
                    0, false, "Carlos needs a Radio and Batteries", 4, 0, carlosActions,
                    new List<string>{"Water Bottle Clean"})},
                {"Angie", new NPC(namesList["Angie"], "Garden", new List<string> {"First Aid Kit", "Epi Pen"}, "basic_angie_0",
                    0, false, "Angie needs a First Aid Kit and Epi Pen", 4, 0, angieActions,
                    new List<string>{"Water Bottle Clean"})},
                
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
        currentPoints = 0;

        turnNumber = 0;
        currentNPC = "";

        foreach (string name in npcNames)
        {
            npcList[name].interacted = false;
        }
        
        adminMode = true;

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
        globalControlsProperties.Remove("annetteDrinkDone");
      
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
    // public static bool ApartmentCondition
    // {
    //     get => apartmentCondition;
    //     set => apartmentCondition = value;
    // }
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
        npcList[currentNPC].node = nodeName;
    }
}


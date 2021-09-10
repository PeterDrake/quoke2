using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GlobalControls
{
    public static int noStoredWaterTime = 2;
    /* poopTimeLeft and waterTimeLeft should be set to initial values after the quake,
       if the player has stored water, they should have the default 12 hours on the water meter, if not
       only 3; then the meters should be enabled in this script*/
    // metersEnabled is currently set to true for testing purposes
    public static bool tooltipsEnabled = true;
    public static bool adminMode = true;
    public static bool metersEnabled = true;
    public static bool objectivesEnabled = true;
    public static bool keybindsEnabled = true;
    public static int currentObjective;
    public static int currentPoints = 0;
    public static int poopTimeLeft = 24;
    public static int waterTimeLeft = 12;
    public static bool poopTaskCompleted;
    public static bool waterTaskCompleted;
    public static bool[] poopTaskProgress;
    public static int timesShoveled;
    public static int currentScene;
    public static bool isStrategicMap;

    public static int turnNumber = 0;
    public static string currentNPC;

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
    
    public static bool apartmentCondition = false;

    public static bool demActionDone = false;
    public static bool demDrinkDone = false;
    public static bool safiWaterActionDone = false;
    public static bool safiGasActionDone = false;
    public static bool safiRescued = false;
    public static bool angieDrinkDone = false;
    public static bool carlosDrinkDone = false;
    public static bool rainerActionDone = false;
    public static bool rainerDrinkDone = false;
    public static bool annetteActionDone = false;
    public static bool annetteDrinkDone = false;

    public static bool playerHasCleanWater = false;
    public static bool playerHasFirstAidKit = false;
    public static bool playerHasEpiPen = false;
    public static bool playerHasWrench = false;
    
    public static bool angieHasEpiPen = false;
    public static bool angieHasFirstAidKit = false;
    public static bool angieSeriousDialogue = false;
    
    public static Dictionary<string, NPC> npcList;
    public static Dictionary<string, string> keybinds;

    public static Dictionary<string, int> points = new Dictionary<string, int>
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

    public static Dictionary<string, string> angieActions = new Dictionary<string, string>
    {
        {"angie0", "action0_angie_8.2"},
        {"angie1", "action0_angie_7.2"},
    };
    public static Dictionary<string, string> safiActions = new Dictionary<string, string>
    {
        //shut of gas, shut of water, and rescue
        {"safi0", ""},
        {"safi1", ""},
        {"safi2", ""},
    };
    public static Dictionary<string, string> demActions = new Dictionary<string, string>
    {
        //sip of water and foraging
        {"dem0", ""},
        {"dem1", ""},
    };
    public static Dictionary<string, string> rainerActions = new Dictionary<string, string>
    {
        //sip of water and comforting words
        {"rainer0", ""},
        {"rainer1", ""},
    };
    public static Dictionary<string, string> annetteActions = new Dictionary<string, string>
    {
        //sip of water and translation
        {"annette0", ""},
        {"annette1", ""},
    };
    public static Dictionary<string, string> carlosActions = new Dictionary<string, string>
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
        if (apartmentCondition)
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
        
        demActionDone = false;
        demDrinkDone = false;
        safiWaterActionDone = false;
        safiGasActionDone = false;
        safiRescued = false;
        angieDrinkDone = false;
        carlosDrinkDone = false;
        rainerActionDone = false;
        rainerDrinkDone = false;
        annetteActionDone = false;
        annetteDrinkDone = false;

        playerHasCleanWater = false;
        playerHasFirstAidKit = false;
        playerHasEpiPen = false;
        playerHasWrench = false;
        
        angieHasFirstAidKit = false;
        angieSeriousDialogue = false;
        angieSeriousDialogue = false;
    }

    public static Dictionary<string,int> Points
    {
        get => points;
    }
    
    public static bool SafiRescued
    {
        get => safiRescued;
        set => safiRescued = value;
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
    
    public static bool SafiWaterActionDone
    {
        get => safiWaterActionDone;
        set => safiWaterActionDone = value;
    }
    
    public static bool SafiGasActionDone
    {
        get => safiGasActionDone;
        set => safiGasActionDone = value;
    }
    
    public static bool AngieDrinkDone
    {
        get => angieDrinkDone;
        set => angieDrinkDone = value;
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
    
    public static bool AnnetteActionDone
    {
        get => annetteActionDone;
        set => annetteActionDone = value;
    }
    
    public static bool AnnetteDrinkDone
    {
        get => annetteDrinkDone;
        set => annetteDrinkDone = value;
    }
    
    public static bool PlayerHasCleanWater
    {
        get => playerHasCleanWater;
        set => playerHasCleanWater = value;
    }

    public static bool PlayerHasFirstAidKit
    {
        get => playerHasFirstAidKit;
        set => playerHasFirstAidKit = value;
    }

    public static bool PlayerHasEpiPen
    {
        get => playerHasEpiPen;
        set => playerHasEpiPen = value;
    }

    public static bool PlayerHasWrench
    {
        get => playerHasWrench;
        set => playerHasWrench = value;
    }

    public static bool AngieSeriousDialogue
    {
        get => angieSeriousDialogue;
        set => angieSeriousDialogue = value;
    }

    public static bool AngieHasEpiPen
    {
        get => angieHasEpiPen;
        set => angieHasEpiPen = value;
    }

    public static bool AngieHasFirstAidKit
    {
        get => angieHasFirstAidKit;
        set => angieHasFirstAidKit = value;
    }
}


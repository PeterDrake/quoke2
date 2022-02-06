using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GlobalControls
{
    public static int poopTimeLeft = 24;  // 24 after the quake

    public static int waterTimeLeft = 12;  // 12 after the quake, or 3 if no water was stored

    private static int npcTotalSatisfaction = 3;

    public static string[] npcNames = {"Safi", "Dem", "Rainer", "Annette", "Carlos", "Angie" };

    public static HashSet<string> globalControlsProperties = new HashSet<string>();

    public static Dictionary<string, NPC> npcList;

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

    public static int noStoredWaterTime;

    public static int currentPoints;

    public static int currentObjective;

    public static bool[] poopTaskProgress;

    public static Dictionary<string, string> keybinds;

    public static int timesShoveled;

    public static int turnNumber = 0;

    public static string currentNpc;

    public static int currentScene;

    static GlobalControls()
    {
        Debug.Log("Starting Global Controls");
        Reset();
    }

    /// <summary>
    /// Resets all of the NPCs to their state before meeting the PC.
    /// </summary>
    public static void ResetNPCInteracted()
    {
        npcList = new Dictionary<string, NPC>
            {
                {"Safi", new NPC("Safi", "Park",
                    new List<string>{""}, new List<bool>{}, "basic_safi_0", 
                    0, false, "Safi needs you to turn off her Gas and Water.", npcTotalSatisfaction,
                    0, new bool[3], new List<string>{"","Wrench","Wrench"})},
                {"Dem", new NPC("Demitrius", "Park",
                    new List<string>{"Canned Food", "Can Opener"}, new List<bool>{false, false}, "basic_dem_0",
                    0, false, "Demitrius needs a Can Opener and Canned Food", npcTotalSatisfaction,
                    0, new bool[1], new List<string>{""})},
                {"Annette", new NPC("Annette", "School",
                    new List<string>{"Leash", "Dog Crate"}, new List<bool>{false, false}, "basic_annette_0", 
                    0, false, "Annette needs a leash and Dog Crate", npcTotalSatisfaction,
                    0, new bool[1], new List<string>{""})},
                {"Rainer", new NPC("Rainer", "School",
                    new List<string>{"Tent", "Blanket"}, new List<bool>{false, false}, "basic_rainer_0",
                    0, false, "Rainer needs a Tent and Blanket", npcTotalSatisfaction,
                    0, new bool[1], new List<string>{""})},
                {"Carlos", new NPC("Carlos", "Garden",
                    new List<string> {"Radio", "Batteries"}, new List<bool>{false, false}, "basic_carlos_0",
                    0, false, "Carlos needs a Radio and Batteries", npcTotalSatisfaction,
                    0, new bool[1], new List<string>{"Water Bottle Clean"})},
                {"Angie", new NPC("Angie", "Garden",
                    new List<string> {"First Aid Kit", "Epi Pen"}, new List<bool>{false, false}, "basic_angie_0",
                    0, false, "Angie needs a First Aid Kit and Epi Pen", npcTotalSatisfaction,
                    0, new bool[1], new List<string>{"Water Bottle Clean"})},
            };
    }

    public static void Reset()
    {
        ResetNPCInteracted();
        currentObjective = globalControlsProperties.Contains("apartmentCondition") ? 2 : 1;
        keybinds = new Dictionary<string, string>()
        {
            {"Exploring", "WASD => Move Character \nSPACE => Drop/Interact \n< > => Move slots \n[ ] => Switch inventory"},
            {"Trading", "< > => select slot. \nSPACE => add item. \n[ ] => change selected inventory. \nENTER => confirm. \nESC => leave interaction."},
            {"Conversing", "< > => Switch Option \nSPACE => Select Option \nESC => Leave Interaction"},
            {"StrategicMap", "< > => Move Locations \nSPACE => Travel to Location"}
        };
        poopTimeLeft = 24;
        waterTimeLeft = 12;        
        poopTaskProgress = new bool[5];
        timesShoveled = 0;
        currentScene = -1;
        currentPoints = 0;
        noStoredWaterTime = 2;
        turnNumber = 0;
        currentNpc = "";
        globalControlsProperties.Clear();
        globalControlsProperties.Add("tooltipsEnabled");
        globalControlsProperties.Add("objectivesEnabled");
        globalControlsProperties.Add("keybindsEnabled");
        globalControlsProperties.Add("adminMode");
     }

    // TODO Shouldn't this be in the NPC class?
    public static void SetCheckpoint(string nodeName)
    {
        npcList[currentNpc].node = nodeName;
    }
}


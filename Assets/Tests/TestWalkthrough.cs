using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;


public class TestWalkthrough
{
    private ReferenceManager referenceManager;
    private GameStateManager gameStateManager;
    private SceneManagement sceneManagement;
    private ObjectiveManager objectiveManager;
    private DialogueManager dialogueManager;
    private QuakeManager quakeManager;
    private QuakeSafeZoneManager quakeSafeZoneManager;
    private ItemLoader itemLoader;
    private WaterHeaterOrGasValve waterHeaterOrGasValve;
    private PlayerKeyboardManager playerKeyboard;
    private StrategicMapKeyboardController strategicMapKeyboard;
    private CheatKeyboardController cheatKeyboard;
    private LayerMask gasValve, waterHeater; 


    
    // A Test behaves as an ordinary method
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("PreQuakeHouse"); 
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
        sceneManagement = referenceManager.sceneManagement.GetComponent<SceneManagement>();
        objectiveManager = referenceManager.objectiveManager.GetComponent<ObjectiveManager>();
        dialogueManager = referenceManager.dialogueCanvas.GetComponent<DialogueManager>();
        quakeManager = GameObject.Find("Quake Event Manager").GetComponent<QuakeManager>();
        quakeSafeZoneManager = GameObject.Find("Interactables").GetComponentInChildren<QuakeSafeZoneManager>();
        
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        waterHeaterOrGasValve = GameObject.Find("Interactables").GetComponentInChildren<WaterHeaterOrGasValve>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        cheatKeyboard = referenceManager.keyboardManager.GetComponent<CheatKeyboardController>();
        gasValve = LayerMask.GetMask("GasValve");
        waterHeater = LayerMask.GetMask("WaterHeater");

        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        GlobalControls.ResetNPCInteracted();
        GlobalItemList.Reset();
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // Destroy all objects
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(obj);
        }
        yield return null;
    }

    
    // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     Tarp,           Blanket,        Radio,              Knife
    // Demetrius Inventory: Bleach,         Tent,           Dog Crate,          Rubber Chicken
    // Carlos Inventory:    Plywood,        Can Opener,     Leash,              Whistle
    // Annette Inventory:   Rope,           First Aid Kit,  Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    Toilet Paper,   Batteries,      Epi Pen,            Wrench
    // Player Inventory:    None            None            None                None            None
    [UnityTest]
    public IEnumerator GetsToQuake()
    {
        SceneManager.LoadScene("PreQuakeHouse");
        yield return new WaitForSeconds(0.5f);
        GlobalControls.ResetNPCInteracted();
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        GlobalControls.ResetNPCInteracted();
        yield return MakesMovesToGetToQuake();
    }
    public IEnumerator MakesMovesToGetToQuake()
    {
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();

        yield return QuokeTestUtils.Press("aaawwaaaaaaadddssssssss aads ", playerKeyboard);
        Assert.AreEqual("QuakeHouse", SceneManager.GetActiveScene().name);
    }

    
        
    // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     Tarp,           Blanket,        Radio,              Knife
    // Demetrius Inventory: Bleach,         Tent,           Dog Crate,          Rubber Chicken
    // Carlos Inventory:    Plywood,        Can Opener,     Leash,              Whistle
    // Annette Inventory:   Rope,           First Aid Kit,  Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    Toilet Paper,   Batteries,      Epi Pen,            Wrench
    // Player Inventory:    None            None            None                None            None
    [UnityTest]
    public IEnumerator GetsOutsideAfterQuake()
    {
        sceneManagement.ChangeScene("QuakeHouse");
        yield return new WaitForSeconds(0.5f);

        GlobalControls.Reset();
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
        sceneManagement = referenceManager.sceneManagement.GetComponent<SceneManagement>(); 
        quakeManager = GameObject.Find("Quake Event Manager").GetComponent<QuakeManager>();
        quakeSafeZoneManager = GameObject.Find("Interactables").GetComponentInChildren<QuakeSafeZoneManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        yield return new WaitForSeconds(0.5f);
        
        GlobalItemList.UpdateItemList("Book", "QuakeHouse", new Vector3(0,0,0), "Shed 1");
        GlobalItemList.UpdateItemList("Dirty Water Bottle", "QuakeHouse", new Vector3(-2.5f,0.5f,-8.5f), "Shed 2");
        
        itemLoader.LoadItems("QuakeHouse");
        gameStateManager.Start();
        quakeManager.ResetQuake();
        GlobalControls.TurnNumber = -5;
        yield return MakesMovesToGetOutsideAfterQuake();
    }
    public IEnumerator MakesMovesToGetOutsideAfterQuake()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
        // quakeManager = GameObject.Find("Quake Event Manager").GetComponent<QuakeManager>();
        // quakeSafeZoneManager = GameObject.Find("Interactables").GetComponentInChildren<QuakeSafeZoneManager>();
        yield return new WaitForSeconds(0.5f);

        gameStateManager.SetExploring();
        // Debug.Log("GlobalControls.TurnNumber: " + GlobalControls.TurnNumber);
        // Debug.Log("quakeManager.turnsTillQuakeStart: " + quakeManager.turnsTillQuakeStart); 
        // Debug.Log("quakeManager.initialTurn: " + quakeManager.turnsTillQuakeStart);
        // Debug.Log("quakeManager.quakeStartTurn: " + quakeManager.quakeStartTurn);
        // Debug.Log("quakeManager.isQuakeTime: " + quakeManager.isQuakeTime );
        // Debug.Log("quakeManager.firstQuakeCompleted: " + quakeManager.firstQuakeCompleted);
        // Debug.Log("quakeManager.turnsTillDeath: " + quakeManager.turnsTillDeath);
        // Debug.Log("quakeManager.underCoverTurn: " + quakeManager.underCoverTurn );
        // Debug.Log("quakeManager.isUnderCover: " + quakeManager.isUnderCover );
        // Debug.Log("quakeManager.hasBeenUnderCover: " + quakeManager.hasBeenUnderCover);
        // Debug.Log("quakeManager.turnsTillAftershock: " + quakeManager.turnsTillAftershock );
        // Debug.Log("quakeManager.isAftershockTime: " + quakeManager.isAftershockTime );
        // Debug.Log("quakeManager.automaticAftershock: " + quakeManager.automaticAftershock);
        yield return QuokeTestUtils.Press("cwwadadadad", playerKeyboard);
        yield return new WaitForSeconds(5f);
        yield return QuokeTestUtils.Press("aaawwwwa", playerKeyboard);
        Assert.AreEqual("Yard", SceneManager.GetActiveScene().name);
    }
    
    
        
    // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     Tarp,           Blanket,        Radio,              Knife
    // Demetrius Inventory: Bleach,         Tent,           Dog Crate,          Rubber Chicken
    // Carlos Inventory:    Plywood,        Can Opener,     Leash,              Whistle
    // Annette Inventory:   Rope,           First Aid Kit,  Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    Toilet Paper,   Batteries,      Epi Pen,            Wrench
    // Player Inventory:    None            None            None                None            None
    [UnityTest]
    public IEnumerator PicksUpBookAndLeavesYard()
    {
        SceneManager.LoadScene("Yard");
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        GlobalItemList.UpdateItemList("Book", "Yard", new Vector3(-3.5f,1.5f,-8.5f), "Shed 1");
        GlobalItemList.UpdateItemList("Dirty Water Bottle", "Yard", new Vector3(-2.5f,0.5f,-8.5f), "Shed 2");
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("Yard");
        GlobalControls.ResetNPCInteracted();

        yield return MakesMovesToPickUpBookAndLeaveYard();
    }
    public IEnumerator MakesMovesToPickUpBookAndLeaveYard()
    {
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("aaassssssss ", playerKeyboard);
        Assert.AreEqual("Book(Clone)", inventory.items[0].name);
        yield return QuokeTestUtils.Press("wwwwwwwwwwwwwwww", playerKeyboard);
        Assert.AreEqual("StrategicMap", SceneManager.GetActiveScene().name);
        
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();

        yield return new WaitForSeconds(0.5f);
        yield return QuokeTestUtils.Press("< ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("WaterfrontPark", SceneManager.GetActiveScene().name);
    }
    
    
        
    // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     Tarp,           Blanket,        Radio,              Knife
    // Demetrius Inventory: Bleach,         Tent,           Dog Crate,          Rubber Chicken
    // Carlos Inventory:    Plywood,        Can Opener,     Leash,              Whistle
    // Annette Inventory:   Rope,           First Aid Kit,  Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    Toilet Paper,   Batteries,      Epi Pen,            Wrench
    // Player Inventory:    BOOK            None            None                None            None
    [UnityTest]
    public IEnumerator TradesBleach()
    {
        SceneManager.LoadScene("WaterfrontPark");
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        GlobalItemList.UpdateItemList("Book", "Inventory", new Vector3(0,0,0), "Player");
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("WaterfrontPark");

        GlobalControls.ResetNPCInteracted();
        yield return MakesMovesToTradeBleach();
    }
    public IEnumerator MakesMovesToTradeBleach()
    {
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();

        yield return QuokeTestUtils.Press("wwwwwaaaaaaaaaaaaaa", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        yield return QuokeTestUtils.Press("     >  ", playerKeyboard, cheatKeyboard);
        yield return QuokeTestUtils.Press(" <<<< ~", playerKeyboard, cheatKeyboard);

        // Check that the items were traded successfully 
        Assert.True(referenceManager.tradeCanvas.GetComponent<TradeManagerUI>()
            .inventories[(int)InventoryE.NPC].items[0].name == "Book(Clone)");
        Inventory playerInventory = referenceManager.tradeCanvas.GetComponent<TradeManagerUI>()
            .inventories[(int)InventoryE.Player];
        Assert.True(playerInventory.items[0].name == "Bleach(Clone)");
        
        yield return QuokeTestUtils.Press("``", playerKeyboard, cheatKeyboard);
        yield return QuokeTestUtils.Press("ddddwwwwwwwwwwwwwww", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("StrategicMap", SceneManager.GetActiveScene().name);
        
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();

        yield return new WaitForSeconds(0.5f);
        yield return QuokeTestUtils.Press("> ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Yard", SceneManager.GetActiveScene().name);
        
    }


        
    // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     Tarp,           Blanket,        Radio,              Knife
    // Demetrius Inventory: BOOK,           Tent,           Dog Crate,          Rubber Chicken
    // Carlos Inventory:    Plywood,        Can Opener,     Leash,              Whistle
    // Annette Inventory:   Rope,           First Aid Kit,  Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    Toilet Paper,   Batteries,      Epi Pen,            Wrench
    // Player Inventory:    BLEACH          None            None                None            None
    [UnityTest]
    public IEnumerator PurifiesWater()
    {
        SceneManager.LoadScene("Yard");
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();

        GlobalItemList.UpdateItemList("Bleach", "Inventory", new Vector3(0,0,0), "Player");
        GlobalItemList.UpdateItemList("Dirty Water Bottle", "Yard", new Vector3(-2.5f,1.5f,-8.5f), "Shed 2");
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("Yard");
        GlobalControls.ResetNPCInteracted();
        GlobalControls.globalControlsProperties.Remove("waterTaskCompleted");
        yield return MakesMovesToPurifyWater();
    }
    public IEnumerator MakesMovesToPurifyWater()
    {
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("aassssssss ", playerKeyboard);
        Assert.AreEqual("Bleach(Clone)", inventory.items[1].name);
        Assert.AreEqual("Dirty Water Bottle(Clone)", inventory.items[1].name);
        yield return QuokeTestUtils.Press("wwwwwwwwaaaaaaa", playerKeyboard);
        yield return QuokeTestUtils.Press("> < ", playerKeyboard);
        Assert.AreEqual(GlobalControls.globalControlsProperties.Contains("waterTaskCompleted"),
            true);
        yield return QuokeTestUtils.Press("dwwwwwww", playerKeyboard);
        Assert.AreEqual("StrategicMap", SceneManager.GetActiveScene().name);

        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        yield return QuokeTestUtils.Press("> ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("PioneerCourthouseSquare", SceneManager.GetActiveScene().name);

    }


    // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     Tarp,           Blanket,        Radio,              Knife
    // Demetrius Inventory: BOOK,           Tent,           Dog Crate,          Rubber Chicken
    // Carlos Inventory:    Plywood,        Can Opener,     Leash,              Whistle
    // Annette Inventory:   Rope,           First Aid Kit,  Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    Toilet Paper,   Batteries,      Epi Pen,            Wrench
    // Player Inventory:    BLEACH          None            None                None            None
    [UnityTest]
    public IEnumerator GoToAnnetteToTradeBleach()
    {
        SceneManager.LoadScene("PioneerCourthouseSquare");
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();

        GlobalItemList.UpdateItemList("Bleach", "Inventory", new Vector3(0,0,0), "Player");
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("PioneerCourthouseSquare");
        GlobalControls.ResetNPCInteracted();
        
        yield return MakesMovesToGoToAnnetteToTradeBleach();
    }
    public IEnumerator MakesMovesToGoToAnnetteToTradeBleach()
    {
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("dddddd", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Annette", GlobalControls.CurrentNPC);
        yield return QuokeTestUtils.Press("                   d>   <<< ~``", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("First Aid Kit(Clone)", inventory.items[0].name);
        yield return QuokeTestUtils.Press("aaassssssssssss", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        yield return QuokeTestUtils.Press("> ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("PSU", SceneManager.GetActiveScene().name);

    }


    // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     Tarp,           Blanket,        Radio,              Knife
    // Demetrius Inventory: BOOK,           Tent,           Dog Crate,          Rubber Chicken
    // Carlos Inventory:    Plywood,        Can Opener,     Leash,              Whistle
    // Annette Inventory:   Rope,           BLEACH,         Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    Toilet Paper,   Batteries,      Epi Pen,            Wrench
    // Player Inventory:    FIRST AID KIT   None            None                None            None
    [UnityTest]
    public IEnumerator GoToAngieAndCarlosToTrade()
    {
        SceneManager.LoadScene("PSU");
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();

        GlobalItemList.UpdateItemList("First Aid Kit", "Inventory", new Vector3(0,0,0), "Player");
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("PSU");
        GlobalControls.ResetNPCInteracted();
        yield return MakesMovesToGoToAngieAndCarlosToTrade();
    }
    public IEnumerator MakesMovesToGoToAngieAndCarlosToTrade()
    {
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("wwwwwwwaaaaaaaa", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Angie", GlobalControls.CurrentNPC);
        yield return QuokeTestUtils.Press("        <<< < ~``", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Radio(Clone)", inventory.items[0].name);
        Assert.AreEqual("Tarp(Clone)", inventory.items[1].name);
        
        // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
        // Angie Inventory:     FIRST AID KIT,  Blanket,        NONE,               Knife
        // Demetrius Inventory: BOOK,           Tent,           Dog Crate,          Rubber Chicken
        // Carlos Inventory:    Plywood,        Can Opener,     Leash,              Whistle
        // Annette Inventory:   Rope,           BLEACH,         Fire Extinguisher,  N95 Mask
        // Rainer Inventory:    Toilet Paper,   Batteries,      Epi Pen,            Wrench
        // Player Inventory:    RADIO           TARP            None                None            None
        yield return QuokeTestUtils.Press("ssssssssssssdsssssssssssssssssaa", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Carlos", GlobalControls.CurrentNPC);
        yield return QuokeTestUtils.Press("           << << ~``", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Leash(Clone)", inventory.items[0].name);
        Assert.AreEqual("Tarp(Clone)", inventory.items[1].name);
        Assert.AreEqual("Plywood(Clone)", inventory.items[2].name);
        yield return QuokeTestUtils.Press("ddsss", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        yield return QuokeTestUtils.Press("< ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("PioneerCourthouseSquare", SceneManager.GetActiveScene().name);

    }


    // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     FIRST AID KIT,  Blanket,        NONE,               Knife
    // Demetrius Inventory: BOOK,           Tent,           Dog Crate,          Rubber Chicken
    // Carlos Inventory:    RADIO,          Can Opener,     NONE,               Whistle
    // Annette Inventory:   Rope,           BLEACH,         Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    Toilet Paper,   Batteries,      Epi Pen,            Wrench
    // Player Inventory:    LEASH           TARP            PLYWOOD             None            None
    [UnityTest]
    public IEnumerator GoToAnnetteToTradeDogLeash()
    {
        SceneManager.LoadScene("PioneerCourthouseSquare");
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        dialogueManager = referenceManager.dialogueCanvas.GetComponent<DialogueManager>();
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        
        GlobalItemList.UpdateItemList("Leash", "Inventory", new Vector3(0,0,0), "Player");
        GlobalItemList.UpdateItemList("Tarp", "Inventory", new Vector3(1,0,0), "Player");
        GlobalItemList.UpdateItemList("Plywood", "Inventory", new Vector3(2,0,0), "Player");

        GlobalItemList.UpdateItemList("Rope", "Inventory", new Vector3(0,0,0), "Annette");
        GlobalItemList.UpdateItemList("Bleach", "Inventory", new Vector3(1,0,0), "Annette");
        GlobalItemList.UpdateItemList("Fire Extinguisher", "Inventory", new Vector3(2,0,0), "Annette");
        GlobalItemList.UpdateItemList("N95 Mask", "Inventory", new Vector3(3,0,0), "Annette");

        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("PioneerCourthouseSquare");
        GlobalControls.ResetNPCInteracted();
        yield return new WaitForSeconds(0.5f);
        yield return MakesMovesToGoToAnnetteToTradeDogLeash();
    }
    public IEnumerator MakesMovesToGoToAnnetteToTradeDogLeash()
    {
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        dialogueManager = referenceManager.dialogueCanvas.GetComponent<DialogueManager>();

        yield return QuokeTestUtils.Press("dddddd", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Annette", GlobalControls.CurrentNPC);
        dialogueManager.currentNode = dialogueManager.forest["basic_annette_1.0"];
        yield return QuokeTestUtils.Press("    <<< < ~``", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Bleach(Clone)", inventory.items[0].name);
        Assert.AreEqual("Tarp(Clone)", inventory.items[1].name);
        Assert.AreEqual("Plywood(Clone)", inventory.items[2].name);
        Assert.AreEqual("Rope(Clone)", inventory.items[3].name);
        
        yield return QuokeTestUtils.Press("aaassssssssssss", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        yield return QuokeTestUtils.Press(">> ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("WaterfrontPark", SceneManager.GetActiveScene().name);

    }
    
    

    // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     FIRST AID KIT,  Blanket,        NONE,               Knife
    // Demetrius Inventory: BOOK,           Tent,           Dog Crate,          Rubber Chicken
    // Carlos Inventory:    RADIO,          Can Opener,     NONE,               Whistle
    // Annette Inventory:   LEASH,          NONE,           Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    Toilet Paper,   Batteries,      Epi Pen,            Wrench
    // Player Inventory:    BLEACH          TARP            PLYWOOD             ROPE            None
    [UnityTest]
    public IEnumerator GoToDemToTradeBleachForTent()
    {
        SceneManager.LoadScene("WaterfrontPark");
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();

        GlobalItemList.UpdateItemList("Bleach", "Inventory", new Vector3(0,0,0), "Player");
        GlobalItemList.UpdateItemList("Tarp", "Inventory", new Vector3(1,0,0), "Player");
        GlobalItemList.UpdateItemList("Plywood", "Inventory", new Vector3(2,0,0), "Player");
        GlobalItemList.UpdateItemList("Rope", "Inventory", new Vector3(3,0,0), "Player");
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("WaterfrontPark");
        GlobalControls.ResetNPCInteracted();
        yield return MakesMovesToGoToDemToTradeBleachForTent();
    }
    public IEnumerator MakesMovesToGoToDemToTradeBleachForTent()
    {
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        dialogueManager = referenceManager.dialogueCanvas.GetComponent<DialogueManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("wwwwwaaaaaaaaaaaaaaaa", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Dem", GlobalControls.CurrentNPC);
        dialogueManager.currentNode = dialogueManager.forest["basic_dem_2.0"];
        yield return QuokeTestUtils.Press("   <<< ~``", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Tent(Clone)", inventory.items[0].name);
        Assert.AreEqual("Tarp(Clone)", inventory.items[1].name);
        Assert.AreEqual("Plywood(Clone)", inventory.items[2].name);
        Assert.AreEqual("Rope(Clone)", inventory.items[3].name);
        
        yield return QuokeTestUtils.Press("dddwwwwwwwwwwwwwwwwww", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        yield return QuokeTestUtils.Press("<< ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("PioneerCourthouseSquare", SceneManager.GetActiveScene().name);

    }
    
    
    // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     FIRST AID KIT,  Blanket,        NONE,               Knife
    // Demetrius Inventory: BOOK,           BLEACH,         Dog Crate,          Rubber Chicken
    // Carlos Inventory:    RADIO,          Can Opener,     NONE,               Whistle
    // Annette Inventory:   LEASH,          NONE,           Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    Toilet Paper,   Batteries,      Epi Pen,            Wrench
    // Player Inventory:    TENT            TARP            PLYWOOD             ROPE            None
    [UnityTest]
    public IEnumerator GoToRainerToTradeTent()
    {
        SceneManager.LoadScene("PioneerCourthouseSquare");
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();

        GlobalItemList.UpdateItemList("Tent", "Inventory", new Vector3(0,0,0), "Player");
        GlobalItemList.UpdateItemList("Tarp", "Inventory", new Vector3(1,0,0), "Player");
        GlobalItemList.UpdateItemList("Plywood", "Inventory", new Vector3(2,0,0), "Player");
        GlobalItemList.UpdateItemList("Rope", "Inventory", new Vector3(3,0,0), "Player");
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("WaterfrontPark");
        GlobalControls.ResetNPCInteracted();
        yield return MakesMovesToGoToRainerToTradeTent();
    }
    public IEnumerator MakesMovesToGoToRainerToTradeTent()
    {
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("sssaaaaaaaaa", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Rainer", GlobalControls.CurrentNPC);
        yield return QuokeTestUtils.Press("     < <<< ~``", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Wrench(Clone)", inventory.items[0].name);
        Assert.AreEqual("Tarp(Clone)", inventory.items[1].name);
        Assert.AreEqual("Plywood(Clone)", inventory.items[2].name);
        Assert.AreEqual("Rope(Clone)", inventory.items[3].name);
        Assert.AreEqual("Toilet Paper(Clone)", inventory.items[4].name);

        yield return QuokeTestUtils.Press("ssssssssds", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        yield return QuokeTestUtils.Press(">> ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("WaterfrontPark", SceneManager.GetActiveScene().name);

    }
    
    
    // Safi Inventory:      Shovel,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     FIRST AID KIT,  Blanket,        NONE,               Knife
    // Demetrius Inventory: BOOK,           BLEACH,         Dog Crate,          Rubber Chicken
    // Carlos Inventory:    RADIO,          Can Opener,     NONE,               Whistle
    // Annette Inventory:   LEASH,          NONE,           Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    TENT,           Batteries,      Epi Pen,            NONE
    // Player Inventory:    WRENCH          TARP            PLYWOOD             ROPE            TOILET PAPER
    [UnityTest]
    public IEnumerator GoToSafiToCompleteActions()
    {
        SceneManager.LoadScene("WaterfrontPark");
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();

        GlobalItemList.UpdateItemList("Wrench", "Inventory", new Vector3(0,0,0), "Player");
        GlobalItemList.UpdateItemList("Tarp", "Inventory", new Vector3(1,0,0), "Player");
        GlobalItemList.UpdateItemList("Plywood", "Inventory", new Vector3(2,0,0), "Player");
        GlobalItemList.UpdateItemList("Rope", "Inventory", new Vector3(3,0,0), "Player");
        GlobalItemList.UpdateItemList("Toilet Paper", "Inventory", new Vector3(4,0,0), "Player");        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("WaterfrontPark");
        GlobalControls.ResetNPCInteracted();
        yield return MakesMovesToGoToSafiToCompleteActions();
    }
    public IEnumerator MakesMovesToGoToSafiToCompleteActions()
    {
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        waterHeaterOrGasValve = GameObject.Find("Interactables").GetComponentInChildren<WaterHeaterOrGasValve>();

        yield return QuokeTestUtils.Press("wwwwwddddd", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Safi", GlobalControls.CurrentNPC);
        yield return QuokeTestUtils.Press("      ", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        yield return QuokeTestUtils.Press("1ssd", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        waterHeaterOrGasValve.UpdateInteractables("Gas", gasValve);
        yield return QuokeTestUtils.Press("wwd", playerKeyboard);

        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Safi", GlobalControls.CurrentNPC);
        yield return QuokeTestUtils.Press("   ", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        yield return QuokeTestUtils.Press("1ssssd ", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        waterHeaterOrGasValve.UpdateInteractables("Heater", waterHeater);
        yield return QuokeTestUtils.Press("wwwwd", playerKeyboard);

        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Safi", GlobalControls.CurrentNPC);
        yield return QuokeTestUtils.Press("    <<<< ~``", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        
        Assert.AreEqual("Shovel(Clone)", inventory.items[0].name);
        Assert.AreEqual("Tarp(Clone)", inventory.items[1].name);
        Assert.AreEqual("Plywood(Clone)", inventory.items[2].name);
        Assert.AreEqual("Rope(Clone)", inventory.items[3].name);
        Assert.AreEqual("Toilet Paper(Clone)", inventory.items[4].name);

        yield return QuokeTestUtils.Press("wwwwwwwwwwwwaaaawww", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        yield return QuokeTestUtils.Press("> ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual("Yard", SceneManager.GetActiveScene().name);

    }

    // Safi Inventory:      WRENCH,         Canned Food,    Gloves,             Playing Cards
    // Angie Inventory:     FIRST AID KIT,  Blanket,        NONE,               Knife
    // Demetrius Inventory: BOOK,           BLEACH,         Dog Crate,          Rubber Chicken
    // Carlos Inventory:    RADIO,          Can Opener,     NONE,               Whistle
    // Annette Inventory:   LEASH,          NONE,           Fire Extinguisher,  N95 Mask
    // Rainer Inventory:    TENT,           Batteries,      Epi Pen,            NONE
    // Player Inventory:    SHOVEL          TARP            PLYWOOD             ROPE            TOILET PAPER
    [UnityTest]
    public IEnumerator GoToCompleteLatrine()
    {
        SceneManager.LoadScene("Yard");
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();

        GlobalItemList.UpdateItemList("Water Bottle Clean", "Yard", new Vector3(0,0,0), "Water Purifying Table");

        GlobalItemList.UpdateItemList("Shovel", "Inventory", new Vector3(0,0,0), "Player");
        GlobalItemList.UpdateItemList("Tarp", "Inventory", new Vector3(1,0,0), "Player");
        GlobalItemList.UpdateItemList("Plywood", "Inventory", new Vector3(2,0,0), "Player");
        GlobalItemList.UpdateItemList("Rope", "Inventory", new Vector3(3,0,0), "Player");
        GlobalItemList.UpdateItemList("Toilet Paper", "Inventory", new Vector3(4,0,0), "Player");        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("Yard");
        GlobalControls.ResetNPCInteracted();
        GlobalControls.globalControlsProperties.Remove("poopTaskCompleted");
        yield return MakesMovesToGoToCompleteLatrine();
    }
    public IEnumerator MakesMovesToGoToCompleteLatrine()
    {
        yield return new WaitForSeconds(0.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("wwaaaaawa  >> > << >>> ", playerKeyboard);
        yield return new WaitForSeconds(0.5f);
        yield return QuokeTestUtils.Press("cccccccc", playerKeyboard);
        Assert.AreEqual(GlobalControls.globalControlsProperties.Contains("poopTaskCompleted"),
            true);

    }
    
    
    
    [UnityTest, Timeout(300000)]
    public IEnumerator CompletesPlaythrough()
    {
        SceneManager.LoadScene("PreQuakeHouse");
        yield return MakesMovesToGetToQuake();
        yield return MakesMovesToGetOutsideAfterQuake();
        yield return MakesMovesToPickUpBookAndLeaveYard();
        yield return MakesMovesToTradeBleach();
        yield return MakesMovesToPurifyWater();
        yield return MakesMovesToGoToAnnetteToTradeBleach();
        yield return MakesMovesToGoToAngieAndCarlosToTrade();
        yield return MakesMovesToGoToAnnetteToTradeDogLeash();
        yield return MakesMovesToGoToDemToTradeBleachForTent();
        yield return MakesMovesToGoToRainerToTradeTent();
        yield return MakesMovesToGoToSafiToCompleteActions();
        yield return MakesMovesToGoToCompleteLatrine();
    }
}

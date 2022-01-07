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
    private ItemLoader itemLoader;
    private PlayerKeyboardManager playerKeyboard;
    private StrategicMapKeyboardController strategicMapKeyboard;
    private CheatKeyboardController cheatKeyboard;

    
    // A Test behaves as an ordinary method
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("PreQuakeHouse"); 
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        cheatKeyboard = referenceManager.keyboardManager.GetComponent<CheatKeyboardController>();
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

    [UnityTest]
    public IEnumerator GetsToQuake()
    {
        SceneManager.LoadScene("PreQuakeHouse");
        yield return new WaitForSeconds(1.5f);
        GlobalControls.ResetNPCInteracted();
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        GlobalControls.ResetNPCInteracted();
        yield return MakesMovesToGetToQuake();
    }
    public IEnumerator MakesMovesToGetToQuake()
    {
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();

        yield return QuokeTestUtils.Press("aaawwaaaaaaadddssssssss aads ", playerKeyboard);
        Assert.AreEqual("QuakeHouse", SceneManager.GetActiveScene().name);
    }

    
    [UnityTest]
    public IEnumerator GetsOutsideAfterQuake()
    {
        SceneManager.LoadScene("QuakeHouse");
        GlobalControls.ResetNPCInteracted();
        yield return MakesMovesToGetOutsideAfterQuake();
    }
    public IEnumerator MakesMovesToGetOutsideAfterQuake()
    {
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
        
        gameStateManager.SetExploring();
        yield return QuokeTestUtils.Press("cwwwwwwwwda", playerKeyboard);
        yield return new WaitForSeconds(5f);
        yield return QuokeTestUtils.Press("aasssaa", playerKeyboard);
        Assert.AreEqual("Yard", SceneManager.GetActiveScene().name);
    }
    
    
    [UnityTest]
    public IEnumerator PicksUpBookAndLeavesYard()
    {
        SceneManager.LoadScene("Yard");
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        GlobalItemList.UpdateItemList("Book", "Yard", new Vector3(-3.5f,1.5f,-8.5f), "Shed 1");
        GlobalItemList.UpdateItemList("Dirty Water Bottle", "Yard", new Vector3(-2.5f,1.5f,-8.5f), "Shed 2");
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("Yard");
        GlobalControls.ResetNPCInteracted();

        yield return MakesMovesToPickUpBookAndLeaveYard();
    }
    public IEnumerator MakesMovesToPickUpBookAndLeaveYard()
    {
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("aaassssssss ", playerKeyboard);
        Assert.AreEqual("Book(Clone)", inventory.items[0].name);
        yield return QuokeTestUtils.Press("wwwwwwwwwwwwwwww", playerKeyboard);
        Assert.AreEqual("StrategicMap", SceneManager.GetActiveScene().name);
        
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();

        yield return new WaitForSeconds(1.5f);
        yield return QuokeTestUtils.Press("< ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("WaterfrontPark", SceneManager.GetActiveScene().name);
    }
    
    
    [UnityTest]
    public IEnumerator TradesBleach()
    {
        SceneManager.LoadScene("WaterfrontPark");
        yield return new WaitForSeconds(1.5f);
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
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();

        yield return QuokeTestUtils.Press("wwwwwaaaaaaaaaaaaaa", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
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
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("StrategicMap", SceneManager.GetActiveScene().name);
        
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();

        yield return new WaitForSeconds(1.5f);
        yield return QuokeTestUtils.Press("> ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("Yard", SceneManager.GetActiveScene().name);
        
    }


    [UnityTest]
    public IEnumerator PurifiesWater()
    {
        SceneManager.LoadScene("Yard");
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();

        GlobalItemList.UpdateItemList("Bleach", "Inventory", new Vector3(0,0,0), "Player");
        GlobalItemList.UpdateItemList("Dirty Water Bottle", "Yard", new Vector3(-2.5f,1.5f,-8.5f), "Shed 2");
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("Yard");
        GlobalControls.ResetNPCInteracted();
        yield return MakesMovesToPurifyWater();
    }
    public IEnumerator MakesMovesToPurifyWater()
    {
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("aassssssss ", playerKeyboard);
        Assert.AreEqual("Dirty Water Bottle(Clone)", inventory.items[1].name);
        yield return QuokeTestUtils.Press("wwwwwwwwaaaaaaa", playerKeyboard);
        yield return QuokeTestUtils.Press("> < ", playerKeyboard);
        Assert.AreEqual(GlobalControls.globalControlsProperties.Contains("waterTaskCompleted"),
            true);
        yield return QuokeTestUtils.Press("dwwwwwww", playerKeyboard);
        Assert.AreEqual("StrategicMap", SceneManager.GetActiveScene().name);

        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        yield return QuokeTestUtils.Press("> ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("PioneerCourthouseSquare", SceneManager.GetActiveScene().name);

    }


    
    [UnityTest]
    public IEnumerator GoToAnnetteToTradeBleach()
    {
        SceneManager.LoadScene("PioneerCourthouseSquare");
        yield return new WaitForSeconds(1.5f);
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
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("dddddd", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("Annette", GlobalControls.CurrentNPC);
        yield return QuokeTestUtils.Press("                   d>   <<< ~``", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("First Aid Kit(Clone)", inventory.items[0].name);
        yield return QuokeTestUtils.Press("aaassssssssssss", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        yield return QuokeTestUtils.Press("> ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("PSU", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator GoToAngieAndCarlosToTrade()
    {
        SceneManager.LoadScene("PSU");
        yield return new WaitForSeconds(1.5f);
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
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("wwwwwwwaaaaaaaa", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("Angie", GlobalControls.CurrentNPC);
        yield return QuokeTestUtils.Press("        <<< < ~``", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("Radio(Clone)", inventory.items[0].name);
        Assert.AreEqual("Tarp(Clone)", inventory.items[1].name);
        
        yield return QuokeTestUtils.Press("ssssssssssssdsssssssssssssssssaa", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("Carlos", GlobalControls.CurrentNPC);
        yield return QuokeTestUtils.Press("           << << ~``", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("Leash(Clone)", inventory.items[0].name);
        Assert.AreEqual("Tarp(Clone)", inventory.items[1].name);
        Assert.AreEqual("Plywood(Clone)", inventory.items[2].name);
        yield return QuokeTestUtils.Press("ddsss", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        yield return QuokeTestUtils.Press("< ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("PioneerCourthouseSquare", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator GoToAnnetteToTradeDogLeash()
    {
        SceneManager.LoadScene("PioneerCourthouseSquare");
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();

        GlobalItemList.UpdateItemList("Leash", "Inventory", new Vector3(0,0,0), "Player");
        GlobalItemList.UpdateItemList("Tarp", "Inventory", new Vector3(1,0,0), "Player");
        GlobalItemList.UpdateItemList("Plywood", "Inventory", new Vector3(2,0,0), "Player");
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
        itemLoader.LoadItems("PioneerCourthouseSquare");
        GlobalControls.ResetNPCInteracted();
        yield return MakesMovesToGoToAnnetteToTradeDogLeash();
    }
    public IEnumerator MakesMovesToGoToAnnetteToTradeDogLeash()
    {
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();

        yield return QuokeTestUtils.Press("dddddd", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("Annette", GlobalControls.CurrentNPC);
        yield return QuokeTestUtils.Press("    <<< < ~``", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("Bleach(Clone)", inventory.items[0].name);
        Assert.AreEqual("Tarp(Clone)", inventory.items[1].name);
        Assert.AreEqual("Plywood(Clone)", inventory.items[2].name);
        Assert.AreEqual("Rope(Clone)", inventory.items[3].name);
        
        yield return QuokeTestUtils.Press("aaassssssssssss", playerKeyboard);
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        yield return QuokeTestUtils.Press(">> ", playerKeyboard, null, strategicMapKeyboard);
        yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("WaterfrontPark", SceneManager.GetActiveScene().name);

    }
    
    

    
    
    
    [UnityTest]
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
    }
}

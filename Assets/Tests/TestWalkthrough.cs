using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;


public class TestWalkthrough
{
    private ReferenceManager referenceManager;
    private PlayerKeyboardManager playerKeyboard;
    private GameStateManager gameStateManager;
    private ItemLoader itemLoader;
    
    // A Test behaves as an ordinary method
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("PreQuakeHouse"); 
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
        itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
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
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
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
    }


    [UnityTest]
    public IEnumerator CompletesPlaythrough()
    {
        SceneManager.LoadScene("PreQuakeHouse");
        yield return MakesMovesToGetToQuake();
        yield return MakesMovesToGetOutsideAfterQuake();
        yield return MakesMovesToPickUpBookAndLeaveYard();
    }
}

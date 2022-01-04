using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class TestPreQuakeHouse
{
    private ReferenceManager referenceManager;
    private PlayerKeyboardManager playerKeyboard;
    private CheatKeyboardController cheatKeyboard;
    private StrategicMapKeyboardController strategicMapKeyboard;
    
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("PreQuakeHouse"); 
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        cheatKeyboard = referenceManager.keyboardManager.GetComponent<CheatKeyboardController>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        playerKeyboard.virtualKeyboard = true;
        cheatKeyboard.virtualKeyboard = true;
        strategicMapKeyboard.virtualKeyboard = true;
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        // Destroy all objects
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(obj);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator Cabinet1IsInitiallyEmpty()
    {
        GameObject cabinet = GameObject.Find("Cabinet 1");
        Assert.Null(cabinet.GetComponent<StorageContainer>().contents);
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator PicksUpAnItem()
    {
        // Find some objects
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        GameObject sunscreen = GameObject.Find("Sunscreen(Clone)");
        Assert.NotNull(sunscreen);
        // Take some steps
        yield return QuokeTestUtils.Press("wwdd", playerKeyboard, cheatKeyboard);
        // Verify that the sunscreen was picked up
        Assert.AreEqual(sunscreen, inventory.items[0]);
    }
    
    [UnityTest]
    public IEnumerator VirtualPlayerMoves()
    {
        yield return QuokeTestUtils.Press("w", playerKeyboard, cheatKeyboard);
        Assert.AreEqual(new Vector3(4.5f,0.5f,-0.5f), referenceManager.player.transform.position);
        yield return QuokeTestUtils.Press("s", playerKeyboard, cheatKeyboard);
        Assert.AreEqual(new Vector3(4.5f,0.5f,-1.5f), referenceManager.player.transform.position);
        yield return QuokeTestUtils.Press("a", playerKeyboard, cheatKeyboard);
        Assert.AreEqual(new Vector3(3.5f,0.5f,-1.5f), referenceManager.player.transform.position);
        yield return QuokeTestUtils.Press("d", playerKeyboard, cheatKeyboard);
        Assert.AreEqual(new Vector3(4.5f,0.5f,-1.5f), referenceManager.player.transform.position);
    }
    
    [UnityTest]
    public IEnumerator CheatKeyboardWorks()
    {
        yield return QuokeTestUtils.Press("l", playerKeyboard, cheatKeyboard);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        Assert.AreEqual("Shovel(Clone)", inventory.items[0].name);
        Assert.AreEqual("Plywood(Clone)", inventory.items[1].name);
    }
    
}

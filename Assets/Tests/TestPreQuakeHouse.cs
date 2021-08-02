using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
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
        
        yield return null;
        
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        referenceManager.keyboardManager.GetComponent<CheatKeyboardController>().SetKeyDown(KeyCode.N);
        
        yield return new WaitForSeconds(1.5f);

        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        cheatKeyboard = referenceManager.keyboardManager.GetComponent<CheatKeyboardController>();
        strategicMapKeyboard = referenceManager.keyboardManager.GetComponent<StrategicMapKeyboardController>();
        playerKeyboard.virtualKeyboard = true;
        cheatKeyboard.virtualKeyboard = true;
        strategicMapKeyboard.virtualKeyboard = true;
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

    /// <summary>
    /// Presses each of the indicated keys, with a pause after each one to allow time to move to the next grid space.
    /// </summary>
    private IEnumerator SimulateKeyboardInput(string keys)
    {
        foreach (char c in keys)
        {
            KeyCode k = (KeyCode) Enum.Parse(typeof(KeyCode), Char.ToUpper(c).ToString());
            playerKeyboard.SetKeyDown(k);
            yield return new WaitForSeconds(0.5f);
        }
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
        PlayerMover player = referenceManager.player.GetComponent<PlayerMover>();
        GameObject sunscreen = GameObject.Find("Sunscreen(Clone)");
        Assert.NotNull(sunscreen);
        // Take some steps
        yield return SimulateKeyboardInput("wwdd");
        // Verify that the sunscreen was picked up
        Assert.AreEqual(sunscreen, inventory.items[0]);
    }
    
    [UnityTest]
    public IEnumerator VirtualPlayerMoves()
    {
        yield return SimulateKeyboardInput("w");
        Assert.AreEqual(new Vector3(4.5f,0.5f,-0.5f), referenceManager.player.transform.position);
        yield return SimulateKeyboardInput("s");
        Assert.AreEqual(new Vector3(4.5f,0.5f,-1.5f), referenceManager.player.transform.position);
        yield return SimulateKeyboardInput("a");
        Assert.AreEqual(new Vector3(3.5f,0.5f,-1.5f), referenceManager.player.transform.position);
        yield return SimulateKeyboardInput("d");
        Assert.AreEqual(new Vector3(4.5f,0.5f,-1.5f), referenceManager.player.transform.position);
    }

    [UnityTest]
    public IEnumerator CheatKeyboardWorks()
    {
        cheatKeyboard.SetKeyDown(KeyCode.L);
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        Assert.AreEqual("Shovel(Clone)", inventory.items[0].name);
        Assert.AreEqual("Plywood(Clone)", inventory.items[1].name);
    }
    
}

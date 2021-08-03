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
    /// Presses each of the indicated keys, with a pause after each one, if needed, to allow time for the game to
    /// respond.
    /// </summary>
    private IEnumerator Press(string keys)
    {
        foreach (char c in keys)
        {
            KeyCode k = (KeyCode) Enum.Parse(typeof(KeyCode), Char.ToUpper(c).ToString());
            // There are two different objects receiving keypresses, but only one will react to a given key
            playerKeyboard.SetKeyDown(k);
            cheatKeyboard.SetKeyDown(k);
            if ("WASDL".IndexOf(Char.ToUpper(c)) != -1)
            {
                // This key take some time for the game to complete its response (e.g., movement)
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                yield return null;
            }
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
        yield return Press("wwdd");
        // Verify that the sunscreen was picked up
        Assert.AreEqual(sunscreen, inventory.items[0]);
    }
    
    [UnityTest]
    public IEnumerator VirtualPlayerMoves()
    {
        yield return Press("w");
        Assert.AreEqual(new Vector3(4.5f,0.5f,-0.5f), referenceManager.player.transform.position);
        yield return Press("s");
        Assert.AreEqual(new Vector3(4.5f,0.5f,-1.5f), referenceManager.player.transform.position);
        yield return Press("a");
        Assert.AreEqual(new Vector3(3.5f,0.5f,-1.5f), referenceManager.player.transform.position);
        yield return Press("d");
        Assert.AreEqual(new Vector3(4.5f,0.5f,-1.5f), referenceManager.player.transform.position);
    }

    [UnityTest]
    public IEnumerator CheatKeyboardWorks()
    {
        yield return Press("l");
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        Assert.AreEqual("Shovel(Clone)", inventory.items[0].name);
        Assert.AreEqual("Plywood(Clone)", inventory.items[1].name);
    }
    
}

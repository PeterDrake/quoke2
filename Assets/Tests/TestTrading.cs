using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class TestTrading
{
    private ReferenceManager referenceManager;
    private PlayerKeyboardManager playerKeyboard;
    private CheatKeyboardController cheatKeyboard;
    private StrategicMapKeyboardController strategicMapKeyboard;
    
    [UnitySetUp]
    public IEnumerator Setup()
    {
        GlobalControls.Reset();
        GlobalItemList.Reset();
        
        SceneManager.LoadScene("TitleScreen");
        
        yield return null;
        
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        referenceManager.keyboardManager.GetComponent<CheatKeyboardController>().SetKeyDown(KeyCode.Z);
        
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
    
    [UnityTest]
    public IEnumerator TradesItemSuccessfully()
    {
        yield return WalkToAngie();
        // Wait for trading UI
        yield return new WaitForSeconds(0.5f);
        yield return GetToTradeAngie();
        // Trade thing with Angie
        yield return QuokeTestUtils.Press(" < < ~", playerKeyboard, cheatKeyboard);
        // Check that the items were traded successfully 
        Assert.True(referenceManager.tradeCanvas.GetComponent<TradeManagerUI>()
            .inventories[(int)InventoryE.NPC].items[2].name == "First Aid Kit(Clone)");
        Inventory playerInventory = referenceManager.tradeCanvas.GetComponent<TradeManagerUI>()
            .inventories[(int)InventoryE.Player];
        Assert.True(playerInventory.items[0].name == "Knife(Clone)"
            && playerInventory.items[3].name == "Blanket(Clone)");
    }
    
    [UnityTest]
    public IEnumerator UnableToTradeNPCNeededItem()
    {
        yield return WalkToAngie();
        // Wait for trading UI
        yield return new WaitForSeconds(0.5f);
        yield return GetToTradeAngie();
        // Trade thing with Angie
        yield return QuokeTestUtils.Press(" < < ~", playerKeyboard, cheatKeyboard);
        // Check that the items were traded successfully 
        Assert.True(referenceManager.tradeCanvas.GetComponent<TradeManagerUI>()
            .inventories[(int)InventoryE.NPC].items[2].name == "First Aid Kit(Clone)");
        Inventory playerInventory = referenceManager.tradeCanvas.GetComponent<TradeManagerUI>()
            .inventories[(int)InventoryE.Player];
        Assert.True(playerInventory.items[0].name == "Knife(Clone)"
                    && playerInventory.items[3].name == "Blanket(Clone)");
        // Attempt to add the First Aid Kit back to us
        yield return QuokeTestUtils.Press(" ", playerKeyboard, cheatKeyboard);
        // Check to make sure the item was not added to the NPC bin
        Assert.Null(referenceManager.tradeCanvas.GetComponent<TradeManagerUI>()
            .inventories[(int)InventoryE.NPCBin].items[0]);
    }

    [UnityTest]
    public IEnumerator ValidTradesForUnneededItems()
    {
        yield return WalkToAngie();
        // Wait for trading UI
        yield return new WaitForSeconds(0.5f);
        yield return GetToTradeAngie();
        // Try invalid trade with Angie
        yield return QuokeTestUtils.Press(">> <<< < ", playerKeyboard, cheatKeyboard);
        // Check that the items weren't traded successfully
        Inventory playerInventory = referenceManager.tradeCanvas.GetComponent<TradeManagerUI>()
            .inventories[(int)InventoryE.Player];
        Assert.Null(playerInventory.items[2]);
        // Try a valid trade
        yield return QuokeTestUtils.Press("<<<<<  ~", playerKeyboard, cheatKeyboard);
        yield return new WaitForSeconds(0.5f);
        // Check that the items were traded successfully 
        Assert.True(playerInventory.items[2].name == "Knife(Clone)");
    }

    [UnityTest]
    public IEnumerator IOUReceiveAndFunction()
    {
        yield return WalkToAngie();
        // Wait for trading UI
        yield return new WaitForSeconds(0.5f);
        yield return GetToTradeAngie();
        // Trade with Angie
        yield return QuokeTestUtils.Press(" < ~", playerKeyboard, cheatKeyboard);
        yield return new WaitForSeconds(0.5f);
        // Check that the items were traded successfully 
        Inventory playerInventory = referenceManager.tradeCanvas.GetComponent<TradeManagerUI>()
            .inventories[(int)InventoryE.Player];
        Assert.True(playerInventory.items[0].name == "Knife(Clone)");
        yield return new WaitForSeconds(0.5f);
        // Trade using the IOU
        yield return QuokeTestUtils.Press("< ~", playerKeyboard, cheatKeyboard);
        Assert.True(playerInventory.items[3].name == "Blanket(Clone)");
    }

    public IEnumerator WalkToAngie()
    {
        yield return QuokeTestUtils.Press("wwwwwwwaaaaaaaa", playerKeyboard, cheatKeyboard);
    }
    
    /*
     * Gets the player through the first dialogue tree of Angie's so they are able to trade with her.
     */
    public IEnumerator GetToTradeAngie()
    {
        yield return QuokeTestUtils.Press("    >  ", playerKeyboard, cheatKeyboard);
    }
}

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
        SceneManager.LoadScene("TitleScreen");
        
        yield return null;
        
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        referenceManager.keyboardManager.GetComponent<CheatKeyboardController>().SetKeyDown(KeyCode.F);
        
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
        // Walk into Angie
        yield return QuokeTestUtils.Press("wwwwwwwaaaaaaaa", playerKeyboard, cheatKeyboard);
        // Wait for trading UI
        yield return new WaitForSeconds(0.5f);
        // Navigate through dialogue to trading
        yield return QuokeTestUtils.Press("    >  ", playerKeyboard, cheatKeyboard);
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
        // Walk into Angie
        yield return QuokeTestUtils.Press("wwwwwwwaaaaaaaa", playerKeyboard, cheatKeyboard);
        // Wait for trading UI
        yield return new WaitForSeconds(0.5f);
        // Navigate through dialogue to trading
        yield return QuokeTestUtils.Press("    >  ", playerKeyboard, cheatKeyboard);
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
        Assert.True(referenceManager.tradeCanvas.GetComponent<TradeManagerUI>()
            .inventories[(int)InventoryE.NPCBin].items[0] == null);
    }
}

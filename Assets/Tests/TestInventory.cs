using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestInventory
{
    // // A Test behaves as an ordinary method
    // [Test]
    // public void TestInventorySimplePasses()
    // {
    //     // Use the Assert class to test conditions
    // }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator Cabinet1IsInitiallyEmpty
        ()
    {
        GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("PreQuakeHouse Level"));
        GameObject cabinet = GameObject.Find("Cabinet 1");
        yield return null;
        Assert.Null(cabinet.GetComponent<StorageContainer>().contents);
        Object.Destroy(gameObject);
    }

    [UnityTest]
    public IEnumerator PicksUpAnItem()
    {
        GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("PreQuakeHouse Level"));
        ReferenceManager referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        ItemLoader itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        itemLoader.LoadItems("PreQuakeHouse");
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        PlayerMover player = referenceManager.player.GetComponent<PlayerMover>();
        yield return null;  // This seems necessary to 
        GameObject cup = GameObject.Find("Cup(Clone)");
        Assert.NotNull(cup);
        // Take some steps
        for (int i = 0; i < 5; i++)
        {
            player.StartMoving(new Vector3(0f, 0f, 1f));
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < 2; i++)
        {
            player.StartMoving(new Vector3(1f, 0f, 0f));
            yield return new WaitForSeconds(0.5f);
        }
        Debug.Log("In slot 0:" + inventory.items[0].name);
        Assert.AreEqual(cup, inventory.items[0]);
        Object.Destroy(gameObject);
    }
}

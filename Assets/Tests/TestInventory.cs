using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestInventory
{
    private GameObject levelPrefab;

    private ReferenceManager referenceManager;
    
    // [SetUp]
    // public void Setup()
    // {
    //     levelPrefab = MonoBehaviour.Instantiate(Resources.Load<GameObject>("PreQuakeHouse Level"));
    //     referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
    //     // Because the test scene doesn't have the expected name, it's necessary to call LoadItems directly
    //     ItemLoader itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
    //     itemLoader.LoadItems("PreQuakeHouse");
    // }
    //
    // [TearDown]
    // public void Teardown()
    // {
    //     Object.Destroy(levelPrefab);
    // }
    
    [UnityTest]
    public IEnumerator Cabinet1IsInitiallyEmpty
        ()
    {
        // Setup
        levelPrefab = MonoBehaviour.Instantiate(Resources.Load<GameObject>("PreQuakeHouse Level"));
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        // // Because the test scene doesn't have the expected name, it's necessary to call LoadItems directly
        // ItemLoader itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        // itemLoader.LoadItems("PreQuakeHouse");
        GameObject cabinet = GameObject.Find("Cabinet 1");
        Assert.Null(cabinet.GetComponent<StorageContainer>().contents);
        yield return null;
        Object.Destroy(levelPrefab);
    }

    [UnityTest]
    public IEnumerator PicksUpAnItem()
    {
        // Setup
        levelPrefab = MonoBehaviour.Instantiate(Resources.Load<GameObject>("PreQuakeHouse Level"));
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        // Because the test scene doesn't have the expected name, it's necessary to call LoadItems directly
        ItemLoader itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        itemLoader.LoadItems("PreQuakeHouse");
        // Get various objects
        Inventory inventory = referenceManager.inventoryCanvas.GetComponent<Inventory>();
        PlayerMover player = referenceManager.player.GetComponent<PlayerMover>();
        GameObject cup = GameObject.Find("Cup(Clone)");
        Assert.NotNull(cup);
        // Take some steps
        for (int i = 0; i < 5; i++)
        {
            player.StartMoving(Vector3.forward);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < 2; i++)
        {
            player.StartMoving(Vector3.right);
            yield return new WaitForSeconds(0.5f);
        }
        // Verify that the cup was picked up
        Assert.AreEqual(cup, inventory.items[0]);
        Object.Destroy(levelPrefab);
    }
}

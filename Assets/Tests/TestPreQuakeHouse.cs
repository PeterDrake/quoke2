using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestPreQuakeHouse
{
    private GameObject levelPrefab;
    private ReferenceManager referenceManager;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        levelPrefab = MonoBehaviour.Instantiate(Resources.Load<GameObject>("PreQuakeHouse Level"));
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        // Because the test scene doesn't have the expected name, it's necessary to call LoadItems directly
        ItemLoader itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        itemLoader.LoadItems("PreQuakeHouse");
        yield return null;
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
        PlayerMover player = referenceManager.player.GetComponent<PlayerMover>();
        GameObject sunscreen = GameObject.Find("Sunscreen(Clone)");
        Assert.NotNull(sunscreen);
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
        // Verify that the sunscreen was picked up
        Assert.AreEqual(sunscreen, inventory.items[0]);
    }

    
}

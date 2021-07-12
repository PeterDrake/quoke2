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
        // // Because the test scene doesn't have the expected name, it's necessary to call LoadItems directly
        // ItemLoader itemLoader = referenceManager.itemLoader.GetComponent<ItemLoader>();
        // itemLoader.LoadItems("PreQuakeHouse");
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

    
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator Cabinet1IsInitiallyEmpty()
    {
        GameObject cabinet = GameObject.Find("Cabinet 1");
        Assert.Null(cabinet.GetComponent<StorageContainer>().contents);
       
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
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
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
        Assert.Null(cabinet.GetComponent<StorageContainer>().contents);
        Object.Destroy(gameObject);
    }
}

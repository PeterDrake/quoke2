using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;


public class TestWalkthrough
{
    private ReferenceManager referenceManager;
    private PlayerKeyboardManager playerKeyboard;
    
    // A Test behaves as an ordinary method
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("PreQuakeHouse"); 
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        GameObject.Find("Inventory Canvas").GetComponent<Inventory>().Clear();
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // Destroy all objects
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(obj);
        }
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator GetsToQuake()
    {
        yield return QuokeTestUtils.Press("aaawwaaaaaaadddssssssss aads ", playerKeyboard);
        // yield return new WaitForSeconds(1.5f);
        Assert.AreEqual("QuakeHouse", SceneManager.GetActiveScene().name);
    }
}

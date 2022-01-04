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
    private GameStateManager gameStateManager;
    
    // A Test behaves as an ordinary method
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("PreQuakeHouse"); 
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
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
        SceneManager.LoadScene("PreQuakeHouse");
        yield return MakesMovesToGetToQuake();
    }
    public IEnumerator MakesMovesToGetToQuake()
    {
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();

        yield return QuokeTestUtils.Press("aaawwaaaaaaadddssssssss aads ", playerKeyboard);
        Assert.AreEqual("QuakeHouse", SceneManager.GetActiveScene().name);
    }

    
    [UnityTest]
    public IEnumerator GetsOutsideAfterQuake()
    {
        SceneManager.LoadScene("QuakeHouse");
        yield return MakesMovesToGetOutsideAfterQuake();
    }
    public IEnumerator MakesMovesToGetOutsideAfterQuake()
    {
        yield return new WaitForSeconds(1.5f);
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        playerKeyboard = referenceManager.keyboardManager.GetComponent<PlayerKeyboardManager>();
        gameStateManager = referenceManager.gameStateManager.GetComponent<GameStateManager>();
        
        gameStateManager.SetExploring();
        yield return QuokeTestUtils.Press("cwwwwwwwwda", playerKeyboard);
        yield return new WaitForSeconds(5f);
        yield return QuokeTestUtils.Press("aasssaa", playerKeyboard);
        Assert.AreEqual("Yard", SceneManager.GetActiveScene().name);
    }


    [UnityTest]
    public IEnumerator CompletesPlaythrough()
    {
        SceneManager.LoadScene("PreQuakeHouse");
        yield return MakesMovesToGetToQuake();
        yield return MakesMovesToGetOutsideAfterQuake();
    }
}

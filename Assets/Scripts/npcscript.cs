using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class npcscript : MonoBehaviour
{
    /// List of the Scenes 
    private List<string> sceneHistory = new List<string>();  //running history of scenes
    //The last string in the list is always the current scene running

    private int eventsLoaded;

    /// Starts the Scene in every Load and keeps the gameObject alive
    void Start()
    {
        sceneHistory.Add(SceneManager.GetActiveScene().name);

    }

    
    /// Update brings back the previous scene when player presses the escape key
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PreviousScene();
            eventsLoaded = 0;
        }
        
        //TODO: Fix this if else statement so it actually stops the player from moving.
        // Freezes the background if the player enters NPC screen
        if (sceneHistory.Count < 2)
        {
            GameObject.Find("Player").GetComponent<PlayerMover>().enabled = true;   
        }
        else
        {
            GameObject.Find("Player").GetComponent<PlayerMover>().enabled = false;
        }
    }
    
    public void LoadScene(string newScene)
    {
        
        if (eventsLoaded <= 0)
        {
            eventsLoaded++;
            sceneHistory.Add(newScene);
            SceneManager.LoadScene(newScene, LoadSceneMode.Additive); //Additive: Loads the scene on the top of previous scene
        }
    }
 
    ///Call this whenever you want to load the previous scene
    ///It will remove the current scene from the history and then load the new last scene in the history
    ///It will return false if we have not moved between scenes enough to have stored a previous scene in the history
    public bool PreviousScene()
    {
        bool returnValue = false;
        if (sceneHistory.Count >= 2)  //Checking that we have actually switched scenes enough to go back to a previous scene
        {
            returnValue = true;
            sceneHistory.RemoveAt(sceneHistory.Count -1);
            SceneManager.UnloadSceneAsync("npcScreen");  // Reloads the game where the player entered the new Scene
        }
        return returnValue;
    }
    
    
    

}

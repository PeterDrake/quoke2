using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class npcscript : MonoBehaviour
{
    /// <summary>
    /// List of the Scenes 
    /// </summary>
    private List<string> sceneHistory = new List<string>();  //running history of scenes
    //The last string in the list is always the current scene running

    
    /// <summary>
    /// Starts the Scene in ever Load and keeps the gameObject alive
    /// </summary>
    void Start()
    {
        sceneHistory.Add(SceneManager.GetActiveScene().name);

    }

    /// On Collision with the gameObject we Load new Scene
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other )
    {
        if (other.gameObject.tag == "Player")
        {
            GlobalControls.CurrentNPC = this.gameObject.name;
            LoadScene("npcScreen");
        }
       
    }
 
    /// Update brings back the previous scene when player presses the escape key
    /// Freezes the background if the player enters NPC screen
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PreviousScene();
        }
        
        if (sceneHistory.Count < 2)
        {
            GameObject.Find("Player").GetComponent<MovementRevised>().enabled = true;   
        }
        else
        {
            GameObject.Find("Player").GetComponent<MovementRevised>().enabled = false;
        }
    }
    
    
    /// Load newScene and add it to sceneHistory
    /// <param name="newScene"></param>
    
    public void LoadScene(string newScene)
    {
        sceneHistory.Add(newScene);
        SceneManager.LoadScene(newScene,LoadSceneMode.Additive);
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

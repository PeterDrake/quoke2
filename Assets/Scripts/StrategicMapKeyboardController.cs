using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// controls the map marker for the strategic map
/// </summary>
public class StrategicMapKeyboardController : MonoBehaviour
{
    public GameObject player;
    
    // Set size of and add locations in the Inspector
    public GameObject[] locations;
    public int playerLocation;
    private KeyCode keyDown = KeyCode.JoystickButton0;
    public bool virtualKeyboard;

    private void Start()
    {
        if (!GlobalControls.globalControlsProperties.Contains("apartmentCondition"))
        {
            Transform[] children = GameObject.Find("Locations").GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if(child.gameObject.name.Equals("Street Marker")) child.gameObject.SetActive(false);
                else if(child.gameObject.name.Equals("Street Text")) child.gameObject.SetActive(false);
                else if(child.gameObject.name.Equals("Yard Marker")) child.gameObject.SetActive(true);
                else if(child.gameObject.name.Equals("Yard Text")) child.gameObject.SetActive(true);
            }
        }
        else
        {
            Transform[] children = GameObject.Find("Locations").GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if(child.gameObject.name.Equals("Yard Marker")) child.gameObject.SetActive(false);
                else if(child.gameObject.name.Equals("Yard Text")) child.gameObject.SetActive(false);
                else if(child.gameObject.name.Equals("Street Marker")) child.gameObject.SetActive(true);
                else if(child.gameObject.name.Equals("Street Text")) child.gameObject.SetActive(true);
            }
        }
        
        // Set the player location to the scene we just left
        playerLocation = GlobalControls.currentScene;
        if (GlobalControls.currentScene < 0 || GlobalControls.currentScene > 3)
        {
            playerLocation = 0;
        }

        ReferenceManager references = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        if (!GlobalControls.globalControlsProperties.Contains("apartmentCondition"))
        {
            locations = new []
            {
                GameObject.Find("PSU Marker"), GameObject.Find("Waterfront Park Marker"), GameObject.Find("Yard Marker"), GameObject.Find("Pioneer Courthouse Square Marker")
            };
        }
        else
        {
            locations = new []
            {
                GameObject.Find("PSU Marker"), GameObject.Find("Waterfront Park Marker"), GameObject.Find("Street Marker"), GameObject.Find("Pioneer Courthouse Square Marker")
            };
        }
        
        player = references.player;
        references.deathCanvas.SetActive(false);
        references.segueCanvas.SetActive(false);
        references.inventoryController.DisableUI();
        references.dialogueCanvas.SetActive(false);
        references.tradeCanvas.SetActive(false);
        references.npcInteractedController.DisableUI();
        if (GlobalControls.globalControlsProperties.Contains("keybindsEnabled"))
        {
            references.keybinds.SetActive(true);
            references.keybinds.GetComponentInChildren<Text>().text = GlobalControls.keybinds["StrategicMap"];
        }
        else if (GlobalControls.globalControlsProperties.Contains("keybindsEnabled")) references.keybinds.SetActive(false);

        GameObject.Find("NPC Inventory").SetActive(false);
    }
    
    /// <summary>
    /// SetKeyDown sets a specific key to be pressed for one update frame. Pass KeyCode.JoystickButton0 for none
    /// </summary>
    /// <param name="pressed"></param>
    public void SetKeyDown(KeyCode pressed)
    {
        keyDown = pressed;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!virtualKeyboard)
        {
            if (Input.GetKeyDown(KeyCode.Space)) keyDown = KeyCode.Space;
            else if (Input.GetKeyDown(KeyCode.Escape)) keyDown = KeyCode.Escape;
            else if (Input.GetKeyDown(KeyCode.Comma)) keyDown = KeyCode.Comma;
            else if (Input.GetKeyDown(KeyCode.Period)) keyDown = KeyCode.Period;
        }
        
        
        // Change the player's location with < and >
        if (keyDown.Equals(KeyCode.Comma))  // Move left with < key
        {
            playerLocation = (playerLocation - 1 + locations.Length) % locations.Length;
        }
        if (keyDown.Equals(KeyCode.Period))  // Move right with > key
        {
            playerLocation = (playerLocation + 1) % locations.Length;
        }
        if (keyDown.Equals(KeyCode.Space))
        {
            GlobalControls.globalControlsProperties.Remove("isStrategicMap");
            SceneManagement sceneManagement = GameObject.Find("Managers").GetComponent<ReferenceManager>().sceneManagement.GetComponent<SceneManagement>();
            sceneManagement.ChangeScene(locations[playerLocation].GetComponent<MapMarker>().mapName);
        }
        // Move the player on top of the marker for the location they are currently at
        player.transform.position = locations[playerLocation].transform.position + Vector3.up;
        
        keyDown = KeyCode.JoystickButton0;
    }
}

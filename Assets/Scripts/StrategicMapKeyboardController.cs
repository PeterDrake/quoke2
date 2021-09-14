using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        playerLocation = GlobalControls.CurrentScene;
        if (GlobalControls.CurrentScene < 0 || GlobalControls.CurrentScene > 3)
        {
            playerLocation = 0;
        }

        ReferenceManager references = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        if (!GlobalControls.globalControlsProperties.Contains("apartmentCondition"))
        {
            locations = new []
            {
                GameObject.Find("School Marker"), GameObject.Find("Park Marker"), GameObject.Find("Yard Marker"), GameObject.Find("Garden Marker")
            };
        }
        else
        {
            locations = new []
            {
                GameObject.Find("School Marker"), GameObject.Find("Park Marker"), GameObject.Find("Street Marker"), GameObject.Find("Garden Marker")
            };
        }
        
        player = references.player;
        references.deathCanvas.SetActive(false);
        references.segueCanvas.SetActive(false);
        references.inventoryCanvas.SetActive(false);
        references.dialogueCanvas.SetActive(false);
        references.tradeCanvas.SetActive(false);
        references.npcInteractedCanvas.SetActive(false);
        if (GlobalControls.KeybindsEnabled)
        {
            references.keybinds.SetActive(true);
            references.keybinds.GetComponentInChildren<Text>().text = GlobalControls.Keybinds["StrategicMap"];
        }
        else if (GlobalControls.KeybindsEnabled) references.keybinds.SetActive(false);

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
            GlobalControls.IsStrategicMap = false;
            SceneManagement sceneManagement = GameObject.Find("Managers").GetComponent<ReferenceManager>().sceneManagement.GetComponent<SceneManagement>();
            sceneManagement.ChangeScene(locations[playerLocation].GetComponent<MapMarker>().mapName);
        }
        // Move the player on top of the marker for the location they are currently at
        player.transform.position = locations[playerLocation].transform.position + Vector3.up;
        
        keyDown = KeyCode.JoystickButton0;
    }
}

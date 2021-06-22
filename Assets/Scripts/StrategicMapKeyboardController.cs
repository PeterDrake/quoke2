using UnityEngine;
using UnityEngine.SceneManagement;

public class StrategicMapKeyboardController : MonoBehaviour
{
    public GameObject player;
    
    // Set size of and add locations in the Inspector
    public GameObject[] locations;
    public int playerLocation;
    
    private void Start()
    {
        // Set the player location to the scene we just left
        playerLocation = GlobalControls.CurrentScene;

        ReferenceManager references = GameObject.Find("Managers").GetComponent<ReferenceManager>();

        locations = new []
        {
            GameObject.Find("School Marker"), GameObject.Find("Park Marker"), GameObject.Find("Yard Marker")
        };
        player = references.player;
        references.deathCanvas.SetActive(false);
        references.segueCanvas.SetActive(false);
        references.inventoryCanvas.SetActive(false);
        references.dialogueCanvas.SetActive(false);
        references.tradeCanvas.SetActive(false);
    }

    void Update()
    {
        // Change the player's location with < and >
        if (Input.GetKeyDown(","))  // Move left with < key
        {
            playerLocation = (playerLocation - 1 + locations.Length) % locations.Length;
        }
        if (Input.GetKeyDown("."))  // Move right with > key
        {
            playerLocation = (playerLocation + 1) % locations.Length;
        }
        if (Input.GetKeyDown("space"))
        {
            GlobalControls.IsStrategicMap = false;
            SceneManager.LoadScene(locations[playerLocation].GetComponent<MapMarker>().mapName);
        }
        // Move the player on top of the marker for the location they are currently at
        player.transform.position = locations[playerLocation].transform.position + Vector3.up;
    }
}

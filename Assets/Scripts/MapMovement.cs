using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMovement : MonoBehaviour
{
    // Set size of and add locations in the Inspector
    public GameObject[] locations;
    public int playerLocation;

    private GameObject player;

    private void Start()
    {
        player = this.gameObject;
    }

    void Update()
    {
        // Change the player's location with < and >
        if (Input.GetKeyDown(","))
        {
            playerLocation--;
        }
        if (Input.GetKeyDown("."))
        {
            playerLocation++;
        }
        if (playerLocation < 0)
        {
            playerLocation = locations.Length - 1;
        }
        else if (playerLocation > locations.Length - 1)
        {
            playerLocation = 0;
        }

        // Move the player to the scene corresponding to the location they are at
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene(locations[playerLocation].GetComponent<MapMarker>().mapName);
        }

        // Move the player on top of the marker for the location they are currently at
        player.transform.position = locations[playerLocation].transform.position + new Vector3(0, 1, 0);
    }
}

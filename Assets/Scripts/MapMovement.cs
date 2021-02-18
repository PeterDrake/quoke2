using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMovement : MonoBehaviour
{
    public int numLocations = 3;

    public GameObject[] locations;
    public GameObject player;

    public int playerLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //go through locatons array
        player.transform.position = locations[playerLocation].transform.position + new Vector3(0, 1, 0);



        //select with < and  > and move to that direction
        if (Input.GetKeyDown(","))
        {
            if (playerLocation == 0)
            {
                playerLocation = 2;
            }

            else playerLocation--;
            
          
        }

        if (Input.GetKeyDown("."))
        {
            if (playerLocation == 2)
            {
                playerLocation = 0;
            }

            else playerLocation++;
        }

        if (Input.GetKeyDown("space"))
        {


            SceneManager.LoadScene(locations[playerLocation].name);

        }

        //once confirmed with space, move to that scene
    }
}

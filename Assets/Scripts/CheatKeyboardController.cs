using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatKeyboardController : MonoBehaviour
{
    // For scenes with no meters (before and during the quake), this can remain null
    public Meters meters;
    public SceneManagement sceneManagement;

    void Start()
    {
        meters = GameObject.Find("Managers").GetComponent<ReferenceManager>().metersCanvas.GetComponent<Meters>();
        sceneManagement = GameObject.Find("Managers").GetComponent<ReferenceManager>().sceneManagement.GetComponent<SceneManagement>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            sceneManagement.ChangeScene("PreQuakeHouse");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            sceneManagement.ChangeScene("QuakeHouse");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            sceneManagement.ChangeScene("StrategicMap");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            sceneManagement.ChangeScene("Park");
        }
        if (meters && Input.GetKeyDown(KeyCode.O))
        {
            meters.MarkTaskAsDone("water");
        }
        if (meters && Input.GetKeyDown(KeyCode.P))
        {
            meters.MarkTaskAsDone("poop");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            sceneManagement.Restart();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GlobalItemList.UpdateItemList("Shovel", "Inventory", new Vector3(0, 0, 0),"Player" );
            GlobalItemList.UpdateItemList("Tarp", "Inventory", new Vector3(1, 0, 0),"Player" );
            GlobalItemList.UpdateItemList("Plywood", "Inventory", new Vector3(2, 0, 0),"Player" );
            GlobalItemList.UpdateItemList("Rope", "Inventory", new Vector3(3, 0, 0),"Player" );
            sceneManagement.ChangeScene("Yard");
        }
        
    }
}

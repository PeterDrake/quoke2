using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatKeyboardController : MonoBehaviour
{
    // For scenes with no meters (before and during the quake), this can remain null
    public Meters meters;
    public SceneManagement sceneManagement;
    private string currentScene;

    void Start()
    {
        meters = GameObject.Find("Managers").GetComponent<ReferenceManager>().metersCanvas.GetComponent<Meters>();
        sceneManagement = GameObject.Find("Managers").GetComponent<ReferenceManager>().sceneManagement.GetComponent<SceneManagement>();
        currentScene = SceneManager.GetActiveScene().name;

    }
    void Update()
    {
        if (GlobalControls.AdminMode)
        {
            if (Input.GetKeyDown(KeyCode.H))//Load PreQuakeHouse
            {
                sceneManagement.ChangeScene("PreQuakeHouse");
            }
            if (Input.GetKeyDown(KeyCode.J))//Load QuakeHouse
            {
                sceneManagement.ChangeScene("QuakeHouse");
            }
            if (Input.GetKeyDown(KeyCode.K))//Load Strategic Map
            {
                sceneManagement.ChangeScene("StrategicMap");
            }
            if (Input.GetKeyDown(KeyCode.B))//Load Park
            {
                sceneManagement.ChangeScene("Park");
            }
            if (meters && Input.GetKeyDown(KeyCode.O)) //Complete Water
            {
                meters.MarkTaskAsDone("water");
            }
            if (meters && Input.GetKeyDown(KeyCode.P)) //Complete Poop
            {
                meters.MarkTaskAsDone("poop");
            }
            if (Input.GetKeyDown(KeyCode.N)) //Restart Game
            {
                sceneManagement.Restart();
            }
            if (currentScene.Equals("Yard") && Input.GetKeyDown(KeyCode.U))
            {
                GameObject.Find("Quake Event Manager").GetComponent<QuakeManager>().TriggerQuake();
            }
            if (Input.GetKeyDown(KeyCode.L)) //Load Yard with Latrine Items
            {
                GlobalItemList.UpdateItemList("Shovel", "Inventory", new Vector3(0, 0, 0),"Player" );
                GlobalItemList.UpdateItemList("Tarp", "Inventory", new Vector3(1, 0, 0),"Player" );
                GlobalItemList.UpdateItemList("Plywood", "Inventory", new Vector3(2, 0, 0),"Player" );
                GlobalItemList.UpdateItemList("Rope", "Inventory", new Vector3(3, 0, 0),"Player" );
                sceneManagement.ChangeScene("Yard");
            }
            if (Input.GetKeyDown(KeyCode.V)) //Load Yard with PreQuake Items
            {
                GlobalItemList.UpdateItemList("Cup", "Yard", new Vector3(6.5f,0.5f,0.5f),"" );
                GlobalItemList.UpdateItemList("Water Bottle", "Yard", new Vector3(-6.5f,0.5f,0.5f),"" );
                GlobalItemList.UpdateItemList("Sandwich", "Yard", new Vector3(3.5f,0.5f,3.5f),"" );
                GlobalItemList.UpdateItemList("Book", "Yard", new Vector3(-5.5f,0.5f,-7.5f),"" );
                sceneManagement.ChangeScene("Yard");
            }
        }
    }
}

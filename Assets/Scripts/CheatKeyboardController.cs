using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatKeyboardController : MonoBehaviour
{
    // For scenes with no meters (before and during the quake), this can remain null
    public Meters meters;

    void Start()
    {
        meters = GameObject.Find("Managers").GetComponent<ReferenceManager>().meters.GetComponent<Meters>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene("movement revised");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("PreQuakeHouse");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("QuakeHouse");
        }
        if (meters && Input.GetKeyDown(KeyCode.O))
        {
            meters.MarkTaskAsDone("water");
        }
        if (meters && Input.GetKeyDown(KeyCode.P))
        {
            meters.MarkTaskAsDone("poop");
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatKeyboardController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene("movement revised");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("HouseScene");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("QuakeHouse");
        }
    }
}

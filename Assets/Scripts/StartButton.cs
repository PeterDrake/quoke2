using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

    public string FirstScene;

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GlobalItemList.Reset();
            SceneManager.LoadScene(FirstScene);
        }
    }
    
}

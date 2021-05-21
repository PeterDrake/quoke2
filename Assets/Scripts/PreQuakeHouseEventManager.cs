using UnityEngine;
using UnityEngine.SceneManagement;

public class PreQuakeHouseEventManager : MonoBehaviour
{
    public StorageContainer[] containers;

    void FixedUpdate()
    {
        if (AllContainersFull())
        {
            SceneManager.LoadSceneAsync("QuakeHouse");
        }
    }
    
    private bool AllContainersFull()
    {
        foreach (StorageContainer container in containers)
        {
            if (!container.contents)
            {
                return false;
            }
        }
        return true;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PreQuakeHouseEventManager : MonoBehaviour
{
    public StorageContainer[] containers;

    void Update()
    {
        if (AllContainersFull())
        {
            SceneManager.LoadScene("QuakeHouse");
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

using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseSceneEventManager : MonoBehaviour
{
    public StorageContainer[] containerArray;

    // Update is called once per frame
    void Update()
    {
        if (AllContainersFull())
        {
            SceneManager.LoadScene("QuakeHouse");
        }
    }
    
    public bool AllContainersFull()
    {
        foreach (StorageContainer container in containerArray)
        {
            if (!container.storedItem)
            {
                return false;
            }
        }
        return true;
    }
}

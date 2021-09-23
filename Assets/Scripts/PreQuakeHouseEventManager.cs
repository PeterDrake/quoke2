using UnityEngine;
using UnityEngine.SceneManagement;

public class PreQuakeHouseEventManager : MonoBehaviour
{
    public StorageContainer[] containers;
    
    void FixedUpdate()
    {
        if (AllContainersFull())
        {
            if (GlobalControls.globalControlsProperties.Contains("apartmentCondition"))
            {
                foreach (StorageContainer container in containers)
                {
                    GameObject item = container.contents;
                    if (item)
                    {
                        GlobalItemList.UpdateItemList(item.name, "QuakeApartment", item.transform.position, container.name);
                    }
                }

                SceneManager.LoadScene("QuakeApartment");
            }
            else
            {
                foreach (StorageContainer container in containers)
                {
                    GameObject item = container.contents;
                    if (item)
                    {
                        GlobalItemList.UpdateItemList(item.name, "QuakeHouse", item.transform.position, container.name);
                    }
                }

                SceneManager.LoadScene("QuakeHouse");
            }
        }
    }
    
    private bool AllContainersFull()
    {
        int fullCount = 0;
        foreach (StorageContainer container in containers)
        {
            if (container.contents)
            {
                fullCount++;
            }
        }

        if (fullCount < 2) return false;
        return true;
    }
}

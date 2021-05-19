using UnityEngine;
using UnityEngine.Serialization;
using Image = UnityEngine.UI.Image;

public class Inventory : MonoBehaviour
{
    public int selectedSlotNumber;
    public PlayerMover player;
    private Transform movePoint;  // TODO This class shouldn't know about this
    
    public Sprite unselectedSlotSprite;
    public Sprite selectedSlotSprite;
    
    // These are UI Images showing sprites (either unselectedSlotSprite or selectedSlotSprite).
    public GameObject[] slotFrames;

    // These are UI Images showing sprites for occupied slots. They're inactive for unoccupied slots.
    public GameObject[] slotContents;
    
    // These are the actual 3D GameObjects that the player has picked up.
    public GameObject[] items;
    
    // You cannot drop an item if something in one of theses layers (e.g., a wall) is in front of you.
    private int dropObstructionLayers;
    
    // This would only allow for 10 inventory slots max
    // If more than 10 slots are needed, add the KeyCodes you want associated with those slots to validInputs here
    private readonly KeyCode[] validInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
    
    // Start is called before the first frame update
    void Start()
    {
        movePoint = player.GetComponent<PlayerMover>().destination;
        // Set initial state of all the arrays
        foreach (GameObject frame in slotFrames)
        {
            frame.GetComponent<Image>().sprite = unselectedSlotSprite;
        }
        foreach (GameObject item in slotContents)
        {
            item.SetActive(false);
        }
        items = new GameObject[slotFrames.Length];
        // Select the first slot
        selectedSlotNumber = 0;
        slotFrames[selectedSlotNumber].GetComponent<Image>().sprite = selectedSlotSprite;
        // Find the layers that obstruct dropping
        dropObstructionLayers = LayerMask.GetMask("Wall", "NPC", "Table", "Exit", "StorageContainer");
    }

    void SelectSlotNumber(int slotNumber)
    {
        if (slotNumber < 0 || slotNumber >= slotFrames.Length)
        {
            return;
        }
        slotFrames[slotNumber].GetComponent<Image>().sprite = selectedSlotSprite;
        if (selectedSlotNumber != slotNumber)
        {
            slotFrames[selectedSlotNumber].GetComponent<Image>().sprite = unselectedSlotSprite;
            selectedSlotNumber = slotNumber;
        }
    }

    // removes item at specific slotNumber and sets item one grid unit in front of the player
    void RemoveItemFromInventorySlot(int slotNumber)
    {
        // Sends a Raycast out one space in front of the player, to check if there is anything in the way before item placement
        if (!player.ObjectAhead(dropObstructionLayers))
        {
            if (slotContents[slotNumber].activeSelf)
            {
                items[slotNumber].SetActive(true);
                items[slotNumber].transform.position = movePoint.position + player.transform.forward;
                items[slotNumber] = null;
                slotContents[slotNumber].SetActive(false);  
            }
        }
    }

    // Handles placing an item in a storage container or taking an item out of a storage container
    void InteractWithStorageContainer(int slotNumber, RaycastHit storageContainerCheck)
    {
        StorageContainer container = storageContainerCheck.collider.gameObject.GetComponent<StorageContainer>();
        if (container.isItemStored() && !slotContents[slotNumber].activeSelf)
        {
            GameObject item = container.removeItem();
            item.GetComponent<Collectible>().inStorageContainer = false;
            AddItemToInventory(item);
        }
        else if (slotContents[slotNumber].activeSelf && container.storeItem(items[slotNumber]))
        {
            items[slotNumber].SetActive(true);
            items[slotNumber].transform.position = player.transform.position + (player.transform.forward) + new Vector3(0f, 1f, 0f);
            items[slotNumber].GetComponent<Collectible>().inStorageContainer = true;
            items[slotNumber] = null;
            slotContents[slotNumber].SetActive(false);
        }
    }

    // finds next empty spot and if there is changes sprite to that item's sprite
    public void AddItemToInventory(GameObject collectable)
    {
        int nextSpot = FindNextEmptySpot();
        if (nextSpot >= 0)
        {
            slotContents[nextSpot].SetActive(true);
            slotContents[nextSpot].GetComponent<Image>().sprite = collectable.GetComponent<Collectible>().sprite;
            items[nextSpot] = collectable;
            collectable.SetActive(false);
        }

    }

    //goes through all the slots to find the first deactivated item and returns that slot number
    int FindNextEmptySpot()
    {
        for(int i = 0; i < slotFrames.Length; i++)
        {
            if (!slotContents[i].activeSelf)
            {
                return i;
            }
        }
        return -1;
    }
    // Update is called once per frame
    void Update()
    {
        //If spacebar is pressed, run RemoveItemFromInventory on currently selected slot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit storageContainerCheck;
            if (Physics.Raycast(player.transform.position, player.transform.forward, out storageContainerCheck, 1f, LayerMask.GetMask("StorageContainer")))
            {
                InteractWithStorageContainer(selectedSlotNumber, storageContainerCheck);
            }
            RemoveItemFromInventorySlot(selectedSlotNumber);
        }
        
        // If the user presses a key that is one of the valid inputs for slot selection, select that slot
        for (int i = 0; i < slotFrames.Length; i++)
        {
            if (Input.GetKey(validInputs[i]))
            {
                SelectSlotNumber(i);
            }
        }

    }

}
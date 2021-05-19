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

    private void DropSelectedItem()
    {
        // Sends a Raycast out one space in front of the player, to check if there is anything in the way before item placement
        if (!player.ObjectAhead(dropObstructionLayers))
        {
            int i = selectedSlotNumber;  // Just to make the later expressions less hairy
            if (slotContents[i].activeSelf)
            {
                // Place item in front of player
                items[i].SetActive(true);
                items[i].transform.position = player.destination.transform.position + player.transform.forward;
                // Remove item from inventory
                items[i] = null;
                slotContents[i].SetActive(false);  
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
            PickUp(item);
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

    public void PickUp(GameObject item)
    {
        int i = NextEmptySpot();
        if (i >= 0)
        {
            // Display the sprite for this item
            slotContents[i].SetActive(true);
            slotContents[i].GetComponent<Image>().sprite = item.GetComponent<Collectible>().sprite;
            // Add item to the items array
            items[i] = item;
            // Remove item from the world
            item.SetActive(false);
        }

    }

    //goes through all the slots to find the first deactivated item and returns that slot number
    private int NextEmptySpot()
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
            DropSelectedItem();
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
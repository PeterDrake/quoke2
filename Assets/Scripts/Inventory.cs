using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public int selectedSlotNumber;
    public PlayerMover player;

    // These are the actual 3D GameObjects that the player has picked up.
    public GameObject[] items;

    private InventoryUI inventoryUI;
    
    private int dropObstructionLayers;  // You cannot drop an item if something in one of theses layers (e.g., a wall)
                                        // is in front of you.
    private int storageContainerLayers;
    private int latrineContainerLayers;
    private int WaterHeaterLayers;
    private int GasValveLayers;
    private int waterLayer;

    private ReferenceManager referenceManager;
    private Meters meters;

    private LatrineStorage latrineStorage;
    private TwoBucketScript twoBucket;
    // Start is called before the first frame update
    //Awake not start because Inventory must load first
    void Awake()
    {
        inventoryUI = this.gameObject.GetComponent<InventoryUI>();

        if (SceneManager.GetActiveScene().name.Equals("Yard"))
        {
            latrineStorage = GameObject.Find("Latrine Hole").GetComponent<LatrineStorage>();
        }
        else if (SceneManager.GetActiveScene().name.Equals("Street"))
        {
            twoBucket = GameObject.Find("Two Bucket Spot").GetComponent<TwoBucketScript>();
        }
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();

        Clear();

        // Find layers for various interactions
        dropObstructionLayers = LayerMask.GetMask("Wall", "NPC", "Table", "Exit", "StorageContainer",
            "LatrineContainer", "WaterPurifying", "GasValve", "WaterHeater");
        storageContainerLayers = LayerMask.GetMask("StorageContainer");
        latrineContainerLayers = LayerMask.GetMask("LatrineContainer");
        GasValveLayers = LayerMask.GetMask("GasValve");
        WaterHeaterLayers = LayerMask.GetMask("WaterHeater");
        waterLayer = LayerMask.GetMask("Water");
    }

    /// <summary>
    /// Sets the inventory to contain no items, with slot 0 selected.
    /// </summary>
    public void Clear()
    {
        items = new GameObject[5];
        selectedSlotNumber = 0;
    }
    
    private void Start()
    {
        player = referenceManager.player.GetComponent<PlayerMover>();

        SelectSlotNumber(0);
    }

    private void OnEnable()
    {
        if(items.Length > 0) SelectSlotNumber(0);
    }

    public void SetAvailableSlots(int numSlots)
    {
        inventoryUI.SetAvailableSlots(numSlots);

        GameObject[] tempItems = new GameObject[numSlots];
        
        for (int i = 0; i < numSlots; i++)
        {
            tempItems[i] = items[i];
        }

        items = tempItems;
    }
    
    
    public void SelectSlotNumber(int slotNumber)
    {
        if (slotNumber < 0 || slotNumber >= items.Length) return;

        if (selectedSlotNumber != slotNumber)
        {
            inventoryUI.SelectSlotNumber(slotNumber);
            selectedSlotNumber = slotNumber;
        }

        inventoryUI.UpdateTooltip();
    }

    /// <summary>
    /// Takes the item in container (if there is one) or puts the selected item into container.
    /// </summary>
    void InteractWithStorageContainer(StorageContainer container)
    {
        int i = FirstEmptySlot();
        if (i >= 0 && !SlotIsOccupied(i) && container.contents)
        {
            GameObject item = container.RemoveItem();
            item.GetComponent<Collectible>().inStorageContainer = false;
            PickUp(item);
        }
        else
        {
            i = selectedSlotNumber; // Just to make the later expressions less hairy
            if (SlotIsOccupied(i) && !container.contents)
            {
                // Place item in the container
                container.contents = items[i];
                items[i].SetActive(true);
                Transform t = player.transform;
                items[i].transform.position =
                    player.destination.transform.position + player.transform.forward + Vector3.up;
                items[i].GetComponent<Collectible>().inStorageContainer = true;
                
                GlobalItemList.UpdateItemList(items[i].name, SceneManager.GetActiveScene().name,
                    items[i].transform.position, container.name);

                if (items[i].name.Equals("Water Bottle Clean(Clone)"))
                    GlobalControls.globalControlsProperties.Remove("playerHasCleanWater");

                if (items[i].name.Equals("Wrench(Clone)"))
                    GlobalControls.globalControlsProperties.Remove("playerHasWrench");

                if (items[i].name.Equals("First Aid Kit(Clone)"))
                    GlobalControls.globalControlsProperties.Remove("playerHasFirstAidKit");
                
                if (items[i].name.Equals("Epi Pen(Clone)"))
                    GlobalControls.globalControlsProperties.Remove("playerHasEpiPen");
                
                if (items[i].name.Equals("Wrench(Clone)"))
                    GlobalControls.globalControlsProperties.Remove("playerHasWrench");
                
                // Remove item from inventory
                items[i] = null;
                inventoryUI.RemoveFromSlot(i);
            }
        }
    }

    void RemoveLatrineItem(int i)
    {
        latrineStorage.contents = items[i];
        items[i].SetActive(true);
        items[i].transform.position = player.destination.transform.position + player.transform.forward + Vector3.up;
        items[i].GetComponent<Collectible>().inLatrine = true;
                    
        GlobalItemList.UpdateItemList(items[i].name, "",
            new Vector3(0,0,0), "");
        GameObject.Find(items[i].name).SetActive(false);
        // Remove item from inventory
        items[i] = null;
        inventoryUI.RemoveFromSlot(i);
        latrineStorage.contents = null;
    }
    
    void InteractWithLatrine()
    {
        int i = 0;
        int x = 0;
        if(SlotIsOccupied(selectedSlotNumber))
        {
            if (items[selectedSlotNumber].name.Equals("Shovel(Clone)")
                && player.ObjectAhead(latrineContainerLayers)
                && !GlobalControls.globalControlsProperties.Contains("poopTaskCompleted")
                && !latrineStorage.plywoodDone)
            {
                latrineStorage.timesShoveled++;
                GlobalControls.TimesShoveled++;
                Debug.Log("Time shoveled: " + latrineStorage.timesShoveled);
                if (latrineStorage.ShovelingComplete())
                {
                    GlobalControls.PoopTaskProgress[0] = true;
                    latrineStorage.shovelingDone = true;
                }else if (latrineStorage.timesShoveled > 4)
                {
                    Debug.Log("Resetting shoveling");
                    latrineStorage.timesShoveled = 0;
                    GlobalControls.TimesShoveled = 0;
                    latrineStorage.shovelingDone = false;
                }
            }
            i = selectedSlotNumber; // Just to make the later expressions less hairy
            if (SlotIsOccupied(i) && !latrineStorage.contents)
            {
                // Place item in the container if the item is a latrine task item
                if (items[i].name.Equals("Shovel(Clone)") && player.ObjectAhead(latrineContainerLayers)) x = 1; 
                if (items[i].name.Equals("Plywood(Clone)") && latrineStorage.shovelingDone) x = 2;
                if (items[i].name.Equals("Rope(Clone)") && latrineStorage.plywoodDone) x = 3;
                if (items[i].name.Equals("Tarp(Clone)") && latrineStorage.ropeDone) x = 4;
                if (items[i].name.Equals("Toilet Paper(Clone)") && latrineStorage.tarpDone) x = 5;

                switch (x)
                {
                    case 0:
                        Debug.Log("You cant do that try again");
                        break;
                    case 1:
                        //When you try to dig
                        break;
                    case 2:
                        RemoveLatrineItem(i);
                        latrineStorage.plywoodDone = true;
                        GlobalControls.PoopTaskProgress[1] = true;
                        Debug.Log("Plywood Complete");
                        break;
                    case 3:
                        RemoveLatrineItem(i);
                        latrineStorage.ropeDone = true;
                        GlobalControls.PoopTaskProgress[2] = true;
                        Debug.Log("Rope Complete");
                        break;
                    case 4:
                        RemoveLatrineItem(i);
                        latrineStorage.tarpDone = true;
                        GlobalControls.PoopTaskProgress[3] = true;
                        Debug.Log("Tarp Complete");
                        break;
                    case 5:
                        RemoveLatrineItem(i);
                        latrineStorage.toiletPaperDone = true;
                        GlobalControls.PoopTaskProgress[4] = true;
                        Debug.Log("Toilet Paper Complete");
                        latrineStorage.LatrineComplete();
                        break;
                }
                latrineStorage.UpdateVisuals();
            }
        }
    }

    void InteractWithTwoBucket()
    {
        int x = 0;
        int i = FirstEmptySlot();
        if (i >= 0 && !SlotIsOccupied(i) && twoBucket.contents)
        {
            GameObject item = twoBucket.RemoveBucketItem();
            item.GetComponent<Collectible>().inStorageContainer = false;
            PickUp(item);
        }
        else
        {
            i = selectedSlotNumber; // Just to make the later expressions less hairy
            if (SlotIsOccupied(i) && !twoBucket.contents && player.ObjectAhead(latrineContainerLayers))
            {
                // Place item in the container if the item is a latrine task item
                if (items[i].name.Equals("Bucket(Clone)")) x = 1; 
                if (items[i].name.Equals("Bucket 2(Clone)")) x = 2;
                if (items[i].name.Equals("Bag(Clone)") && twoBucket.bucketTwoDone && twoBucket.bucketDone) x = 3;
                if (items[i].name.Equals("Toilet Paper(Clone)") && twoBucket.bagDone) x = 4;
                if (items[i].name.Equals("Wood Chips(Clone)") && twoBucket.toiletPaperDone) x = 5;

                switch (x)
                {
                    case 0:
                        Debug.Log("You cant do that try again");
                        break;
                    case 1:
                        RemoveBucketItem(i);
                        twoBucket.bucketDone = true;
                        GlobalControls.PoopTaskProgress[0] = true;
                        Debug.Log("Bucket Complete");
                        break;
                    case 2:
                        RemoveBucketItem(i);
                        twoBucket.bucketTwoDone = true;
                        GlobalControls.PoopTaskProgress[1] = true;
                        Debug.Log("Bucket two Complete");
                        break;
                    case 3:
                        RemoveBucketItem(i);
                        twoBucket.bagDone = true;
                        GlobalControls.PoopTaskProgress[2] = true;
                        Debug.Log("Bag Complete");
                        break;
                    case 4:
                        RemoveBucketItem(i);
                        twoBucket.toiletPaperDone = true;
                        GlobalControls.PoopTaskProgress[3] = true;
                        Debug.Log("Toilet Paper Complete");
                        break;
                    case 5:
                        RemoveBucketItem(i);
                        twoBucket.woodChipsDone = true;
                        GlobalControls.PoopTaskProgress[4] = true;
                        Debug.Log("Wood Chips Complete");
                        twoBucket.TwoBucketComplete();
                        break;
                        
                }
                twoBucket.UpdateVisuals();
            }
        }
    }
    
    void RemoveBucketItem(int i)
    {
        twoBucket.contents = items[i];
        items[i].SetActive(true);
        items[i].transform.position = player.destination.transform.position + player.transform.forward + Vector3.up;
        items[i].GetComponent<Collectible>().inLatrine = true;
                    
        GlobalItemList.UpdateItemList(items[i].name, "",
            new Vector3(0,0,0), "");
        GameObject.Find(items[i].name).SetActive(false);
        // Remove item from inventory
        items[i] = null;
        inventoryUI.RemoveFromSlot(i);
        twoBucket.contents = null;
    }
    
    private bool SlotIsOccupied(int i)
    {
        return items[i] != null;
    }
    
    public void PickUp(GameObject item)
    {
        int i = FirstEmptySlot();
        if (i >= 0)
        {
            inventoryUI.AddToSlot(i, item.GetComponent<Collectible>().sprite);

            // Add item to the items array
            items[i] = item;

            //updates item list to add item to list
            GlobalItemList.UpdateItemList(item.name,
                "Inventory",
                new Vector3(i, 0, 0),
                "Player");
            
            // Remove item from the world
            item.SetActive(false);

            if (item.name.Equals("Water Bottle Clean(Clone)"))
            {
                Debug.Log("Player Has Water!");
                GlobalControls.globalControlsProperties.Add("playerHasCleanWater");
            }
            
            if (item.name.Equals("Wrench(Clone)"))
            {
                Debug.Log("Player Has Wrench!");
                GlobalControls.globalControlsProperties.Add("playerHasWrench");
            }
            
            if (item.name.Equals("First Aid Kit(Clone)"))
            {
                Debug.Log("Player Has First Aid Kit!");
                GlobalControls.globalControlsProperties.Add("playerHasFirstAidKit");
            }

            if (item.name.Equals("Epi Pen(Clone)"))
            {
                Debug.Log("Player Has Epi Pen!");
                GlobalControls.globalControlsProperties.Add("playerHasEpiPen");
            }
        }
        //reselect slot to current slot number to update tooltip if necessary
        SelectSlotNumber(selectedSlotNumber);
    }

    /// <summary>
    /// Returns the index of the first empty slot, or -1 if all slots are full.
    /// <returns></returns>
    private int FirstEmptySlot()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// If in front of a container, take the item in the container (if there is one) or drop the selected item.
    /// Otherwise drop the selected item unless obstructed.
    /// </summary>
    public void PickUpOrDrop()
    {
        if (!SceneManager.GetActiveScene().name.Equals("GameEnd"))
        {
            GameObject container = player.ObjectAhead(storageContainerLayers);
            GameObject latrine = player.ObjectAhead(latrineContainerLayers);
            GameObject heater = player.ObjectAhead(WaterHeaterLayers);
            GameObject gas = player.ObjectAhead(GasValveLayers);
            if (container)
            {
                if (container.name.Equals("Water Purifying Table"))
                {
                    InteractWithWaterPurifyingTable(container.GetComponent<StorageContainer>());
                }
                else InteractWithStorageContainer(container.GetComponent<StorageContainer>());
            }
            else if (latrine && !GlobalControls.globalControlsProperties.Contains("apartmentCondition"))
            {
                InteractWithLatrine();
            }
            else if (GlobalControls.globalControlsProperties.Contains("apartmentCondition") && latrine)
            {
                InteractWithTwoBucket();
            }
            else if (heater || gas)
            {
                InteractWithWaterHeaterOrGasValve();
            }
        }
    }

    private void InteractWithWaterHeaterOrGasValve()
    {
        

    }

    private void InteractWithWaterPurifyingTable(StorageContainer container)
    {
        if (!container.contents && SlotIsOccupied(selectedSlotNumber))
        {
            if (items[selectedSlotNumber].name.Equals("Dirty Water Bottle(Clone)"))
            {
                Debug.Log("Dropping water bottle");
                container.contents = items[selectedSlotNumber];
                items[selectedSlotNumber].SetActive(true);
                Transform t = player.transform;
                items[selectedSlotNumber].transform.position =
                    player.destination.transform.position + player.transform.forward + Vector3.up;
                items[selectedSlotNumber].GetComponent<Collectible>().inStorageContainer = true;
                
                GlobalItemList.UpdateItemList(items[selectedSlotNumber].name, SceneManager.GetActiveScene().name,
                    items[selectedSlotNumber].transform.position, container.name);
                
                // Remove item from inventory
                items[selectedSlotNumber] = null;
                inventoryUI.RemoveFromSlot(selectedSlotNumber);
            }
        }
        else if (container.contents && SlotIsOccupied(selectedSlotNumber))
        {
            if (items[selectedSlotNumber].name.Equals("Bleach(Clone)") && !GlobalControls.globalControlsProperties.Contains("waterPurified"))
            {
                Debug.Log("Using bleach");

                meters = referenceManager.metersCanvas.GetComponent<Meters>();
                meters.MarkTaskAsDone("water");
                    
                GlobalItemList.UpdateItemList("Dirty Water Bottle",
                    "",
                    new Vector3(0,0,0),
                    "");
                GameObject.Find("Dirty Water Bottle(Clone)").SetActive(false);
                GlobalItemList.UpdateItemList("Water Bottle Clean",
                    SceneManager.GetActiveScene().name, 
                    player.destination.transform.position + player.transform.forward + Vector3.up,
                    "Water Purifying Table");
                GlobalControls.globalControlsProperties.Add("waterPurified");
                    
                GameObject prefab = (GameObject) Resources.Load("Water Bottle Clean", typeof(GameObject));
                GameObject waterBottleClean = Instantiate(prefab,
                    player.destination.transform.position + player.transform.forward + Vector3.up,
                    Quaternion.identity);
                container.contents = waterBottleClean;
            }
        }
        else
        {
            int i = FirstEmptySlot();
            if (i >= 0 && !SlotIsOccupied(i) && container.contents)
            {
                //picking up the item
                GameObject item = container.RemoveItem();
                item.GetComponent<Collectible>().inStorageContainer = false;
                PickUp(item);
            }
        }

    }


    /** Only used by ItemLoader.cs */
    public void PickUpAtSlot(int slot, GameObject item)
    {
        // Debug.Log(slotContents.Length + " slotcontents length");
        int i = slot;
        if (i >= 0)
        {
            inventoryUI.AddToSlot(slot, item.GetComponent<Collectible>().sprite);
            
            // Add item to the items array
            items[i] = item;
            if (item.name.Equals("Water Bottle Clean(Clone)"))
                GlobalControls.globalControlsProperties.Add("playerHasCleanWater");
            if (item.name.Equals("Wrench(Clone)"))
                GlobalControls.globalControlsProperties.Add("playerHasWrench");
            if (item.name.Equals("First Aid Kit(Clone)"))
                GlobalControls.globalControlsProperties.Add("playerHasFirstAidKit");
            if (item.name.Equals("Epi Pen(Clone)"))
                GlobalControls.globalControlsProperties.Add("playerHasEpiPen");
            
            // Remove item from the world
            item.SetActive(false);
        }
        
    }
}
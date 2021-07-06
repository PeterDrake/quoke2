using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatrineStorage : MonoBehaviour
{

    public int timesShoveled;
    public GameObject contents;

    public bool shovelingDone;
    public bool plywoodDone;
    public bool ropeDone;
    public bool tarpDone;
    public bool toiletPaperDone;

    
    public GameObject shovel;
    public GameObject plywood;
    public GameObject rope;
    public GameObject tarp;
    public GameObject toiletPaper;

    public Meters meters;
    public ReferenceManager referenceManager;

    public void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        meters = referenceManager.metersCanvas.GetComponent<Meters>();
        shovelingDone = GlobalControls.PoopTaskProgress[0];
        plywoodDone = GlobalControls.PoopTaskProgress[1];
        ropeDone = GlobalControls.PoopTaskProgress[2];
        tarpDone = GlobalControls.PoopTaskProgress[3];
        toiletPaperDone = GlobalControls.PoopTaskProgress[4];
        
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (child.gameObject.name.Equals("Hole")) shovel = child.gameObject;
            else if (child.gameObject.name.Equals("Plywood")) plywood = child.gameObject;
            else if (child.gameObject.name.Equals("Tarp")) tarp = child.gameObject;
            else if (child.gameObject.name.Equals("Rope")) rope = child.gameObject;
            else if (child.gameObject.name.Equals("Toilet Paper")) toiletPaper = child.gameObject;
        }
        UpdateVisuals();
    }


    public void UpdateVisuals()
    {
        if(shovelingDone) shovel.SetActive(true);
        else shovel.SetActive(false);
        if(plywoodDone) plywood.SetActive(true);
        else plywood.SetActive(false);
        if(ropeDone) rope.SetActive(true);
        else rope.SetActive(false);
        if(tarpDone) tarp.SetActive(true);
        else tarp.SetActive(false);
        if(toiletPaperDone) toiletPaper.SetActive(true);
        else toiletPaper.SetActive(false);
    }
    
    /// Removes and returns the currently stored item (or null if there is no such item)
    public GameObject RemoveLatrineItem()
    {
        GameObject temp = contents;
        contents = null;
        return temp;
    }
    
    public bool CheckAllLatrineItems()
    {
        if (inventoryHasItem("Shovel") || inventoryHasItem("Tarp") || inventoryHasItem("Rope") || inventoryHasItem("Plywood") || inventoryHasItem("Toilet Paper"))
        {
            return true;
        }
        return false;
    }

    public bool inventoryHasItem(string itemName)
    {
        foreach (Item item in GlobalItemList.ItemList.Values)
        {
            if (itemName.Equals(item.name))
            {
                if (item.containerName.Equals("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }



    public bool ShovelingComplete()
    {
        if (timesShoveled >= 2 && timesShoveled <= 4)
        {
            return true;
        }
        return false;
    }


    public bool LatrineComplete()
    {
        if (!ShovelingComplete()) return false;
        if (!plywoodDone) return false;
        if (!ropeDone) return false;
        if (!tarpDone) return false;
        if (!toiletPaperDone) return false;
        Debug.Log("Completed latrine uwu");
        meters.MarkTaskAsDone("poop");
        return true;
    }
    
    
    
}

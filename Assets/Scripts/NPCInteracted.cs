using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteracted : MonoBehaviour
{
    private ReferenceManager referenceManager;

    public bool safiInteracted;
    public bool demInteracted;
    public bool rainerInteracted;
    public bool fredInteracted;

    private GameObject safiImage;
    private GameObject demImage;
    private GameObject rainerImage;
    private GameObject fredImage;

    void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        safiImage = GameObject.Find("Safi Met Image");
        demImage = GameObject.Find("Dem Met Image");
        rainerImage = GameObject.Find("Rainer Met Image");
        fredImage = GameObject.Find("Fred Met Image");

        safiInteracted = GlobalControls.SafiInteracted;
        demInteracted = GlobalControls.DemInteracted;
        rainerInteracted = GlobalControls.RainerInteracted;
        fredInteracted = GlobalControls.FredInteracted;

        if (!safiInteracted)
        { 
            safiImage.SetActive(false);
        }
        if (!demInteracted)
        { 
            demImage.SetActive(false);
        }
        if (!rainerInteracted)
        { 
            rainerImage.SetActive(false);
        }
        if (!fredInteracted)
        { 
            fredImage.SetActive(false);
        }
        
    }

    public void updateNPCInteracted(string name)
    {
        Debug.Log("Updating NPC Interacted");
        if (name.Equals("safi0"))
        {
            safiInteracted = true;
            GlobalControls.SafiInteracted = true;
            safiImage.SetActive(true);
        }
        else if (name.Equals("dem0"))
        {
            demInteracted = true;
            GlobalControls.DemInteracted = true;
            demImage.SetActive(true);
        }
        else if (name.Equals("rainer0"))
        {
            rainerInteracted = true;
            GlobalControls.RainerInteracted = true;
            rainerImage.SetActive(true);
        }
        else if (name.Equals("fred0"))
        {
            fredInteracted = true;
            GlobalControls.FredInteracted = true;
            fredImage.SetActive(true);
        }
    }
    
    
}

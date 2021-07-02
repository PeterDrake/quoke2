using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    
    private GameObject safiSatisfaction;
    private GameObject demSatisfaction;
    private GameObject rainerSatisfaction;
    private GameObject fredSatisfaction;


    private void OnEnable()
    {
        if(safiSatisfaction) safiSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["safi0"].satisfaction + " / " 
            + GlobalControls.NPCList["safi0"].totalSatisfaction;
        if(demSatisfaction) demSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["dem0"].satisfaction + " / " 
            + GlobalControls.NPCList["dem0"].totalSatisfaction;
        if(rainerSatisfaction) rainerSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["rainer0"].satisfaction + " / " 
            + GlobalControls.NPCList["rainer0"].totalSatisfaction;
        if(fredSatisfaction) fredSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["fred0"].satisfaction + " / " 
            + GlobalControls.NPCList["fred0"].totalSatisfaction;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("PreQuakeHouse") ||
            SceneManager.GetActiveScene().name.Equals("QuakeHouse"))
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            safiImage = GameObject.Find("Safi Met Image");
            demImage = GameObject.Find("Dem Met Image");
            rainerImage = GameObject.Find("Rainer Met Image");
            fredImage = GameObject.Find("Fred Met Image");
            
            safiSatisfaction = GameObject.Find("Safi Satisfaction");
            demSatisfaction = GameObject.Find("Dem Satisfaction");
            rainerSatisfaction = GameObject.Find("Rainer Satisfaction");
            fredSatisfaction = GameObject.Find("Fred Satisfaction");

            safiInteracted = GlobalControls.SafiInteracted;
            demInteracted = GlobalControls.DemInteracted;
            rainerInteracted = GlobalControls.RainerInteracted;
            fredInteracted = GlobalControls.FredInteracted;

            safiSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["safi0"].satisfaction + " / " 
                + GlobalControls.NPCList["safi0"].totalSatisfaction;
            demSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["dem0"].satisfaction + " / " 
                + GlobalControls.NPCList["dem0"].totalSatisfaction;
            rainerSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["rainer0"].satisfaction + " / " 
                + GlobalControls.NPCList["rainer0"].totalSatisfaction;
            fredSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["fred0"].satisfaction + " / " 
                + GlobalControls.NPCList["fred0"].totalSatisfaction;

            if (!safiInteracted)
            {
                safiImage.SetActive(false);
                safiSatisfaction.SetActive(false);
            }

            if (!demInteracted)
            {
                demImage.SetActive(false);
                demSatisfaction.SetActive(false);
            }

            if (!rainerInteracted)
            {
                rainerImage.SetActive(false);
                rainerSatisfaction.SetActive(false);
            }

            if (!fredInteracted)
            {
                fredImage.SetActive(false);
                fredSatisfaction.SetActive(false);
            }
            
            
        }

    }

    public void UpdateNPCInteracted(string name)
    {
        if (name.Equals("safi0"))
        {
            safiSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["safi0"].satisfaction + " / " 
                + GlobalControls.NPCList["safi0"].totalSatisfaction;
            safiInteracted = true;
            GlobalControls.SafiInteracted = true;
            GlobalControls.NPCList["safi0"].interracted = true;
            safiImage.SetActive(true);
            safiSatisfaction.SetActive(true);
        }
        else if (name.Equals("dem0"))
        {
            demSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["dem0"].satisfaction + " / " 
                + GlobalControls.NPCList["dem0"].totalSatisfaction;
            demInteracted = true;
            GlobalControls.DemInteracted = true;
            GlobalControls.NPCList["dem0"].interracted = true;
            demImage.SetActive(true);
            demSatisfaction.SetActive(true);
        }
        else if (name.Equals("rainer0"))
        {
            rainerSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["rainer0"].satisfaction + " / " 
                + GlobalControls.NPCList["rainer0"].totalSatisfaction;
            rainerInteracted = true;
            GlobalControls.RainerInteracted = true;
            GlobalControls.NPCList["rainer0"].interracted = true;
            rainerImage.SetActive(true);
            rainerSatisfaction.SetActive(true);
        }
        else if (name.Equals("fred0"))
        {
            fredSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["fred0"].satisfaction + " / " 
                + GlobalControls.NPCList["fred0"].totalSatisfaction;
            fredInteracted = true;
            GlobalControls.FredInteracted = true;
            GlobalControls.NPCList["fred0"].interracted = true;
            fredImage.SetActive(true);
            fredSatisfaction.SetActive(true);
        }
    }
}

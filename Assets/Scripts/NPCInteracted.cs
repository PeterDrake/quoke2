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
    public bool annetteInteracted;
    public bool carlosInteracted;
    public bool bobInteracted;

    private GameObject safiImage;
    private GameObject demImage;
    private GameObject rainerImage;
    private GameObject annetteImage;
    private GameObject carlosImage;
    private GameObject bobImage;
    
    private GameObject safiSatisfaction;
    private GameObject demSatisfaction;
    private GameObject rainerSatisfaction;
    private GameObject annetteSatisfaction;
    private GameObject carlosSatisfaction;
    private GameObject bobSatisfaction;


    private void OnEnable()
    {
        if(safiSatisfaction) safiSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["safi0"].satisfaction + " / " 
            + GlobalControls.NPCList["safi0"].totalSatisfaction;
        if(demSatisfaction) demSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["dem0"].satisfaction + " / " 
            + GlobalControls.NPCList["dem0"].totalSatisfaction;
        if(rainerSatisfaction) rainerSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["rainer0"].satisfaction + " / " 
            + GlobalControls.NPCList["rainer0"].totalSatisfaction;
        if(annetteSatisfaction) annetteSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["annette0"].satisfaction + " / " 
            + GlobalControls.NPCList["annette0"].totalSatisfaction;
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
            annetteImage = GameObject.Find("Annette Met Image");
            carlosImage = GameObject.Find("Carlos Met Image");
            bobImage = GameObject.Find("Bob Met Image");
            
            safiSatisfaction = GameObject.Find("Safi Satisfaction");
            demSatisfaction = GameObject.Find("Dem Satisfaction");
            rainerSatisfaction = GameObject.Find("Rainer Satisfaction");
            annetteSatisfaction = GameObject.Find("Annette Satisfaction");
            carlosSatisfaction = GameObject.Find("Carlos Satisfaction");
            bobSatisfaction = GameObject.Find("Bob Satisfaction");

            safiInteracted = GlobalControls.SafiInteracted;
            demInteracted = GlobalControls.DemInteracted;
            rainerInteracted = GlobalControls.RainerInteracted;
            annetteInteracted = GlobalControls.AnnetteInteracted;
            carlosInteracted = GlobalControls.CarlosInteracted;
            bobInteracted = GlobalControls.BobInteracted;

            safiSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["safi0"].satisfaction + " / " 
                + GlobalControls.NPCList["safi0"].totalSatisfaction;
            demSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["dem0"].satisfaction + " / " 
                + GlobalControls.NPCList["dem0"].totalSatisfaction;
            rainerSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["rainer0"].satisfaction + " / " 
                + GlobalControls.NPCList["rainer0"].totalSatisfaction;
            annetteSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["annette0"].satisfaction + " / " 
                + GlobalControls.NPCList["annette0"].totalSatisfaction;
            carlosSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["carlos0"].satisfaction + " / " 
                + GlobalControls.NPCList["carlos0"].totalSatisfaction;
            bobSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["bob0"].satisfaction + " / " 
                + GlobalControls.NPCList["bob0"].totalSatisfaction;

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

            if (!annetteInteracted)
            {
                annetteImage.SetActive(false);
                annetteSatisfaction.SetActive(false);
            }
            
            if (!carlosInteracted)
            {
                carlosImage.SetActive(false);
                carlosSatisfaction.SetActive(false);
            }
            
            if (!bobInteracted)
            {
                bobImage.SetActive(false);
                bobSatisfaction.SetActive(false);
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
        else if (name.Equals("annette0"))
        {
            annetteSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["annette0"].satisfaction + " / " 
                + GlobalControls.NPCList["annette0"].totalSatisfaction;
            annetteInteracted = true;
            GlobalControls.AnnetteInteracted = true;
            GlobalControls.NPCList["annette0"].interracted = true;
            annetteImage.SetActive(true);
            annetteSatisfaction.SetActive(true);
        }
        else if (name.Equals("carlos0"))
        {
            carlosSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["carlos0"].satisfaction + " / " 
                + GlobalControls.NPCList["carlos0"].totalSatisfaction;
            carlosInteracted = true;
            GlobalControls.CarlosInteracted = true;
            GlobalControls.NPCList["carlos0"].interracted = true;
            carlosImage.SetActive(true);
            carlosSatisfaction.SetActive(true);
        }
        else if (name.Equals("bob0"))
        {
            bobSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["bob0"].satisfaction + " / " 
                + GlobalControls.NPCList["bob0"].totalSatisfaction;
            bobInteracted = true;
            GlobalControls.BobInteracted = true;
            GlobalControls.NPCList["bob0"].interracted = true;
            bobImage.SetActive(true);
            bobSatisfaction.SetActive(true);
        }
    }
}

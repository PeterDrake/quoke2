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
    public bool carlosInteracted;
    public bool bobInteracted;

    private GameObject safiImage;
    private GameObject demImage;
    private GameObject rainerImage;
    private GameObject fredImage;
    private GameObject carlosImage;
    private GameObject bobImage;
    
    private GameObject safiSatisfaction;
    private GameObject demSatisfaction;
    private GameObject rainerSatisfaction;
    private GameObject fredSatisfaction;
    private GameObject carlosSatisfaction;
    private GameObject bobSatisfaction;

    private GameObject safiOwes;
    private GameObject demOwes;
    private GameObject rainerOwes;
    private GameObject fredOwes;
    private GameObject carlosOwes;
    private GameObject bobOwes;


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
        if(carlosSatisfaction) carlosSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["carlos0"].satisfaction + " / " 
            + GlobalControls.NPCList["carlos0"].totalSatisfaction;
        if(bobSatisfaction) bobSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["bob0"].satisfaction + " / " 
            + GlobalControls.NPCList["bob0"].totalSatisfaction;
        
        if(safiOwes) safiOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["safi0"].owes.ToString();
        if(demOwes) demOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["dem0"].owes.ToString();
        if(rainerOwes) rainerOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["rainer0"].owes.ToString();
        if(fredOwes) fredOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["fred0"].owes.ToString();
        if(carlosOwes) carlosOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["carlos0"].owes.ToString();
        if(bobOwes) bobOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["bob0"].owes.ToString();
        
        if(safiOwes && GlobalControls.NPCList["safi0"].owes != 0) 
            safiOwes.SetActive(true);
        if(demOwes && GlobalControls.NPCList["dem0"].owes != 0) 
            demOwes.SetActive(true);
        if(rainerOwes && GlobalControls.NPCList["rainer0"].owes != 0) 
            rainerOwes.SetActive(true);
        if(fredOwes && GlobalControls.NPCList["fred0"].owes != 0) 
            fredOwes.SetActive(true);
        if(carlosOwes && GlobalControls.NPCList["carlos0"].owes != 0) 
            carlosOwes.SetActive(true);
        if(bobOwes && GlobalControls.NPCList["bob0"].owes != 0) 
            bobOwes.SetActive(true);
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
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.Equals("Safi Met Image")) safiImage = child.gameObject;
                else if (child.name.Equals("Dem Met Image")) demImage = child.gameObject;
                else if (child.name.Equals("Rainer Met Image")) rainerImage = child.gameObject;
                else if (child.name.Equals("Fred Met Image")) fredImage = child.gameObject;
                else if (child.name.Equals("Carlos Met Image")) carlosImage = child.gameObject;
                else if (child.name.Equals("Bob Met Image")) bobImage = child.gameObject;
                else if (child.name.Equals("Safi Satisfaction")) safiSatisfaction = child.gameObject;
                else if (child.name.Equals("Dem Satisfaction")) demSatisfaction = child.gameObject;
                else if (child.name.Equals("Rainer Satisfaction")) rainerSatisfaction = child.gameObject;
                else if (child.name.Equals("Fred Satisfaction")) fredSatisfaction = child.gameObject;
                else if (child.name.Equals("Carlos Satisfaction")) carlosSatisfaction = child.gameObject;
                else if (child.name.Equals("Bob Satisfaction")) bobSatisfaction = child.gameObject;
                else if (child.name.Equals("Safi Owes")) safiOwes = child.gameObject;
                else if (child.name.Equals("Dem Owes")) demOwes = child.gameObject;
                else if (child.name.Equals("Rainer Owes")) rainerOwes = child.gameObject;
                else if (child.name.Equals("Fred Owes")) fredOwes = child.gameObject;
                else if (child.name.Equals("Carlos Owes")) carlosOwes = child.gameObject;
                else if (child.name.Equals("Bob Owes")) bobOwes = child.gameObject;
            }

            safiInteracted = GlobalControls.SafiInteracted;
            demInteracted = GlobalControls.DemInteracted;
            rainerInteracted = GlobalControls.RainerInteracted;
            fredInteracted = GlobalControls.FredInteracted;
            carlosInteracted = GlobalControls.CarlosInteracted;
            bobInteracted = GlobalControls.BobInteracted;

            safiSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["safi0"].satisfaction + " / " 
                + GlobalControls.NPCList["safi0"].totalSatisfaction;
            demSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["dem0"].satisfaction + " / " 
                + GlobalControls.NPCList["dem0"].totalSatisfaction;
            rainerSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["rainer0"].satisfaction + " / " 
                + GlobalControls.NPCList["rainer0"].totalSatisfaction;
            fredSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["fred0"].satisfaction + " / " 
                + GlobalControls.NPCList["fred0"].totalSatisfaction;
            carlosSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["carlos0"].satisfaction + " / " 
                + GlobalControls.NPCList["carlos0"].totalSatisfaction;
            bobSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["bob0"].satisfaction + " / " 
                + GlobalControls.NPCList["bob0"].totalSatisfaction;

            safiOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["safi0"].owes.ToString();
            demOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["dem0"].owes.ToString();
            rainerOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["rainer0"].owes.ToString();
            fredOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["fred0"].owes.ToString();
            carlosOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["carlos0"].owes.ToString();
            bobOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["bob0"].owes.ToString();

            if (!safiInteracted)
            {
                safiImage.SetActive(false);
                safiSatisfaction.SetActive(false);
                safiOwes.SetActive(false);
            }
            else
            {
                safiImage.SetActive(true);
                safiSatisfaction.SetActive(true);
                if(GlobalControls.NPCList["safi0"].owes != 0) safiOwes.SetActive(true);
            }

            if (!demInteracted)
            {
                demImage.SetActive(false);
                demSatisfaction.SetActive(false);
                demOwes.SetActive(false);
            }
            else
            {
                demImage.SetActive(true);
                demSatisfaction.SetActive(true);
                if(GlobalControls.NPCList["dem0"].owes != 0) demOwes.SetActive(true);
            }

            if (!rainerInteracted)
            {
                rainerImage.SetActive(false);
                rainerSatisfaction.SetActive(false);
                rainerOwes.SetActive(false);
            }
            else
            {
                rainerImage.SetActive(true);
                rainerSatisfaction.SetActive(true);
                if(GlobalControls.NPCList["rainer0"].owes != 0) rainerOwes.SetActive(true);
            }

            if (!fredInteracted)
            {
                fredImage.SetActive(false);
                fredSatisfaction.SetActive(false);
                fredOwes.SetActive(false);
            }
            else
            {
                fredImage.SetActive(true);
                fredSatisfaction.SetActive(true);
                if(GlobalControls.NPCList["fred0"].owes != 0) fredOwes.SetActive(true);
            }
            
            if (!carlosInteracted)
            {
                carlosImage.SetActive(false);
                carlosSatisfaction.SetActive(false);
                carlosOwes.SetActive(false);
            }
            else
            {
                carlosImage.SetActive(true);
                carlosSatisfaction.SetActive(true);
                if(GlobalControls.NPCList["carlos0"].owes != 0) carlosOwes.SetActive(true);
            }
            
            if (!bobInteracted)
            {
                bobImage.SetActive(false);
                bobSatisfaction.SetActive(false);
                bobOwes.SetActive(false);
            }
            else
            {
                bobImage.SetActive(true);
                bobSatisfaction.SetActive(true);
                if(GlobalControls.NPCList["bob0"].owes != 0) bobOwes.SetActive(true);
            }
        }

    }

    public void UpdateNPCInteracted(string name)
    {
        if (name.Equals("safi0"))
        {
            safiSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["safi0"].satisfaction + " / " 
                + GlobalControls.NPCList["safi0"].totalSatisfaction;
            safiOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["safi0"].owes.ToString();
            safiInteracted = true;
            GlobalControls.SafiInteracted = true;
            GlobalControls.NPCList["safi0"].interracted = true;
            if(GlobalControls.NPCList["safi0"].owes != 0) safiOwes.SetActive(true);
            safiImage.SetActive(true);
            safiSatisfaction.SetActive(true);
        }
        else if (name.Equals("dem0"))
        {
            demSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["dem0"].satisfaction + " / " 
                + GlobalControls.NPCList["dem0"].totalSatisfaction;
            demOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["dem0"].owes.ToString();
            demInteracted = true;
            GlobalControls.DemInteracted = true;
            GlobalControls.NPCList["dem0"].interracted = true;
            if(GlobalControls.NPCList["dem0"].owes != 0) demOwes.SetActive(true);
            demImage.SetActive(true);
            demSatisfaction.SetActive(true);
        }
        else if (name.Equals("rainer0"))
        {
            rainerSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["rainer0"].satisfaction + " / " 
                + GlobalControls.NPCList["rainer0"].totalSatisfaction;
            rainerOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["rainer0"].owes.ToString();
            rainerInteracted = true;
            GlobalControls.RainerInteracted = true;
            GlobalControls.NPCList["rainer0"].interracted = true;
            if(GlobalControls.NPCList["rainer0"].owes != 0) rainerOwes.SetActive(true);
            rainerImage.SetActive(true);
            rainerSatisfaction.SetActive(true);
        }
        else if (name.Equals("fred0"))
        {
            fredSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["fred0"].satisfaction + " / " 
                + GlobalControls.NPCList["fred0"].totalSatisfaction;
            fredOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["fred0"].owes.ToString();
            fredInteracted = true;
            GlobalControls.FredInteracted = true;
            GlobalControls.NPCList["fred0"].interracted = true;
            if(GlobalControls.NPCList["fred0"].owes != 0) fredOwes.SetActive(true);
            fredImage.SetActive(true);
            fredSatisfaction.SetActive(true);
        }
        else if (name.Equals("carlos0"))
        {
            carlosSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["carlos0"].satisfaction + " / " 
                + GlobalControls.NPCList["carlos0"].totalSatisfaction;
            carlosOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["carlos0"].owes.ToString();
            carlosInteracted = true;
            GlobalControls.CarlosInteracted = true;
            GlobalControls.NPCList["carlos0"].interracted = true;
            if(GlobalControls.NPCList["carlos0"].owes != 0) carlosOwes.SetActive(true);
            carlosImage.SetActive(true);
            carlosSatisfaction.SetActive(true);
        }
        else if (name.Equals("bob0"))
        {
            bobSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["bob0"].satisfaction + " / " 
                + GlobalControls.NPCList["bob0"].totalSatisfaction;
            bobOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["bob0"].owes.ToString();
            bobInteracted = true;
            GlobalControls.BobInteracted = true;
            GlobalControls.NPCList["bob0"].interracted = true;
            if(GlobalControls.NPCList["bob0"].owes != 0) bobOwes.SetActive(true);
            bobImage.SetActive(true);
            bobSatisfaction.SetActive(true);
        }
    }
}

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

    public HashSet<string> interacted = new HashSet<string>();
    
    // public bool safiInteracted;
    // public bool demInteracted;
    // public bool rainerInteracted;
    // public bool annetteInteracted;
    // public bool carlosInteracted;
    // public bool angieInteracted;

    private GameObject safiImage;
    private GameObject demImage;
    private GameObject rainerImage;
    private GameObject annetteImage;
    private GameObject carlosImage;
    private GameObject angieImage;
    
    private GameObject safiSatisfaction;
    private GameObject demSatisfaction;
    private GameObject rainerSatisfaction;
    private GameObject annetteSatisfaction;
    private GameObject carlosSatisfaction;
    private GameObject angieSatisfaction;

    private GameObject safiOwes;
    private GameObject demOwes;
    private GameObject rainerOwes;
    private GameObject annetteOwes;
    private GameObject carlosOwes;
    private GameObject angieOwes;


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
        if(carlosSatisfaction) carlosSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["carlos0"].satisfaction + " / " 
            + GlobalControls.NPCList["carlos0"].totalSatisfaction;
        if(angieSatisfaction) angieSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["angie0"].satisfaction + " / " 
            + GlobalControls.NPCList["angie0"].totalSatisfaction;
        
        if(safiOwes) safiOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["safi0"].owes.ToString();
        if(demOwes) demOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["dem0"].owes.ToString();
        if(rainerOwes) rainerOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["rainer0"].owes.ToString();
        if(annetteOwes) annetteOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["annette0"].owes.ToString();
        if(carlosOwes) carlosOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["carlos0"].owes.ToString();
        if(angieOwes) angieOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["angie0"].owes.ToString();
        
        if(safiOwes && GlobalControls.NPCList["safi0"].owes != 0) 
            safiOwes.SetActive(true);
        else if(safiOwes && GlobalControls.NPCList["safi0"].owes == 0)
            safiOwes.SetActive(false);
        if(demOwes && GlobalControls.NPCList["dem0"].owes != 0) 
            demOwes.SetActive(true);
        else if(demOwes && GlobalControls.NPCList["dem0"].owes == 0)
            demOwes.SetActive(false);
        if(rainerOwes && GlobalControls.NPCList["rainer0"].owes != 0) 
            rainerOwes.SetActive(true);
        else if(rainerOwes && GlobalControls.NPCList["rainer0"].owes == 0)
            rainerOwes.SetActive(false);
        if(annetteOwes && GlobalControls.NPCList["annette0"].owes != 0) 
            annetteOwes.SetActive(true);
        else if(annetteOwes && GlobalControls.NPCList["annette0"].owes == 0)
            annetteOwes.SetActive(false);
        if(carlosOwes && GlobalControls.NPCList["carlos0"].owes != 0) 
            carlosOwes.SetActive(true);
        else if(carlosOwes && GlobalControls.NPCList["carlos0"].owes == 0)
            carlosOwes.SetActive(false);
        if(angieOwes && GlobalControls.NPCList["angie0"].owes != 0) 
            angieOwes.SetActive(true);
        else if(angieOwes && GlobalControls.NPCList["angie0"].owes == 0)
            angieOwes.SetActive(false);
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
                else if (child.name.Equals("Annette Met Image")) annetteImage = child.gameObject;
                else if (child.name.Equals("Carlos Met Image")) carlosImage = child.gameObject;
                else if (child.name.Equals("Angie Met Image")) angieImage = child.gameObject;
                else if (child.name.Equals("Safi Satisfaction")) safiSatisfaction = child.gameObject;
                else if (child.name.Equals("Dem Satisfaction")) demSatisfaction = child.gameObject;
                else if (child.name.Equals("Rainer Satisfaction")) rainerSatisfaction = child.gameObject;
                else if (child.name.Equals("Annette Satisfaction")) annetteSatisfaction = child.gameObject;
                else if (child.name.Equals("Carlos Satisfaction")) carlosSatisfaction = child.gameObject;
                else if (child.name.Equals("Angie Satisfaction")) angieSatisfaction = child.gameObject;
                else if (child.name.Equals("Safi Owes")) safiOwes = child.gameObject;
                else if (child.name.Equals("Dem Owes")) demOwes = child.gameObject;
                else if (child.name.Equals("Rainer Owes")) rainerOwes = child.gameObject;
                else if (child.name.Equals("Annette Owes")) annetteOwes = child.gameObject;
                else if (child.name.Equals("Carlos Owes")) carlosOwes = child.gameObject;
                else if (child.name.Equals("Angie Owes")) angieOwes = child.gameObject;
            }

            if (GlobalControls.SafiInteracted)
            {
                interacted.Add("safi");
            }
            else
            {
                interacted.Remove("safi");
            }
            
            if (GlobalControls.DemInteracted)
            {
                interacted.Add("dem");
            }
            else
            {
                interacted.Remove("dem");
            }
            
            if (GlobalControls.RainerInteracted)
            {
                interacted.Add("rainer");
            }
            else
            {
                interacted.Remove("rainer");
            }
            
            if (GlobalControls.AnnetteInteracted)
            {
                interacted.Add("annette");
            }
            else
            {
                interacted.Remove("annette");
            }
            
            if (GlobalControls.CarlosInteracted)
            {
                interacted.Add("carlos");
            }
            else
            {
                interacted.Remove("carlos");
            }
            
            if (GlobalControls.AngieInteracted)
            {
                interacted.Add("angie");
            }
            else
            {
                interacted.Remove("angie");
            }
                
            // safiInteracted = GlobalControls.SafiInteracted;
            // demInteracted = GlobalControls.DemInteracted;
            // rainerInteracted = GlobalControls.RainerInteracted;
            // annetteInteracted = GlobalControls.AnnetteInteracted;
            // carlosInteracted = GlobalControls.CarlosInteracted;
            // angieInteracted = GlobalControls.AngieInteracted;

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
            angieSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["angie0"].satisfaction + " / " 
                + GlobalControls.NPCList["angie0"].totalSatisfaction;

            safiOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["safi0"].owes.ToString();
            demOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["dem0"].owes.ToString();
            rainerOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["rainer0"].owes.ToString();
            annetteOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["annette0"].owes.ToString();
            carlosOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["carlos0"].owes.ToString();
            angieOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["angie0"].owes.ToString();

            if (!interacted.Contains("safi"))
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
                else if (GlobalControls.NPCList["safi0"].owes == 0) safiOwes.SetActive(false);
            }

            if (!interacted.Contains("dem"))
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
                else if (GlobalControls.NPCList["dem0"].owes == 0) demOwes.SetActive(false);
            }

            if (!interacted.Contains("rainer"))
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
                else if (GlobalControls.NPCList["rainer0"].owes == 0) rainerOwes.SetActive(false);
            }

            if (!interacted.Contains("annette"))
            {
                annetteImage.SetActive(false);
                annetteSatisfaction.SetActive(false);
                annetteOwes.SetActive(false);
            }
            else
            {
                annetteImage.SetActive(true);
                annetteSatisfaction.SetActive(true);
                if(GlobalControls.NPCList["annette0"].owes != 0) annetteOwes.SetActive(true);
                else if (GlobalControls.NPCList["annette0"].owes == 0) annetteOwes.SetActive(false);
            }
            
            if (!interacted.Contains("carlos"))
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
                else if (GlobalControls.NPCList["carlos0"].owes == 0) carlosOwes.SetActive(false);
            }
            
            if (!interacted.Contains("angie"))
            {
                angieImage.SetActive(false);
                angieSatisfaction.SetActive(false);
                angieOwes.SetActive(false);
            }
            else
            {
                angieImage.SetActive(true);
                angieSatisfaction.SetActive(true);
                if(GlobalControls.NPCList["angie0"].owes != 0) angieOwes.SetActive(true);
                else if (GlobalControls.NPCList["angie0"].owes == 0) angieOwes.SetActive(false);
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
            interacted.Add("safi");
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
            interacted.Add("dem");
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
            interacted.Add("rainer");
            GlobalControls.RainerInteracted = true;
            GlobalControls.NPCList["rainer0"].interracted = true;
            if(GlobalControls.NPCList["rainer0"].owes != 0) rainerOwes.SetActive(true);
            rainerImage.SetActive(true);
            rainerSatisfaction.SetActive(true);
        }
        else if (name.Equals("annette0"))
        {
            annetteSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["annette0"].satisfaction + " / " 
                + GlobalControls.NPCList["annette0"].totalSatisfaction;
            interacted.Add("annette");
            GlobalControls.AnnetteInteracted = true;
            GlobalControls.NPCList["annette0"].interracted = true;
            annetteImage.SetActive(true);
            annetteSatisfaction.SetActive(true);

            annetteOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["annette0"].owes.ToString();
            if(GlobalControls.NPCList["annette0"].owes != 0) annetteOwes.SetActive(true);
        }
        else if (name.Equals("carlos0"))
        {
            carlosSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["carlos0"].satisfaction + " / " 
                + GlobalControls.NPCList["carlos0"].totalSatisfaction;
            carlosOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["carlos0"].owes.ToString();
            interacted.Add("carlos");
            GlobalControls.CarlosInteracted = true;
            GlobalControls.NPCList["carlos0"].interracted = true;
            if(GlobalControls.NPCList["carlos0"].owes != 0) carlosOwes.SetActive(true);
            carlosImage.SetActive(true);
            carlosSatisfaction.SetActive(true);
        }
        else if (name.Equals("angie0"))
        {
            angieSatisfaction.GetComponent<Text>().text = GlobalControls.NPCList["angie0"].satisfaction + " / " 
                + GlobalControls.NPCList["angie0"].totalSatisfaction;
            angieOwes.GetComponentInChildren<Text>(true).text = GlobalControls.NPCList["angie0"].owes.ToString();
            interacted.Add("angie");
            GlobalControls.AngieInteracted = true;
            GlobalControls.NPCList["angie0"].interracted = true;
            if(GlobalControls.NPCList["angie0"].owes != 0) angieOwes.SetActive(true);
            angieImage.SetActive(true);
            angieSatisfaction.SetActive(true);
        }
    }
}

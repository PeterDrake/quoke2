using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPCInteracted : MonoBehaviour
{
    private ReferenceManager referenceManager;

    private List<Transform> panels;
    
    // private GameObject safiImage;
    // private GameObject demImage;
    // private GameObject rainerImage;
    // private GameObject annetteImage;
    // private GameObject carlosImage;
    // private GameObject angieImage;
    //
    // private GameObject safiSatisfaction;
    // private GameObject demSatisfaction;
    // private GameObject rainerSatisfaction;
    // private GameObject annetteSatisfaction;
    // private GameObject carlosSatisfaction;
    // private GameObject angieSatisfaction;
    //
    // private GameObject safiOwes;
    // private GameObject demOwes;
    // private GameObject rainerOwes;
    // private GameObject annetteOwes;
    // private GameObject carlosOwes;
    // private GameObject angieOwes;


    private void OnEnable()
    {
        panels = new List<Transform>();
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (child.name.EndsWith("Panel"))
            {
                panels.Add(child);
            }
        }

        for (int i = 0; i < panels.Count; i++)
        {
            string name = GlobalControls.npcNames[i];
            NPC npc = GlobalControls.npcList[name];
            // Update display of how satisfied this NPC is
            Text satisfaction = panels[i].Find("Satisfaction").GetComponent<Text>();
            satisfaction.text = npc.satisfaction + " / " + npc.totalSatisfaction;
            // Update display of how much this NPC owes us
            Text owes = panels[i].Find("Owes").Find("Number").GetComponent<Text>();
            owes.text = npc.owes.ToString();
            // Activate or deactivate components depending on whether we've interacted with this NPC
            GameObject image = panels[i].Find("Frame").Find("Image").gameObject;
            Debug.Log("image in Start: " + image);
            image.SetActive(true);
            satisfaction.gameObject.SetActive(npc.interacted);
            owes.gameObject.SetActive(npc.owes != 0);
        }
        // if(safiSatisfaction) safiSatisfaction.GetComponent<Text>().text = GlobalControls.npcList["safi0"].satisfaction + " / " 
        //     + GlobalControls.npcList["safi0"].totalSatisfaction;
        // if(demSatisfaction) demSatisfaction.GetComponent<Text>().text = GlobalControls.npcList["dem0"].satisfaction + " / " 
        //     + GlobalControls.npcList["dem0"].totalSatisfaction;
        // if(rainerSatisfaction) rainerSatisfaction.GetComponent<Text>().text = GlobalControls.npcList["rainer0"].satisfaction + " / " 
        //     + GlobalControls.npcList["rainer0"].totalSatisfaction;
        // if(annetteSatisfaction) annetteSatisfaction.GetComponent<Text>().text = GlobalControls.npcList["annette0"].satisfaction + " / " 
        //     + GlobalControls.npcList["annette0"].totalSatisfaction;
        // if(carlosSatisfaction) carlosSatisfaction.GetComponent<Text>().text = GlobalControls.npcList["carlos0"].satisfaction + " / " 
        //     + GlobalControls.npcList["carlos0"].totalSatisfaction;
        // if(angieSatisfaction) angieSatisfaction.GetComponent<Text>().text = GlobalControls.npcList["angie0"].satisfaction + " / " 
        //     + GlobalControls.npcList["angie0"].totalSatisfaction;
        //
        // if(safiOwes) safiOwes.GetComponentInChildren<Text>(true).text = GlobalControls.npcList["safi0"].owes.ToString();
        // if(demOwes) demOwes.GetComponentInChildren<Text>(true).text = GlobalControls.npcList["dem0"].owes.ToString();
        // if(rainerOwes) rainerOwes.GetComponentInChildren<Text>(true).text = GlobalControls.npcList["rainer0"].owes.ToString();
        // if(annetteOwes) annetteOwes.GetComponentInChildren<Text>(true).text = GlobalControls.npcList["annette0"].owes.ToString();
        // if(carlosOwes) carlosOwes.GetComponentInChildren<Text>(true).text = GlobalControls.npcList["carlos0"].owes.ToString();
        // if(angieOwes) angieOwes.GetComponentInChildren<Text>(true).text = GlobalControls.npcList["angie0"].owes.ToString();
        //
        // if(safiOwes && GlobalControls.npcList["safi0"].owes != 0) 
        //     safiOwes.SetActive(true);
        // else if(safiOwes && GlobalControls.npcList["safi0"].owes == 0)
        //     safiOwes.SetActive(false);
        // if(demOwes && GlobalControls.npcList["dem0"].owes != 0) 
        //     demOwes.SetActive(true);
        // else if(demOwes && GlobalControls.npcList["dem0"].owes == 0)
        //     demOwes.SetActive(false);
        // if(rainerOwes && GlobalControls.npcList["rainer0"].owes != 0) 
        //     rainerOwes.SetActive(true);
        // else if(rainerOwes && GlobalControls.npcList["rainer0"].owes == 0)
        //     rainerOwes.SetActive(false);
        // if(annetteOwes && GlobalControls.npcList["annette0"].owes != 0) 
        //     annetteOwes.SetActive(true);
        // else if(annetteOwes && GlobalControls.npcList["annette0"].owes == 0)
        //     annetteOwes.SetActive(false);
        // if(carlosOwes && GlobalControls.npcList["carlos0"].owes != 0) 
        //     carlosOwes.SetActive(true);
        // else if(carlosOwes && GlobalControls.npcList["carlos0"].owes == 0)
        //     carlosOwes.SetActive(false);
        // if(angieOwes && GlobalControls.npcList["angie0"].owes != 0) 
        //     angieOwes.SetActive(true);
        // else if(angieOwes && GlobalControls.npcList["angie0"].owes == 0)
        //     angieOwes.SetActive(false);
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
            panels = new List<Transform>();
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.EndsWith("Panel"))
                {
                    panels.Add(child);
                }
            }

            for (int i = 0; i < panels.Count; i++)
            {
                string name = GlobalControls.npcNames[i];
                NPC npc = GlobalControls.npcList[name];
                // Update display of how satisfied this NPC is
                Text satisfaction = panels[i].Find("Satisfaction").GetComponent<Text>();
                satisfaction.text = npc.satisfaction + " / " + npc.totalSatisfaction;
                // Update display of how much this NPC owes us
                Text owes = panels[i].Find("Owes").Find("Number").GetComponent<Text>();
                owes.text = npc.owes.ToString();
                // Activate or deactivate components depending on whether we've interacted with this NPC
                GameObject image = panels[i].Find("Frame").Find("Image").gameObject;
                Debug.Log("image in Start: " + image);
                // image.SetActive(npc.interacted);
                image.SetActive(true);
                satisfaction.gameObject.SetActive(npc.interacted);
                owes.gameObject.SetActive(npc.owes != 0);
            }
        }
    }

    public void UpdateNPCInteracted(string name)
    {
        Debug.Log("Interacted with " + name);
        NPC npc = GlobalControls.npcList[name];
        Debug.Log("This npc is " + npc.name);
        int i = Array.IndexOf(GlobalControls.npcNames, name);
        Debug.Log("i is " + i);
        // Update display of how satisfied this NPC is
        Text satisfaction = panels[i].Find("Satisfaction").GetComponent<Text>();
        satisfaction.text = npc.satisfaction + " / " + npc.totalSatisfaction;
        // Update display of how much this NPC owes us
        Text owes = panels[i].Find("Owes").Find("Number").GetComponent<Text>();
        owes.text = npc.owes.ToString();
        // Note that we've interacted with this NPC
        npc.interacted = true;
        // Activate components
        GameObject image = panels[i].Find("Frame").Find("Image").gameObject;
        Debug.Log("image in UpdateNPCInteracted: " + image);
        image.SetActive(true);
        satisfaction.gameObject.SetActive(true);
        owes.gameObject.SetActive(npc.owes != 0);
    }
}

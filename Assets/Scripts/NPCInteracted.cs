using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPCInteracted : MonoBehaviour
{
    private ReferenceManager referenceManager;

    private List<Transform> panels;
    
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
                image.SetActive(npc.interacted);
                satisfaction.gameObject.SetActive(npc.interacted);
                owes.gameObject.SetActive(npc.owes != 0);
            }
        }
    }

    public void UpdateNPCInteracted(string name)
    {
        NPC npc = GlobalControls.npcList[name];
        int i = Array.IndexOf(GlobalControls.npcNames, name);
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
        image.SetActive(true);
        satisfaction.gameObject.SetActive(true);
        owes.gameObject.SetActive(npc.owes != 0);
    }
}

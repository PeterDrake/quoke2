using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPCInteracted : MonoBehaviour
{
    private ReferenceManager referenceManager;

    private List<Transform> panels;

    private int selectedSlotNumber;
    
    public Sprite unselectedSlotSprite;
    public Sprite selectedSlotSprite;
    public Sprite unselectedSlotSpriteInUse;
    public Sprite selectedSlotSpriteInUse;
    
    
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
                Transform satisfaction = panels[i].Find("Satisfaction").GetComponent<Transform>();
                // Update display of how much this NPC owes us
                Text owes = panels[i].Find("Owes").Find("Number").GetComponent<Text>();
                owes.text = npc.owes.ToString();
                // Activate or deactivate components depending on whether we've interacted with this NPC
                GameObject image = panels[i].Find("Frame").Find("Image").gameObject;
                image.SetActive(npc.interacted);
                satisfaction.gameObject.SetActive(npc.interacted);
                UpdateSatisfactionStars(npc, i);
                owes.gameObject.SetActive(npc.owes != 0);
            }
        }
    }

    private void UpdateSatisfactionStars(NPC npc, int index)
    {
        List<Transform> stars = new List<Transform>();
        foreach (Transform child in panels[index].Find("Satisfaction").GetComponentsInChildren<Transform>(true))
        {
                stars.Add(child);
        }
        for (int i = 0; i <= npc.totalSatisfaction; i++)
        {
            //Debug.Log("TOTAL SATISFACTION = " + npc.totalSatisfaction);
            //Debug.Log("NPC SATISFACTION = " + npc.satisfaction);
            if(npc.satisfaction >= i)
            {
                Debug.Log(npc.name + " satisfaction up!");
                stars[i].gameObject.SetActive(true);

            }
            else
            {
                Debug.Log(npc.name + " satisfaction down...");
                stars[i].gameObject.SetActive(false);
            }

        }

    }
    public void UpdateNPCInteracted(string name)
    {
        Debug.Log("name: " + name);
        NPC npc = GlobalControls.npcList[name];
        int i = Array.IndexOf(GlobalControls.npcNames, name);
        // Update display of how satisfied this NPC is
        
        Transform satisfaction = panels[i].Find("Satisfaction").GetComponent<Transform>();
        // Update display of how much this NPC owes us
        Text owes = panels[i].Find("Owes").Find("Number").GetComponent<Text>();
        owes.text = npc.owes.ToString();
        // Note that we've interacted with this NPC
        npc.interacted = true;
        // Activate components
        GameObject image = panels[i].Find("Frame").Find("Image").gameObject;
        image.SetActive(true);
        satisfaction.gameObject.SetActive(true);
        UpdateSatisfactionStars(npc, i);
        owes.gameObject.SetActive(npc.owes != 0);
    }
    
    public void SelectSlotNumber(int slotNumber)
    {
        if (selectedSlotNumber != slotNumber)
        {
            panels[slotNumber].Find("Frame").GetComponent<Image>().sprite = selectedSlotSpriteInUse;
            panels[selectedSlotNumber].Find("Frame").GetComponent<Image>().sprite = unselectedSlotSpriteInUse;
            selectedSlotNumber = slotNumber;
        }

        // UpdateTooltip();
    }

    public void UpdateTooltip()
    {
        
    }
}

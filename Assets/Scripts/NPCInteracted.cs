using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPCInteracted : MonoBehaviour
{
    public Dictionary<string, NPCUIElement> npcDictionary = new Dictionary<string, NPCUIElement>();

    private void OnEnable()
    {
        foreach(var npc in npcDictionary)
        {
            if(npc.Value.satisfaction) npc.Value.satisfaction.GetComponent<Text>().text =
                GlobalControls.NPCList[npc.Key].satisfaction + " / " 
                + GlobalControls.NPCList[npc.Key].totalSatisfaction;
            
            if (npc.Value.owes) npc.Value.owes.GetComponentInChildren<Text>(true).text =
                GlobalControls.NPCList[npc.Key].owes.ToString();
            
            if (npc.Value.owes && GlobalControls.NPCList[npc.Key].owes != 0)
                npc.Value.owes.SetActive(true);
            else if(npc.Value.owes && GlobalControls.NPCList[npc.Key].owes == 0)
                npc.Value.owes.SetActive(false);
        }
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
            // Add all the current NPCs in the game to the dictionary
            string[] npcs = new string[6] {"safi0", "dem0", "rainer0", "annette0", "carlos0", "angie0"};
            foreach (var npcName in npcs)
            {
                npcDictionary.Add(npcName, new NPCUIElement());
            }

            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.Equals("Safi Met Image")) npcDictionary["safi0"].image = child.gameObject;
                else if (child.name.Equals("Dem Met Image")) npcDictionary["dem0"].image = child.gameObject;
                else if (child.name.Equals("Rainer Met Image")) npcDictionary["rainer0"].image = child.gameObject;
                else if (child.name.Equals("Annette Met Image")) npcDictionary["annette0"].image = child.gameObject;
                else if (child.name.Equals("Carlos Met Image")) npcDictionary["carlos0"].image = child.gameObject;
                else if (child.name.Equals("Angie Met Image")) npcDictionary["angie0"].image = child.gameObject;
                else if (child.name.Equals("Safi Satisfaction")) npcDictionary["safi0"].satisfaction = child.gameObject;
                else if (child.name.Equals("Dem Satisfaction")) npcDictionary["dem0"].satisfaction = child.gameObject;
                else if (child.name.Equals("Rainer Satisfaction")) npcDictionary["rainer0"].satisfaction = child.gameObject;
                else if (child.name.Equals("Annette Satisfaction")) npcDictionary["annette0"].satisfaction = child.gameObject;
                else if (child.name.Equals("Carlos Satisfaction")) npcDictionary["carlos0"].satisfaction = child.gameObject;
                else if (child.name.Equals("Angie Satisfaction")) npcDictionary["angie0"].satisfaction = child.gameObject;
                else if (child.name.Equals("Safi Owes")) npcDictionary["safi0"].owes = child.gameObject;
                else if (child.name.Equals("Dem Owes")) npcDictionary["dem0"].owes = child.gameObject;
                else if (child.name.Equals("Rainer Owes")) npcDictionary["rainer0"].owes = child.gameObject;
                else if (child.name.Equals("Annette Owes")) npcDictionary["annette0"].owes = child.gameObject;
                else if (child.name.Equals("Carlos Owes")) npcDictionary["carlos0"].owes = child.gameObject;
                else if (child.name.Equals("Angie Owes")) npcDictionary["angie0"].owes = child.gameObject;
            }

            npcDictionary["safi0"].interacted = GlobalControls.SafiInteracted;
            npcDictionary["dem0"].interacted = GlobalControls.DemInteracted;
            npcDictionary["rainer0"].interacted = GlobalControls.RainerInteracted;
            npcDictionary["annette0"].interacted = GlobalControls.AnnetteInteracted;
            npcDictionary["carlos0"].interacted = GlobalControls.CarlosInteracted;
            npcDictionary["angie0"].interacted = GlobalControls.AngieInteracted;

            foreach(var npc in npcDictionary)
            {
                NPCUIElement npcUIElement = npc.Value;
                npcUIElement.satisfaction.GetComponent<Text>().text =
                    GlobalControls.NPCList[npc.Key].satisfaction + " / " 
                    + GlobalControls.NPCList[npc.Key].totalSatisfaction;
                
                npcUIElement.owes.GetComponentInChildren<Text>(true).text =
                    GlobalControls.NPCList[npc.Key].owes.ToString();
                
                
                if (!npcUIElement.interacted)
                {
                    npcUIElement.image.SetActive(false);
                    npcUIElement.satisfaction.SetActive(false);
                    npcUIElement.owes.SetActive(false);
                }
                else
                {
                    npcUIElement.image.SetActive(true);
                    npcUIElement.satisfaction.SetActive(true);
                    if(GlobalControls.NPCList[npc.Key].owes != 0) npcUIElement.owes.SetActive(true);
                    else if (GlobalControls.NPCList[npc.Key].owes == 0) npcUIElement.owes.SetActive(false);
                }
            }
        }

    }

    public void UpdateNPCInteracted(string name)
    {
        NPCUIElement npcUIElement;
        if (npcDictionary.TryGetValue(name, out npcUIElement))
        {
            npcUIElement.satisfaction.GetComponent<Text>().text = GlobalControls.NPCList[name].satisfaction + " / " 
                + GlobalControls.NPCList[name].totalSatisfaction;
            npcUIElement.owes.GetComponentInChildren<Text>(true).text =
                GlobalControls.NPCList[name].owes.ToString();
            npcUIElement.interacted = true;
            
            switch (name)
            {
                case "safi0":
                    GlobalControls.SafiInteracted = true;
                    break; 
                case "dem0":
                    GlobalControls.DemInteracted = true;
                    break;
                case "rainer0":
                    GlobalControls.RainerInteracted = true;
                    break;
                case "annette0":
                    GlobalControls.AnnetteInteracted = true;
                    break;
                case "carlos0":
                    GlobalControls.CarlosInteracted = true;
                    break;
                case "angie0":
                    GlobalControls.AngieInteracted = true;
                    break;
            }
            
            GlobalControls.NPCList[name].interracted = true;
            if(GlobalControls.NPCList[name].owes != 0) npcUIElement.owes.SetActive(true);
            npcUIElement.image.SetActive(true);
            npcUIElement.satisfaction.SetActive(true);
        }
    }
}

public class NPCUIElement
{
    public bool interacted;
    public GameObject image;
    public GameObject satisfaction;
    public GameObject owes;
}
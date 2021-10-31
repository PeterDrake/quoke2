using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    private ReferenceManager referenceManager;
    private GameObject toolTips;
    private GameObject objectives;
    private Text tooltipText;
    private Text npcInventoryTooltipName;
    private Image[] npcInventoryTooltipSprites;
    private GameObject npcInventoryTooltip;
    private Text[] npcInventoryTooltipItemName;
    public Text pointsText;
    
    private Sprite unselected;
    private Sprite selected;

    // Start is called before the first frame update
    void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (child.gameObject.name.Equals("Points")) pointsText = child.gameObject.GetComponentInChildren<Text>();
        }
        
        unselected = Resources.Load<Sprite>("UnselectedSlot 1");
        selected = Resources.Load<Sprite>("SelectedSlot 1");
    }

    public void HandleTooltip()
    {
        if (GlobalControls.globalControlsProperties.Contains("tooltipsEnabled"))
        {
            npcInventoryTooltipSprites = new Image[4];
            npcInventoryTooltipItemName = new Text[4];
            int i = 0;
            int j = 0;
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (child.gameObject.name.Equals("Tooltip"))
                {
                    toolTips = child.gameObject;
                    tooltipText = child.gameObject.GetComponentInChildren<Text>(true);
                }
                else if (child.gameObject.name.Equals("Objectives"))
                {
                    objectives = child.gameObject;
                }
                else if (child.gameObject.name.Equals("NPC Inventory"))
                {
                    npcInventoryTooltip = child.gameObject;
                    npcInventoryTooltipName = child.gameObject.GetComponentInChildren<Text>(true);
                }
                else if (child.gameObject.name.Contains("NPC Inventory"))
                {
                    if (child.gameObject.name.Equals("NPC Inventory")) continue;
                    if (child.gameObject.name.Contains("Image"))
                    {
                        npcInventoryTooltipSprites[i] = child.gameObject.GetComponentInChildren<Image>();
                        i++;
                    }
                    else
                    {
                        npcInventoryTooltipItemName[j] = child.gameObject.GetComponentInChildren<Text>(true);
                        j++;
                    }
                }
            }

            pointsText.text = GlobalControls.CurrentPoints.ToString();
        }
    }

    public void UpdateTooltipWithNPCInteracted(string npc)
    {
        if (GlobalControls.globalControlsProperties.Contains("tooltipsEnabled") &&
            GlobalControls.npcList[npc].interacted)
        {
            if (!tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.activeSelf)
                tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(true);
            tooltipText.text = GlobalControls.npcList[npc].description;
            npcInventoryTooltip.SetActive(true);
            npcInventoryTooltipName.text =
                GlobalControls.npcList[npc].name + "'s Inventory";
        
            for (int j = npcInventoryTooltipSprites.Length - 1; j >= 0; j--)
            {
                npcInventoryTooltipSprites[j].sprite = unselected;
                npcInventoryTooltipItemName[j].text = "";
            }

            foreach (Item item in GlobalItemList.ItemList.Values)
            {
                if (item.scene.Equals("Inventory") &&
                    item.containerName.Equals(npc))
                {
                    GameObject prefab = (GameObject) Resources.Load(item.name, typeof(GameObject));
                    Sprite sprite = prefab.GetComponent<Collectible>().sprite;
                    npcInventoryTooltipSprites[(int) item.location.x].sprite = sprite;
                    npcInventoryTooltipItemName[(int) item.location.x].text = item.name;
                }
            }
        }
        else
        {
            tooltipText.gameObject.GetComponentInParent<Image>(true).gameObject.SetActive(false);
            SetNPCInventoryTooltipInactive();
        }
    }

    public void SetNPCInventoryTooltipInactive()
    {
        npcInventoryTooltip.SetActive(false);
    }

    public void SetObjectivesActive()
    {
        objectives.SetActive(true);
    }
    
    public void SetObjectivesInactive()
    {
        objectives.SetActive(false);
    }
    
    public void SetTooltipsActive()
    {
        toolTips.SetActive(true);
    }
    
    public void SetTooltipsInactive()
    {
        toolTips.SetActive(false);
    }
}

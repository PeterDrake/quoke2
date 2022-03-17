using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractedController : MonoBehaviour
{
    private NewNPCInteracted npcInteracted;

    void Awake()
    {
        npcInteracted = GameObject.Find("Neighbors").GetComponent<NewNPCInteracted>();
    }

    public void UpdateNPCInteracted(string name)
    {
        Debug.Log(name);
        npcInteracted.NewInteraction(name);
    }

    public void EnableUI()
    {
        return;
    }
    
    public void DisableUI()
    {
        return;
    }
}

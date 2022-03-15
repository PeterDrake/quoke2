using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractedController : MonoBehaviour
{
    private NewNPCInteracted npcInteracted;

    void Start()
    {
        npcInteracted = GameObject.Find("Neighbors").GetComponent<NewNPCInteracted>();
    }

    public void UpdateNPCInteracted(string name)
    {
        npcInteracted.NewInteraction(name);
    }
}

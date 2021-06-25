using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var score = 0;
        foreach (NPC npc in GlobalControls.NPCList.Values)
        {
            score += npc.satisfaction;
        }
        
        GameObject.Find("Score Number").GetComponent<Text>().text = score.ToString();
    }
}

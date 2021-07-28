using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    private GameObject prefab;
    private ReferenceManager referenceManager;
    private DialogueManager dialogueManager;


    void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        dialogueManager = referenceManager.dialogueCanvas.GetComponent<DialogueManager>();
        
        AddDialogue("hello im duc", "Duc");
        AddDialogue("hello im NPC", "NPC");
    }

    public void AddDialogue(string dialogue, string name)
    {
        Debug.Log("Adding a new dialogue");
        prefab = (GameObject) Resources.Load("Dialogue Box");
        GameObject dialogueText;
        dialogueText = (GameObject) Instantiate(prefab, transform);
        foreach (Text text in dialogueText.GetComponentsInChildren<Text>(true))
        {
            if (text.gameObject.name.Equals("Name Text"))
            {
                text.text = name;
                if (name.Equals("Duc")) text.alignment = TextAnchor.MiddleLeft;
                else text.alignment = TextAnchor.MiddleRight;
            }

            if (text.gameObject.name.Equals("Dialogue Text"))
            {
                text.text = dialogue;
            }
        }
        
    }

    public void ClearDialogue()
    {
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
        {
            Object.Destroy(child.gameObject);
        }
    }

    public void LoadNPC(string npcName)
    {
        ClearDialogue();
        
        foreach (string node in GlobalControls.NPCList[npcName].dialogueList)
        {
            AddDialogue(node,GlobalControls.NPCList[npcName].name);
        }
    }
}

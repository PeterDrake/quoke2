using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public GameObject prefab;
    private ReferenceManager referenceManager;
    private DialogueManager dialogueManager;

    void OnEnable()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        dialogueManager = referenceManager.dialogueCanvas.GetComponent<DialogueManager>();
        
        AddDialogue("Hello world");
    }

    public void AddDialogue(string dialogue)
    {
        GameObject dialogueText;
        dialogueText = (GameObject)Instantiate(prefab, transform);
        
    }
}

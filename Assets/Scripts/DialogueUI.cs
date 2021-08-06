using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    private GameObject prefab;
    private ReferenceManager referenceManager;
    private DialogueManager dialogueManager;
    private ScrollRect scrollRect;


    void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        dialogueManager = referenceManager.dialogueCanvas.GetComponent<DialogueManager>();
        scrollRect = dialogueManager.gameObject.GetComponentInChildren<ScrollRect>(true);
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
                if (name.Equals("Duc"))
                {
                    text.alignment = TextAnchor.UpperLeft;
                    text.gameObject.GetComponentInParent<Image>().color = Color.green;
                }
                else
                {
                    text.alignment = TextAnchor.UpperRight;
                    text.gameObject.GetComponentInParent<Image>().color = Color.cyan;
                }
            }

            if (text.gameObject.name.Equals("Dialogue Text"))
            {
                text.text = dialogue;
                text.alignment = TextAnchor.UpperCenter;
            }
        }

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;

    }

    public void ClearDialogue()
    {
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
        {
            if(!child.name.Equals(this.gameObject.name))
                Object.Destroy(child.gameObject);
        }
    }

    public void LoadNPC(string npcName)
    {
        ClearDialogue();
        foreach (DialogueNode node in GlobalControls.NPCList[npcName].dialogueList)
        {
            AddDialogue(node.text,node.name);
        }
    }
}

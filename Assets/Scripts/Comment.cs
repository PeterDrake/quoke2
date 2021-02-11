// This can be attached to any GameObject as a place for a comment visible within Unity
// Idea from https://www.codeproject.com/Tips/1208852/How-to-Add-Comments-Notes-to-a-GameObject-in-Unity
using UnityEngine;

public class Comment : MonoBehaviour
{
    [TextArea] public string notes = "Type comment here.";
}

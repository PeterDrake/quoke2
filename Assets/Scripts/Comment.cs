// This can be attached to any GameObject as a place for a comment visible within Unity
// Idea from https://www.codeproject.com/Tips/1208852/How-to-Add-Comments-Notes-to-a-GameObject-in-Unity
using UnityEngine;

public class Comment : MonoBehaviour
{
    // This line is a garbage comment just to change a file
    [TextArea(minLines: 2, maxLines: 32)]
    public string notes = "Type comment here.";
}

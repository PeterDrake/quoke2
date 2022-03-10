using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{

    private GameObject helpCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        helpCanvas = GameObject.Find("Help Canvas");
        DisableHelpMenu();
    }

    public void EnableHelpMenu()
    {
        helpCanvas.SetActive(true);
    }

	public void DisableHelpMenu()
    {
        helpCanvas.SetActive(false);
    }

    public bool IsCanvasEnabled()
    {
        return helpCanvas.activeSelf;
    }
}

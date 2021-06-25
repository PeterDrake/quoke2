using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    private Text tooltipText;
    // Start is called before the first frame update
    void Start()
    {
        tooltipText = this.GetComponentInChildren<Text>();
    }

    public void DisplayTooltip()
    {
        
    }
}

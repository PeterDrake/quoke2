using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHeaterOrGasValve : MonoBehaviour
{
    private bool hasWrench;
    private bool gasDone;

    // Start is called before the first frame update
    void Start()
    {
        hasWrench = GlobalControls.globalControlsProperties.Contains("playerHasWrench");
        gasDone = GlobalControls.globalControlsProperties.Contains("safiGasDone");
    }

    private bool GasDone()
    {
        return hasWrench;
    }
    
    private bool HeaterDone()
    {
        return hasWrench && gasDone;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Score Number").GetComponent<Text>().text = GlobalControls.CurrentPoints.ToString();
    }
}

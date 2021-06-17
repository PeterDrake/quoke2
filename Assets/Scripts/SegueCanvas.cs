using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegueCanvas : MonoBehaviour
{
    public ReferenceManager referenceManager;
    // Start is called before the first frame update
    void Start()
    {
        referenceManager = GameObject.Find("Managers").GetComponent<ReferenceManager>();
        referenceManager.keyboardManager.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            referenceManager.keyboardManager.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}

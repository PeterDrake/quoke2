using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float panSpeed = 20f;

    void Update ()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(Vector3.left,Vector3.up);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(Vector3.right,Vector3.up);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(Vector3.forward,Vector3.up);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(Vector3.back,Vector3.up);
        }
        
    }
    
    
}

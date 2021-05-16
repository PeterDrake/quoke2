using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMover : MonoBehaviour
{
    public Movement PlayerMover;

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(Input.GetAxisRaw("Horizontal")) >= 1f)
        {
            PlayerMover.MoveHorizontally(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f));
        } else if (Math.Abs(Input.GetAxisRaw("Vertical")) >= 1f)
        {
            PlayerMover.MoveHorizontally(new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")));
        }
        else
        {
            PlayerMover.SetMoving(false);
        }
    }
}

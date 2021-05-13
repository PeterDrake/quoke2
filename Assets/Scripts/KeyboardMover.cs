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
            PlayerMover.MoveHorizontally(Input.GetAxisRaw("Horizontal"));
        } else if (Math.Abs(Input.GetAxisRaw("Vertical")) >= 1f)
        {
            PlayerMover.MoveVertically(Input.GetAxisRaw("Vertical"));
        }
        else
        {
            PlayerMover.SetMoving(false);
        }
    }
}

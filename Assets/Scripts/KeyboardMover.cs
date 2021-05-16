using System;
using UnityEngine;

/// <summary>
/// Handles keyboard input related to moving the player.
/// </summary>
public class KeyboardMover : MonoBehaviour
{
    public PlayerMover Player;

    // Update is called once per frame
    void Update()
    {
        Player.SetCrouching(Input.GetKey(KeyCode.C));
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (Math.Abs(h) >= 1f)
        {
            Player.StartMoving(new Vector3(h, 0f, 0f));
            GlobalControls.TurnNumber++;
        } else if (Math.Abs(v) >= 1f)
        {
            Player.StartMoving(new Vector3(0f, 0f, v));
            GlobalControls.TurnNumber++;
        }
    }
}

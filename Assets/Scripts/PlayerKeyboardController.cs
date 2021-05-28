using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Handles keyboard input related to moving the player.
/// </summary>
public class PlayerKeyboardController : MonoBehaviour
{
    public PlayerMover player;

    public Inventory inventory;
    
    // Note that the 1 key is at index 0, and so on. This neatly accounts for 0-based array index and doesn't have to be
    // accounted for elsewhere.
    private readonly KeyCode[] validInputs = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
    
    // Update is called once per frame
    void Update()
    {
        // Crouch (c)
        player.SetCrouching(Input.GetKey(KeyCode.C));
        // Move (wasd or arrow keys)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (Math.Abs(h) >= 1f)
        {
            player.StartMoving(new Vector3(h, 0f, 0f));
        } else if (Math.Abs(v) >= 1f)
        {
            player.StartMoving(new Vector3(0f, 0f, v));
        }
        // Select from inventory (1-9)
        for (int i = 0; i < validInputs.Length; i++)
        {
            if (inventory && Input.GetKey(validInputs[i]))
            {
                inventory.SelectSlotNumber(i);
            }
        }
        // Pick up / drop (space)
        if (inventory && Input.GetKeyDown(KeyCode.Space))
        {
            inventory.PickUpOrDrop();
        }
    }
}

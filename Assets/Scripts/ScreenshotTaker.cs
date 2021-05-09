// Adapted from: https://www.youtube.com/watch?v=nKLHw3dIegE
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    
    /// <summary>
    /// Saves a screenshot (in the top-level directory for this project) when the user presses 'k'.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ScreenCapture.CaptureScreenshot("screenshot.png");
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

    public string FirstScene;

    public void RestartGame()
    {
        GlobalControls.PoopTimeLeft = 24;
        GlobalControls.WaterTimeLeft = 12;
        GlobalControls.PoopTaskCompleted = false;
        GlobalControls.WaterTaskCompleted = false;
        GlobalControls.TurnNumber = 0;
        GlobalItemList.Reset();
        SceneManager.LoadScene(FirstScene);
    }
}

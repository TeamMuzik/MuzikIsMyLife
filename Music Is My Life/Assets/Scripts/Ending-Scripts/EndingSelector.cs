using System.Collections;
using UnityEngine;

public class EndingSelector : MonoBehaviour
{
    public SceneMove sceneMoveScript;

    void Start()
    {
        SelectEnding();
    }

    public void SelectEnding() // 엔딩으로
    {
        int money = PlayerPrefs.GetInt("Money");
        int myFame = PlayerPrefs.GetInt("MyFame");
        int bandFame = PlayerPrefs.GetInt("BandFame");
        string endingScene;

        if (money >= 150 && myFame >= 75 && bandFame >= 150)
        {
            endingScene = "Ending-Collaboration";
        }
        else if (money >= 150 && myFame >= 75)
        {
            endingScene = "Ending-Entertainment";
        }
        else if (myFame >= 75 && bandFame >= 150)
        {
            endingScene = "Ending-BackStage";
        }
        else if (money >= 150 && bandFame >= 150)
        {
            endingScene = "Ending-WanDuck";
        }
        else if (money >= 150)
        {
            endingScene = "Ending-Expedition";
        }
        else if (myFame >= 120)
        {
            endingScene = "Ending-ExcellentMyFame";
        }
        else if (myFame >= 75)
        {
            endingScene = "Ending-GreatMyFame";
        }
        else if (myFame >= 50)
        {
            endingScene = "Ending-NormalMyFame";
        }
        else if (bandFame >= 200)
        {
            endingScene = "Ending-ExcellentBandFame";
        }
        else if (bandFame >= 150)
        {
            endingScene = "Ending-GreatBandFame";
        }
        else if (bandFame >= 100)
        {
            endingScene = "Ending-NormalBandFame";
        }
        else
        {
            endingScene = "Ending-Normal";
        }

        // 엔딩 잠금 해제 및 저장
        PlayerPrefs.SetString("Ending", endingScene);
        sceneMoveScript.targetScene = endingScene;
    }
}

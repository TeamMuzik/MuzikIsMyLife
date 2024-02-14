using System.Collections;
using UnityEngine;
public class MainUpdateController : MonoBehaviour
{
    public FunitureController funitureController;
    public StatusController statusController;
    public GameObject roomPanel;
    public GameObject phonePanel;
    public GameObject storePanel;

    void Start()
    {
        roomPanel.SetActive(true);
        phonePanel.SetActive(false);
        storePanel.SetActive(false);
        StatusChanger.UpdateDay(); // 날짜 업데이트
        // 14일이 지나면 15일: 엔딩으로 이동
        if (PlayerPrefs.GetInt("Dday") >= PlayerPrefs.GetInt("EndDday")) // EndDday: 15
        {
            GoToEnding();
        }
        // 스트레스가 100 이상일 경우 게임오버
        if (PlayerPrefs.GetInt("Stress") >= 100)
        {
            GoToGameOver();
        }
        funitureController.UpdateFurnitures();
        statusController.UpdateStatus();
    }

    public void UpdateAfterStore()
    {
        funitureController.UpdateFurnitures();
        statusController.UpdateStatus();
        phonePanel.SetActive(false);
    }

    public void GoToEnding() // 엔딩으로
    {
        int money = PlayerPrefs.GetInt("Money");
        int myFame = PlayerPrefs.GetInt("MyFame");
        int bandFame = PlayerPrefs.GetInt("Fame");

        SceneMove sceneMove = gameObject.AddComponent<SceneMove>();
        if (money >= 2500000)
        {
            sceneMove.targetScene = "Ending-Expedition";
        }
        else if (myFame >= 100)
        {
            sceneMove.targetScene = "Ending-OpeningBand";
        }
        else if (bandFame >= 300)
        {
            sceneMove.targetScene = "Ending-ConcertInKorea";
        }
        else
        {
            sceneMove.targetScene = "Ending-Normal";
        }
        sceneMove.ChangeScene();
    }

    public void GoToGameOver()
    {
        SceneMove sceneMove = gameObject.AddComponent<SceneMove>();
        sceneMove.targetScene = "Ending-GameOver";
        sceneMove.ChangeScene();
    }

}
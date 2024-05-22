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

        Screen.orientation = ScreenOrientation.LandscapeLeft;
        roomPanel.SetActive(true);
        phonePanel.SetActive(false);
        storePanel.SetActive(false);
        StatusChanger.UpdateDay(); // 날짜 업데이트
        // 14일이 지나면 15일: 엔딩으로 이동
        if (PlayerPrefs.GetInt("Dday") >= PlayerPrefs.GetInt("EndDday")) // EndDday: 15
        {
            SceneMove sceneMove = gameObject.AddComponent<SceneMove>();
            sceneMove.targetScene = "GoToEnding";
            sceneMove.ChangeScene();

        }
        // 스트레스가 100 이상일 경우 게임오버
        if (PlayerPrefs.GetInt("Stress") >= 100)
        {
            SceneMove sceneMove = gameObject.AddComponent<SceneMove>();
            sceneMove.targetScene = "Ending-GameOver";
            sceneMove.ChangeScene();
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

}

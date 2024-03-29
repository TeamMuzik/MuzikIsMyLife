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
            SceneMove sceneMove = gameObject.AddComponent<SceneMove>();
            sceneMove.targetScene = "Nar-GoToEnding";
            sceneMove.ChangeScene();

        }
        // 스트레스가 100 이상일 경우 게임오버
        if (PlayerPrefs.GetInt("Stress") >= 100)
        {
            GoToGameOver();
        }
        funitureController.UpdateFurnitures();
        statusController.UpdateStatus();
    }

    public void UpdateAfterStore() // Store Panel의 ExitBtn에서 호출
    {
        funitureController.UpdateFurnitures();
        statusController.UpdateStatus();
        phonePanel.SetActive(false);
    }

    private void GoToGameOver()
    {
        PlayerPrefs.SetString("Ending", "Ending-GameOver");
        SceneMove sceneMove = gameObject.AddComponent<SceneMove>();
        sceneMove.targetScene = "Ending-GameOver";
        sceneMove.ChangeScene();
    }
}
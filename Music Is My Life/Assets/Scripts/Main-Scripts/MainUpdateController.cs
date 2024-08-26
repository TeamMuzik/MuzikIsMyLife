using System.Collections;
using UnityEngine;

public class MainUpdateController : MonoBehaviour
{
    public FunitureController funitureController;
    public StatusController statusController;
    public GameObject roomPanel;
    public GameObject phonePanel;
    public GameObject storePanel;

    private bool orientationSet = false; // Update에서 한번만 변경하도록 flag 설정

    void Start()
    {
        SetLandscapeOrientation();

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

    void Update()
    {
        // Start에서 설정된 후에 한번 더 확인하여 화면 방향 설정
        if (!orientationSet)
        {
            if (Screen.orientation != ScreenOrientation.LandscapeLeft)
            {
                SetLandscapeOrientation();
            }
            else
            {
                orientationSet = true; // 올바르게 설정되면 flag를 true로 설정
            }
        }
    }

    private void SetLandscapeOrientation()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // 잠시 딜레이 후 다시 설정 (보조적인 안정화)
        StartCoroutine(EnsureLandscapeOrientation());
    }

    private IEnumerator EnsureLandscapeOrientation()
    {
        yield return new WaitForSeconds(0.5f); // 0.5초 딜레이 후 다시 설정
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void UpdateAfterStore()
    {
        funitureController.UpdateFurnitures();
        statusController.UpdateStatus();
        phonePanel.SetActive(false);
    }
}

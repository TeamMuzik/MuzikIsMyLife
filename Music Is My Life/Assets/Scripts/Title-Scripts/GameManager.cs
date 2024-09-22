using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public GameObject startPanel;
    public GameObject htpPanel;
    public GameObject htpBtn;
    public GameObject continueBtn;
    private InitializeManager initializeManager;
    private SceneMove sceneMove;

    void Start()
    {
        startPanel.SetActive(false);
        htpPanel.SetActive(false);
        if (PlayerPrefs.GetInt("Dday") == 0) {
            htpBtn.SetActive(true);
            continueBtn.SetActive(false);
        }
        else
        {
            htpBtn.SetActive(false);
            continueBtn.SetActive(true);
        }
        initializeManager = gameObject.AddComponent<InitializeManager>();
        sceneMove = gameObject.AddComponent<SceneMove>();
    }

    public void NewGame() // 새 게임
    {
        initializeManager.InitializeGamedata();
    }

    public void ContinueGame() // 이어하기: Main으로 씬 이동
    {   
        //만약 엔딩을 보고 난 후에 이어하기 버튼을 누를 경우 새 게임 버튼을 누른 것과 동일하게 적용
        if (PlayerPrefs.GetString("PreviousScene") == "Ending-End"){
            initializeManager.InitializeGamedata();
        }
        sceneMove.targetScene = "Main";
        sceneMove.ChangeScene();
    }

    public void SavePlayerName()
    {
        string playerName;
        playerName = nameInputField.text; //안되면 ToString()
        if (playerName.Length == 0 || playerName == null)
        {
            playerName = "연보라";
        }
        if (playerName.Length > 0 && playerName.Length < 11)
        {
            Debug.Log("PlayerName: "+playerName);
            PlayerPrefs.SetString("PlayerName", playerName);
            // 프롤로그로 씬 이동
            sceneMove.targetScene = "Prologue";
            sceneMove.ChangeScene();
        }
        else
        {
            Debug.Log("PlayerName이 1~10자가 아닙니다. " + playerName);
        }
    }

    public void QuitGame() // 게임 종료 버튼
    {
        Application.Quit();
    }

    public void ResetGame() // 게임 리셋: 엔딩컬렉션도 초기화
    {
        EndingCollectionManager.ResetEndingCollection();
        initializeManager.InitializeGamedata();
        htpBtn.SetActive(true);
        continueBtn.SetActive(false);
    }
}

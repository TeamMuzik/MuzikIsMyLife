using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public GameObject startPanel;
    public GameObject htpPanel;
    private InitializeManager initializeManager = new InitializeManager();

    void Start()
    {
        startPanel.SetActive(false);
        htpPanel.SetActive(false);
        initializeManager.InitializeGamedata();
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
            SceneMove sceneMove = gameObject.AddComponent<SceneMove>();
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

    public void ResetGame() // 리셋 버튼
    {
        EndingCollectionManager.ResetEndingCollection();
        initializeManager.InitializeGamedata();
    }
}

using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMP_InputField nameInputField;

    void Start()
    {
        InitializeGamedata();
    }

    public void SavePlayerName()
    {
        string playerName;
        playerName = nameInputField.text; //안되면 ToString()
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

    public void InitializeGamedata()
    {
        PlayerPrefs.DeleteAll(); // 초기화 시 기존 데이터 모두 삭제
        PlayerPrefs.SetInt("Money", 0); // 돈
        PlayerPrefs.SetInt("MyFame", 0); // 내 명성
        PlayerPrefs.SetInt("BandFame", 0); // 야옹의 명성 (추후 교체 필요)
        PlayerPrefs.SetInt("Stress", 0); // 스트레스
        PlayerPrefs.SetString("Date", "2023/12/31"); // 날짜 (하루 넘기고 시작할 예정)
        PlayerPrefs.SetInt("Dday", 0); // 몇일차인지
        // 뻔뻔지수
        PlayerPrefs.SetInt("Confidence", 0);
        // Cover Game
        PlayerPrefs.SetInt("Subscribers", 0);
        // 알바 연속
        PlayerPrefs.SetInt("PartTimeContinuity", 0);
        // 가구 세팅
        DefaultFunitureSetting();
    }

    public static void DefaultFunitureSetting()
    {
        // 정한 가구명으로...
        string[] defaultFurnitureCI = { "ROOM_0", "BED_0", "GUITAR_0", "SHELF_0", "DESK_0", "CHARACTER_0", "SOUNDEQUIP_0", "COMPUTER_0", "CHAIR_0" };
        foreach (string Category_Index in defaultFurnitureCI)
        {
            PlayerPrefs.SetInt($"{Category_Index}_IsOwned", 1);
            PlayerPrefs.SetInt($"{Category_Index}_IsEquipped", 1);
        }
    }

    public void QuitGame() // 게임 종료 버튼
    {
        Application.Quit();
    }

    public void ResetGame() // 리셋 버튼
    {
        InitializeGamedata();
    }
}

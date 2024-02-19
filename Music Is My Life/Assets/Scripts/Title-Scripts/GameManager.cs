using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public GameObject startPanel;
    public GameObject htpPanel;

    void Start()
    {
        startPanel.SetActive(false);
        htpPanel.SetActive(false);
        InitializeGamedata();
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

    public void InitializeGamedata()
    {
        var keys = PlayerPrefs.GetString("SavedEndingKeys", "").Split(',');
        foreach (var key in keys)
        {
            // "Ending"으로 시작하지 않는 키에 대해서만 삭제
            if (!key.StartsWith("Ending"))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }    // 초기화 시 기존 데이터 모두 삭제 (엔딩 데이터 제외)
        PlayerPrefs.SetInt("Money", 0); // 돈
        PlayerPrefs.SetInt("MyFame", 0); // 내 명성
        PlayerPrefs.SetInt("BandFame", 0); // 야옹의 명성 (추후 교체 필요)
        PlayerPrefs.SetInt("Stress", 0); // 스트레스
        PlayerPrefs.SetString("Date", "2023/12/31"); // 날짜 (하루 넘기고 시작할 예정)
        PlayerPrefs.SetInt("Dday", 0); // 몇일차인지
        PlayerPrefs.SetInt("EndDday", 15); // 15일차에 종료

        // Cover Game
        PlayerPrefs.SetInt("Subscribers", 0); // 구독자 수
        PlayerPrefs.SetInt("Subs_Min", 500); // 구독자 수 최소치
        PlayerPrefs.SetInt("Subs_Max", 5000); // 구독자 수 최대치
        PlayerPrefs.SetInt("Subs_Multiplier", 1); // 구독자 증가 배율

        // Jjirasi Game
        PlayerPrefs.SetInt("JjirasiClick", 0);

        // 알바 연속
        PlayerPrefs.SetInt("PartTimeContinuity", 0);

        // 기본 가구 세팅 & 상점 구입 가능 물품 세팅
        FunitureInitializeSetting();

        // DayBehavior 초기화
        int endDday = PlayerPrefs.GetInt("EndDday");
        for (int d = 1; d < endDday; d++)
        {
            PlayerPrefs.SetInt("Day" + d + "_Behavior", -1); // -1로 초기화함
        }

        // 플레이어 기본 이름 (프롤로그, Ending-End 테스트 용으로 삽입)
        PlayerPrefs.SetString("PlayerName", "연보라");
    }

    public static void FunitureInitializeSetting()
    {
        // 디폴트 가구들: index가 0
        string[] defaultFurniture = { "ROOM_0", "BED_0", "GUITAR_0", "SHELF_0", "DESK_0", "CHARACTER_0", "MIC_0", "COMPUTER_0", "CHAIR_0", "CARPET_0" };
        SetFurnitureData(defaultFurniture, 1);

        // 바닥 물건들
        string[] floorThings = { "FLOOR_GUITARBAG", "FLOOR_AIRPLANE", "FLOOR_SNACK", "FLOOR_JJIRASI", "FLOOR_COKE", "FLOOR_TRASH" };
        SetFurnitureData(floorThings, 0);

        // 음향기기들
        // 기타 (1~2), 컴퓨터 (1), 마이크 (1), 이펙터 (1~6), 페달보드 (1), 오인페 (1)
        string[] audioProducts = { "GUITAR_1", "GUITAR_2", "COMPUTER_1", "MIC_1", "EF_1", "EF_2", "EF_3", "EF_4", "EF_5", "EF_6", "PDBD_1", "AUIN_1" };
        SetFurnitureData(audioProducts, 0);

        // 굿즈들
        // CD (1~14)
        string[] cds = { "CD_CURRENT", "CD_1", "CD_2", "CD_3", "CD_4", "CD_5", "CD_6", "CD_7", "CD_8", "CD_9", "CD_10", "CD_11", "CD_12", "CD_13", "CD_14" };
        SetFurnitureData(cds, 0);
        // LP (1~6; SHOP)
        string[] lps = { "LP_CURRENT", "LP_1", "LP_2", "LP_3", "LP_4", "LP_5", "LP_6", "LP_SHOP" };
        SetFurnitureData(lps, 0);
        // 포스터 (1~10)
        string[] posters = { "POSTER_CURRENT", "POSTER_1", "POSTER_2", "POSTER_3", "POSTER_4", "POSTER_5", "POSTER_6", "POSTER_7", "POSTER_8", "POSTER_9", "POSTER_10" };
        SetFurnitureData(posters, 0);

        // replaceable settings
        string[] categories = {"CHARACTER_CURRENT", "GUITAR_CURRENT", "MIC_CURRENT", "COMPUTER_CURRENT", "BED_CURRENT" };
        foreach (string category in categories)
        {
            PlayerPrefs.SetInt(category, 0); // 0으로 세팅
        }
    }

    private static void SetFurnitureData(string[] furnitureNames, int stat)
    {
        foreach (string Category_Index in furnitureNames)
        {
            PlayerPrefs.SetInt($"{Category_Index}_IsOwned", stat);
            PlayerPrefs.SetInt($"{Category_Index}_IsEquipped", stat);
        }
    }

    public void QuitGame() // 게임 종료 버튼
    {
        Application.Quit();
    }

    public void ResetGame() // 리셋 버튼
    {
        EndingCollectionManager.ResetEndingCollection();
        InitializeGamedata();
    }
}

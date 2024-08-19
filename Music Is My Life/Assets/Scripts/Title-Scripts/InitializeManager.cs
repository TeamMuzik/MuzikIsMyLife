using TMPro;
using UnityEngine;

public class InitializeManager : MonoBehaviour
{
    private string[] datelist = { "2024/3/1", "2024/8/1", "2024/12/1" }; // 이 날짜부터 시작하는 것으로 변경 (하루 뒤x)
    private const int endDday = 15;

    public void InitializeGamedata()
    {
        // 초기화 시 기존 데이터 모두 삭제 (엔딩 데이터 제외)
        var keys = PlayerPrefs.GetString("SavedEndingKeys", "").Split(',');
        foreach (var key in keys)
        {
            // "Ending"으로 시작하지 않는 키에 대해서만 삭제
            if (!key.StartsWith("Ending"))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }

        // 스프라이트 데이터 리셋
        SpriteUtils.ResetAllSavedSprites();

        // 플레이어 기본 이름
        PlayerPrefs.SetString("PlayerName", "연보라");

        // Season마다 초기화되는 데이터 초기화
        PlayerPrefs.SetInt("SeasonNum", 0);
        InitializeForNextSeason();

        // 기본 가구, 변경되는 기본 가구, 교체 가능한 가구, 상점 물품 세팅
        InitializeDefaultFurniture();
        InitializeRepFurniture();
        InitializeStoreProducts();

        // 알바별 누적 번 돈 초기화
        InitializePartTimeCumulativeIncome();
    }

    public void InitializeForNextSeason()
    {
        SpriteUtils.ResetAllSavedSprites();
        
        // 게임 기본 데이터
        PlayerPrefs.SetInt("Money", 0); // 돈
        PlayerPrefs.SetInt("MyFame", 0); // 내 명성
        PlayerPrefs.SetInt("BandFame", 0); // 야옹의 명성 (추후 교체 필요)
        PlayerPrefs.SetInt("Stress", 0); // 스트레스

        // season 번호: 1, 2, 3
        int seasonNum = PlayerPrefs.GetInt("SeasonNum") + 1;
        PlayerPrefs.SetInt("SeasonNum", seasonNum);
        PlayerPrefs.SetInt("SEASONAL_CURRENT", seasonNum - 1); // 계절 가구 선택 (0, 1, 2)
        PlayerPrefs.SetInt("SeasonEvent", 0); // 0이면 안 봄, 1~3이면 봄

        // 날짜, 디데이 관련 초기화
        PlayerPrefs.SetString("Date", datelist[seasonNum - 1]); // 날짜
        PlayerPrefs.SetInt("Dday", 0); // 몇일차인지
        PlayerPrefs.SetInt("EndDday", endDday); // 15일차에 종료

        // 행동 관련 초기화
        // 알바 연속
        PlayerPrefs.SetInt("PartTimeContinuity", 0);

        // Cover Game
        PlayerPrefs.SetInt("Subscribers", 0); // 구독자 수
        PlayerPrefs.SetInt("Subs_Min", 500); // 구독자 수 최소치
        PlayerPrefs.SetInt("Subs_Max", 5000); // 구독자 수 최대치
        PlayerPrefs.SetInt("Subs_Multiplier", 1); // 구독자 증가 배율

        // Jjirasi Game
        PlayerPrefs.SetInt("JjirasiClick", 0);

        // DayBehavior 초기화, 행운 초기화
        for (int dayNum = 1; dayNum < endDday; dayNum++)
        {
            PlayerPrefs.SetInt($"Day{dayNum}_Behavior", -1); // -1로 초기화
            PlayerPrefs.SetInt($"Day{dayNum}_Fortune", -1); // -1로 초기화
        }
    }

    private void InitializeDefaultFurniture()
    {
        // 디폴트 가구들: index가 0
        string[] defaultFurniture = { "COMPUTER_0", "GUITAR_0", "MIC_0",
            "BED_0", "CHARACTER_0", "CURTAIN_0", "WINDOW_0",
            "ROOM_0", "SHELF_0", "FLOOR_0", "CARPET_0", "CHAIR_0", "DESK_0",
        };
        SetFurnitureData(defaultFurniture, 1);
    }

    private void InitializeStoreProducts()
    {
        // 음향기기들
        // 컴퓨터 (1), 기타 (1~2), 마이크 (1), 이펙터 (1~6), 페달보드 (1), 오인페 (1)
        string[] audioProducts = { "COMPUTER_1", "GUITAR_1", "GUITAR_2", "MIC_1", "EF_1", "EF_2", "EF_3", "EF_4", "EF_5", "EF_6", "PDBD_1", "AUIN_1" };
        SetFurnitureData(audioProducts, 0);

        // 굿즈들
        // CD (1~14)
        string[] cds = { "CD_1", "CD_2", "CD_3", "CD_4", "CD_5", "CD_6", "CD_7", "CD_8", "CD_9", "CD_10", "CD_11", "CD_12", "CD_13", "CD_14" };
        // LP (1~6)
        string[] lps = { "LP_1", "LP_2", "LP_3", "LP_4", "LP_5", "LP_6"};
        // 포스터 (1~10)
        string[] posters = { "POSTER_1", "POSTER_2", "POSTER_3", "POSTER_4", "POSTER_5", "POSTER_6", "POSTER_7", "POSTER_8", "POSTER_9", "POSTER_10" };
        SetFurnitureData(cds, 0);
        SetFurnitureData(lps, 0);
        SetFurnitureData(posters, 0);
    }

    private void SetFurnitureData(string[] furnitureNames, int stat)
    {
        foreach (string Category_Index in furnitureNames)
        {
            PlayerPrefs.SetInt($"{Category_Index}_IsOwned", stat);
            // PlayerPrefs.SetInt($"{Category_Index}_IsEquipped", stat);
        }
    }

    private void InitializeRepFurniture()
    {
        // replaceable한 가구들의 초기 인덱스를 0으로 세팅
        string[] categories = {"COMPUTER_CURRENT", "GUITAR_CURRENT", "MIC_CURRENT",
            "BED_CURRENT", "CHARACTER_CURRENT", "CURTAIN_CURRENT", "WINDOW_CURRENT",
            "CD_CURRENT", "LP_CURRENT", "POSTER_CURRENT" };
        foreach (string category in categories)
        {
            PlayerPrefs.SetInt(category, 0); // 0으로 세팅
        }
    }

    private void InitializePartTimeCumulativeIncome()
    {
        PlayerPrefs.SetInt("CumulativeIncome_Cafe", 0);
        PlayerPrefs.SetInt("CumulativeIncome_Office", 0);
        PlayerPrefs.SetInt("CumulativeIncome_Factory", 0);
    }
}

using UnityEngine;
using System;

public class StatusChanger : MonoBehaviour
{
    // 돈 벌음
    public static void EarnMoney(int income)
    {
        if (income < 0)
            throw new ArgumentException("income 값이 0 미만입니다.");
        int money = PlayerPrefs.GetInt("Money");
        int balance = money + income;
        Debug.Log(balance);
        if (balance < 0)
            throw new Exception("돈이 정수 범위를 벗어남");
        PlayerPrefs.SetInt("Money", balance);
    }

    // 돈 소비함 (양수로 보냄)
    public static bool SpendMoney(int price)
    {
        if (price <= 0)
            throw new ArgumentException("price 값이 0 이하입니다.");
        int money = PlayerPrefs.GetInt("Money");
        int balance = money - price;
        if (balance < 0)
        {
            return false;
        } else {
            PlayerPrefs.SetInt("Money", balance);
            return true;
        }
    }

    // 내 명성 업데이트: 최솟값 0. change는 음수, 양수 둘다 가능
    public static void UpdateMyFame(int change)
    {
        int myFame = PlayerPrefs.GetInt("MyFame");
        myFame += change;
        if (myFame < 0)
            myFame = 0;
        PlayerPrefs.SetInt("MyFame", myFame);
    }

    // 밴드 명성 업데이트: 최솟값 0. change는 음수, 양수 둘다 가능
    public static void UpdateBandFame(int change)
    {
        int bandFame = PlayerPrefs.GetInt("BandFame");
        bandFame += change;
        if (bandFame < 0)
            bandFame = 0;
        PlayerPrefs.SetInt("BandFame", bandFame);
    }

    // 스트레스 업데이트: 최솟값 0. change는 음수, 양수 둘다 가능
    public static void UpdateStress(int change)
    {
        int stress = PlayerPrefs.GetInt("Stress");
        stress += change;
        if (stress < 0)
            stress = 0;
        PlayerPrefs.SetInt("Stress", stress);
    }

    // 날짜 업데이트
    public static void UpdateDay()
    {
        int yesterday = PlayerPrefs.GetInt("Dday");
        if (yesterday > 0)
        {
            // 어제 한 행동이 상점이거나 게임 종료(강제 종료 포함)한 경우면 날짜 업데이트 하지 않음
            int ydBehaviorId = PlayerPrefs.GetInt("Day" + yesterday + "_Behavior");
            if (ydBehaviorId == 6 || ydBehaviorId == -1)
                return;
        }
        // Dday
        int nextDday = PlayerPrefs.GetInt("Dday") + 1;
        PlayerPrefs.SetInt("Dday", nextDday);

        // Date - 어제가 0일이면 Date 업데이트 x
        if (yesterday == 0)
            return;
        string savedDate = PlayerPrefs.GetString("Date");
        DateTime nextDate = DateTime.Parse(savedDate).AddDays(1); // 날짜를 하루 뒤로 업데이트
        string updatedDate = nextDate.ToString("yyyy-MM-dd");
        PlayerPrefs.SetString("Date", updatedDate);
    }

}
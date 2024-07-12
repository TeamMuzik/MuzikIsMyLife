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

        // 알바인 경우에 누적 수입을 기록
        int todayNum = PlayerPrefs.GetInt("Dday");
        int partTimeId = PlayerPrefs.GetInt($"Day{todayNum}_Behavior");
        switch (partTimeId)
        {
            case 0:
                PlayerPrefs.SetInt("CumulativeIncome_Cafe", PlayerPrefs.GetInt("CumulativeIncome_Cafe") + income);
                break;
            case 1:
                PlayerPrefs.SetInt("CumulativeIncome_Office", PlayerPrefs.GetInt("CumulativeIncome_Office") + income);
                break;
            case 2:
                PlayerPrefs.SetInt("CumulativeIncome_Factory", PlayerPrefs.GetInt("CumulativeIncome_Factory") + income);
                break;
        }
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
    public static void UpdateDay(bool forceUpdate = false)
    {
        int yesterday = PlayerPrefs.GetInt("Dday");
        if (yesterday > 0 && !forceUpdate)
        {
            // Main 씬에 재진입하기 전에 한 행동이 상점이거나 운세, 이벤트, 게임 종료(강제 종료 포함)인 경우 날짜 업데이트 x
            // -1는 디폴트와 이벤트와 게임 종료, -2는 컴퓨터(운세, SNS)
            int ydBehaviorId = PlayerPrefs.GetInt("Day" + yesterday + "_Behavior");
            if (ydBehaviorId == 6 || ydBehaviorId == -1 || ydBehaviorId == -2)
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

        // 전날의 Fortune 효과 초기화
        DayFortune.ResetYdayFortuneEffect();
    }

    // 구독자 최솟값 업데이트: 최솟값 500. change는 음수, 양수 둘다 가능
    public static void UpdateSubsMin(int change)
    {
        int subsMin = PlayerPrefs.GetInt("Subs_Min");
        subsMin += change;
        if (subsMin < 500)
            subsMin = 500;
        PlayerPrefs.SetInt("Subs_Min", subsMin);

    }

    // 구독자 최댓값 업데이트: 최솟값 5000. change는 음수, 양수 둘다 가능
    public static void UpdateSubsMax(int change)
    {
        int subsMax = PlayerPrefs.GetInt("Subs_Max");
        subsMax += change;
        if (subsMax < 5000)
            subsMax = 5000;
        PlayerPrefs.SetInt("Subs_Max", subsMax);

    }

    // 구독자 증가배율 업데이트: 최솟값 1. change는 양수만 가능
    public static void UpdateSubsMultiplier(int rate)
    {
        int subsMultiplier = PlayerPrefs.GetInt("Subs_Multiplier");
        subsMultiplier *= rate;
        if (subsMultiplier < 1)
            subsMultiplier = 1;
        PlayerPrefs.SetInt("Subs_Multiplier", subsMultiplier);

    }
}

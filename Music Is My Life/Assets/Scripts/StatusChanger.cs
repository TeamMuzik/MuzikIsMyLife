using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class StatusChanger : MonoBehaviour
{
    public static void EarnMoney(int income)
    {
        int money = PlayerPrefs.GetInt("Money");
        int balance = money + income;
        Debug.Log(balance);
        if (balance < 0)
            throw new Exception("돈이 정수 범위를 벗어남");
        PlayerPrefs.SetInt("Money", balance);
    }

    public static bool SpendMoney(int price) // 양수로 보냄
    {
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

    public static void UpdateFame(int change) // 음수, 양수 둘다 가능
    {
        int fame = PlayerPrefs.GetInt("Fame");
        fame = fame + change;
        if (fame < 0)
            fame = 0; // 최소 0
        PlayerPrefs.SetInt("Fame", fame);
    }

    public void UpdateStress(int change)
    {
        int stress = PlayerPrefs.GetInt("Stress");
        stress = stress + change;
        if (stress < 0)
            stress = 0;  // 최소 0
        PlayerPrefs.SetInt("Stress", stress);
    }

    public static void UpdateConfidence(int change)
    {
        int confid = PlayerPrefs.GetInt("Confidence");
        confid = confid + change;
        if (confid < 0)
            confid = 0;  // 최소 0
        PlayerPrefs.SetInt("Confidence", confid);
    }

    public static void UpdateDay()
    {
        // Date
        string savedDate = PlayerPrefs.GetString("Date");
        DateTime nextDate = DateTime.Parse(savedDate).AddDays(1); // 날짜를 하루 뒤로 업데이트
        string updatedDate = nextDate.ToString("yyyy/MM/dd");
        PlayerPrefs.SetString("Date", updatedDate);
        // Dday
        int nextDday = PlayerPrefs.GetInt("Dday") - 1;
        PlayerPrefs.SetInt("Dday", nextDday);
        if (nextDday == 0)
        {
            // 원래는 엔딩 분기점으로 이동해야
            SceneManager.LoadScene("TitleScene");
        }
    }
}
using UnityEngine;

public class DayBehavior : MonoBehaviour
{
    public int behaviorId;

    public void SaveTodayBehavior()
    {
        int d = PlayerPrefs.GetInt("Dday");
        PlayerPrefs.SetInt("Day" + d + "_Behavior", behaviorId);
        Debug.Log("SaveTodayBehavior:" + behaviorId);
        if (behaviorId > 2 && behaviorId != 6){ // 선택한 app이 알바가 아니고, 상점이 아닌경우, 알바 연속 횟수 변수 0으로 초기화
            PlayerPrefs.SetInt("PartTimeContinuity", 0);
        }

        else if (behaviorId < 3)  // 선택한 app이 알바이면, 알바 연속 횟수 변수 증가
        {
            int count = PlayerPrefs.GetInt("PartTimeContinuity");
            PlayerPrefs.SetInt("PartTimeContinuity", count + 1);
        }
    }
}

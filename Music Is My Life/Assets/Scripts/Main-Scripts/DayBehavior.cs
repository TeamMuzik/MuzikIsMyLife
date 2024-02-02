using UnityEngine;

public class DayBehavior : MonoBehaviour
{
    public int behaviorId;

    public void SaveTodayBehavior()
    {
        int d = PlayerPrefs.GetInt("Dday");
        PlayerPrefs.SetInt("Day" + d + "_Behavior", behaviorId);
        Debug.Log("SaveTodayBehavior:" + behaviorId);
        if (behaviorId < 3)  // 선택한 app이 알바이면, 알바 연속 횟수 변수 증가
        {
            int count = PlayerPrefs.GetInt("PartTimeContinuity");
            PlayerPrefs.SetInt("PartTimeContinuity", count + 1);
        }
}

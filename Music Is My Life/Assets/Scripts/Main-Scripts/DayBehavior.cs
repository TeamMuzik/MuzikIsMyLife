using UnityEngine;

public class DayBehavior : MonoBehaviour
{
    public int behaviorId;

    public void SaveTodayBehavior()
    {
        int d = PlayerPrefs.GetInt("Dday");
        PlayerPrefs.SetInt("Day" + d + "_Behavior", behaviorId);
        Debug.Log("SaveTodayBehavior:" + behaviorId);
    }
}

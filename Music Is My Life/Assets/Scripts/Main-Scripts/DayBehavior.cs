using System.Collections;
using UnityEngine;

public class DayBehavior : MonoBehaviour
{
    public int behaviorId;

    public void Start()
    {
        if (behaviorId < 3) // 알바의 경우 어제 한 건 못하게
            SetByYesterdayBehavior();
    }

    public void SaveTodayBehavior()
    {
        int d = PlayerPrefs.GetInt("Dday");
        PlayerPrefs.SetInt("Day" + d + "_Behavior", behaviorId);
    }

    public void SetByYesterdayBehavior()
    {
        int yd = PlayerPrefs.GetInt("Dday") - 1;
        if (yd == 0)
            return;
        PlayerPrefs.GetInt("Day" + yd + "_Behavior");
        if (behaviorId == yd)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}

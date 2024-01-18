using UnityEngine;
using TMPro;

public class StatusController : MonoBehaviour
{
    public TMP_Text dday;
    public TMP_Text date;
    public TMP_Text status;

    void Start()
    {
        //추후 GameManager 등 통해 처음인지 파악 필요함
        StatusChanger.UpdateDay(); // 날짜 업데이트
        DdayText();
        DateText();
        StatusText();
    }

    public void DdayText()
    {
        dday.text = "D-" + PlayerPrefs.GetInt("Dday");
    }
    public void DateText()
    {
        date.text = PlayerPrefs.GetString("Date");
    }

    public void StatusText()
    {
        status.text = "돈: " + PlayerPrefs.GetInt("Money") + "원"
                    + "\n명성: " + PlayerPrefs.GetInt("Fame")
                    + "\n스트레스: " + PlayerPrefs.GetInt("Stress") + "%";
    }
}
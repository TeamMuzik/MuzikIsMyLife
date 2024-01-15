using UnityEngine;
using TMPro;

public class StatusController : MonoBehaviour
{
    public TMP_Text dday;
    public TMP_Text date;
    public TMP_Text status;

    void Start()
    {
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
        status.text = "돈: " + PlayerPrefs.GetInt("Money")
                    + "\n명성: " + PlayerPrefs.GetInt("Fame") + "원"
                    + "\n스트레스: " + PlayerPrefs.GetInt("Stress") + "%";
    }
}
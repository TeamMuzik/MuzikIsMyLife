using UnityEngine;
using TMPro;

public class StatusController : MonoBehaviour
{
    public TMP_Text status;

    void Start()
    {
        StatusText();
    }

    public void StatusText()
    {
        status.text = "돈: " + PlayerPrefs.GetInt("Money")
                    + "\n명성: " + PlayerPrefs.GetInt("Fame") + "원"
                    + "\n스트레스: " + PlayerPrefs.GetInt("Stress") + "%";
    }
}
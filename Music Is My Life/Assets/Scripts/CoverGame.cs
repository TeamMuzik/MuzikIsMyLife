using UnityEngine;
using TMPro;

public class CoverGame : MonoBehaviour
{
    int subs; //구독자수
    public TMP_Text msg;

    public void AddSubcribers()
    {
        //subs = PlayerPrefs.GetInt("Subscribers", 0); //구독자수
        int newSubs = Random.Range(1, 11) * 500;
        int totalSubs = subs + newSubs;
        //PlayerPrefs.SetInt("Subscribers", subs);
        msg.text = "구독자 수가 " + newSubs + "명 증가하여\n" + totalSubs + "명이 되었습니다.\n";
        if (totalSubs / 5000 > subs / 5000)
        {
            PlayerPrefs.SetInt("Fame", PlayerPrefs.GetInt("Fame") + 1);
            msg.text += "명성이 1 증가했습니다.\n";
        }
        subs += newSubs;
    }
    void Start()
    {
        subs = 0;
        //PlayerPrefs.SetInt("Subscribers", subs);
        msg.text = "";
    }

}

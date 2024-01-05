using UnityEngine;
using TMPro;

public class BeggingGame : MonoBehaviour
{
    public TMP_Text msg;

    public void BegForMoney()
    {
        int newMoney;
        float p = Random.value;
        if (p < 0.05f)
        {
            newMoney = 200000000;
            msg.text = "2억을 얻었습니다. 해피엔딩!";
            Debug.Log("5%의 확률 성공!");
        }
        else
        {
            newMoney = Random.Range(2, 11) * 5000;
            msg.text = newMoney+"원을 얻었습니다.\n";
        }
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + newMoney);
    }
    void Start()
    {
        msg.text = "";
    }

}

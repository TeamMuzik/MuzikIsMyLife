using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BeggingGame : MonoBehaviour
{
    public TMP_Text moneyStatus;
    public TMP_Text message;
    public GameObject finishBtn; // 필드로 버튼 오브젝트 받음
    public int income;
    public int count;
    public bool isStart = true;

    public void BegForMoney() // 우선 대화창을 누르면 가능
    {
        if (count == -1 || count == 5) // 구걸 게임 종료
        {
            finishBtn.SetActive(true); // 나가기 버튼 
            if (count == -1)
                finishBtn.GetComponent<SceneMove>().targetScene = "Ending-Rich";
            else
                finishBtn.GetComponent<SceneMove>().targetScene = "Main";
            return;
        }
        Debug.Log("함수 실행");
        float p = Random.value;
        if (p < 0.01f)
        {
            income = 2000000000;
            message.text = "20억을 얻었습니다!";
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + income);
            moneyStatus.text = "획득한 돈: " + income + "\n보유한 돈:" + PlayerPrefs.GetInt("Money");
            Debug.Log("1%의 확률 성공!");
            count = -1;
            return;
        }
        else
        {
            income = Random.Range(2, 11) * 5000;
            message.text = income + "원을 얻었습니다.\n";
            moneyStatus.text = "획득한 돈: " + income + "\n보유한 돈:" + PlayerPrefs.GetInt("Money");
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + income);
            Debug.Log("돈: " + PlayerPrefs.GetInt("Money") + "->" + PlayerPrefs.GetInt("Money") + income + "원을 얻었습니다.");
            count++;
        }
    }
    void Start()
    {
        income = 0;
        count = 0;
        message.text = "구걸을 시작합니다.";
        moneyStatus.text = "획득한 돈: "+ income + "\n보유한 돈:" + PlayerPrefs.GetInt("Money");
        finishBtn.SetActive(false); // 나가기 버튼 비활성화
        if (isStart == false)
            StatusChanger.updateDay();
        else
            isStart = true;
    }

}

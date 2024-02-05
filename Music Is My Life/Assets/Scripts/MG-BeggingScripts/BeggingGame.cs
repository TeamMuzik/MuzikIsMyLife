using UnityEngine;
using TMPro;

public class BeggingGame : MonoBehaviour
{
    public TMP_Text moneyStatus;
    public TMP_Text dialTitle;
    public TMP_Text dialContent;
    public GameObject scorePanel; // 점수판
    public TMP_Text resultContent; // 점수 결과

    private int income;
    private int newMoney;
    private int turn;

    void Start()
    {
        income = 0;
        newMoney = 0;
        turn = 5; // 5에서 0까지 감소
        dialTitle.text = "";
        dialContent.text = "구걸을 시작합니다. (클릭해서 시작)";
        moneyStatus.text = "획득한 돈: " + income;
        scorePanel.SetActive(false); // 결과 보기 비활성화
    }

    public void BegForMoney() // 우선 대화창을 누르면 가능
    {
        dialTitle.text = "구걸중...";
        if (turn == 0) // 버튼 클릭하지 못하도록
            return;

        float p = Random.value;
        Debug.Log("확률: "+p);
        if (p < 0.02f) // 2%의 확률로 1조
        {
            turn = 0;
            dialContent.text = "1조를 얻었습니다!";
            moneyStatus.text = "획득한 돈: 1조 " + income +"만원";
            Debug.Log("2%의 확률 성공. Ending-Rich를 봅니다.");
            // 1조의 경우 PlayerPrefs에는 저장하지 않음
            StatusChanger.EarnMoney(income);
            resultContent.text = "획득한 돈: 1조 " + income + "만원\n" + "나의 자산: 1조 "+ PlayerPrefs.GetInt("Money") + "만원";
            scorePanel.GetComponent<SceneMove>().targetScene = "Ending-Rich";
            scorePanel.SetActive(true); // 결과 보기
        }
        else
        {
            turn--;
            newMoney = Random.Range(1, 6);
            dialContent.text = newMoney + "만원을 얻었습니다.\n";
            income += newMoney;
            moneyStatus.text = "획득한 돈: " + income + "만원";
            Debug.Log("income: " + income + " | newMoney:" + newMoney + " | turnLeft: " + turn);
            if (turn == 0) // 부자 엔딩 나지 않고 5회 진행 -> 구걸 게임 종료
            {
                StatusChanger.EarnMoney(income);
                resultContent.text = "획득한 돈: " + income + "만원\n" + "나의 자산: " + PlayerPrefs.GetInt("Money") + "만원";
                scorePanel.GetComponent<SceneMove>().targetScene = "Main";
                scorePanel.SetActive(true); // 결과 보기
            }
        }
    }
}

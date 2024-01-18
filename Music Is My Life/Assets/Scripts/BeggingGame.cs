using UnityEngine;
using TMPro;

public class BeggingGame : MonoBehaviour
{
    public TMP_Text moneyStatus;
    public TMP_Text message;
    public GameObject finishBtn; // 필드로 버튼 오브젝트 받음

    private int income;
    private int newMoney;
    private int turn;

    void Start()
    {
        income = 0;
        newMoney = 0;
        turn = 5; // 5에서 0까지 감소
        message.text = "구걸을 시작합니다.";
        moneyStatus.text = "획득한 돈: " + income;
        finishBtn.SetActive(false); // 나가기 버튼 비활성화
        finishBtn.GetComponent<SceneMove>().targetScene = "Main";
    }

    public void BegForMoney() // 우선 대화창을 누르면 가능
    {
        if (turn == 0) // 버튼 클릭하지 못하도록
            return;

        float p = Random.value;
        if (p < 0.02f) // 2%의 확률로 20억
        {
            turn = 0;
            newMoney = 2000000000;
            message.text = "20억을 얻었습니다!";
            income += newMoney;
            moneyStatus.text = "획득한 돈: " + income;
            Debug.Log("2%의 확률 성공. Ending-Rich를 봅니다.");
            finishBtn.GetComponent<SceneMove>().targetScene = "Ending-Rich";
        }
        else
        {
            turn--;
            newMoney = Random.Range(2, 11) * 5000;
            message.text = newMoney + "원을 얻었습니다.\n";
            income += newMoney;
            moneyStatus.text = "획득한 돈: " + income;
            Debug.Log("income: " + income + " | newMoney:" + newMoney + " | turnLeft: " + turn);
        }
        if (turn == 0) // 엔딩 남 or 5회 진행함 -> 구걸 게임 종료
        {
            StatusChanger.EarnMoney(income);
            finishBtn.SetActive(true); // 나가기 버튼 활성화
        }
    }
}

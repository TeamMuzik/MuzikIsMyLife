using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class BeggingGame : MonoBehaviour
{
    public TMP_Text moneyStatus;
    public TMP_Text fameStatus;
    public TMP_Text dialTitle;
    public TMP_Text dialContent;
    public GameObject scorePanel; // 점수판
    public TMP_Text resultContent; // 점수 결과
    public Button button;

    private int income;
    private int turn;
    private int myFame;
    private int fameDiff;

    void Start()
    {
        income = 0;
        turn = 5; // 5에서 0까지 감소
        myFame = PlayerPrefs.GetInt("MyFame");
        fameDiff = 0;

        SetMoneyFameStatusText();
        dialTitle.text = "";
        dialContent.text = "서울역에서 구걸을 한다.\n(클릭해서 진행)";
        scorePanel.SetActive(false); // 결과 보기 비활성화
    }

    public void TryBegging() // 우선 대화창을 누르면 가능
    {
        dialTitle.text = "구걸중...";
        if (turn <= 0) // 버튼 클릭하지 못하도록
            return;

        float p = Random.value; // 팬을 만날 확률, 혹은 돈을 벌 확률
        Debug.Log("확률: " + p);

        turn--; // 턴 감소

        if (p < 0.01f) // 1%의 확률로 1조
        {
            turn = -1;
            BecameRich();
        }
        else if (myFame >= 100 && p < 0.30f) // 명성이 100 이상이고 30%
        {
            Debug.Log("구걸 게임오버");
            SceneManager.LoadScene("Ending-TooFamousToBeg"); // 추후 Beggine Game만의 GameOver 씬으로 바꿔야 함
        }
        else if (myFame >= 40 && p < 0.15f) // 명성이 40 이상이고 15%
        {
            MeetFan(); // 15%의 확률로 팬을 만남
        }
        else if (myFame >= 20 && p < 0.05f) // 명성이 20 이상이고 5%
        {
            PeopleNoticed(); // 5%의 확률로 행인이 알아봄
        }
        else
        {
            BegForMoneyNormal(p);
        }

        SetMoneyFameStatusText();
        if (turn == 0) // 부자 엔딩 나지 않고 5회 진행 -> 구걸 게임 종료
        {
            StatusChanger.EarnMoney(income);
            StatusChanger.UpdateMyFame(fameDiff);
            //resultContent.text = "번 돈: " + income + "만원\n" + "나의 돈: " + PlayerPrefs.GetInt("Money") + "만원\n" + "나의 명성: " + myFame;
            resultContent.text = "나의 돈: " + PlayerPrefs.GetInt("Money") + "만원 (+" + income + "만원)\n" + "나의 명성: " + PlayerPrefs.GetInt("MyFame");
            if (fameDiff < 0)
                resultContent.text += " (" + fameDiff + ")";
            else if (fameDiff > 0)
                resultContent.text += " (+" + fameDiff + ")";
            scorePanel.GetComponent<SceneMove>().targetScene = "Main";
            scorePanel.SetActive(true); // 결과 보기
        }
    }

    public void BecameRich()
    {
        // 1조의 경우 PlayerPrefs에는 저장하지 않음

        StatusChanger.EarnMoney(income);
        StatusChanger.UpdateMyFame(fameDiff);
        SceneMove sceneMove = gameObject.AddComponent<SceneMove>();
        sceneMove.targetScene = "Ending-Rich";
        PlayerPrefs.SetString("Ending", "Ending-Rich");


        sceneMove.ChangeScene();
    }

    // 결과창에 띄우는 구버전: 호출x
    /*public void BecameRichInBeggingResult()
    {
        dialContent.text = "1조를 얻었습니다!";
        Debug.Log("2%의 확률 성공. Ending-Rich를 봅니다.");
        moneyStatus.text = "번 돈: 1조 " + income + "만원";
        if (fameDiff != 0)
            fameStatus.text += "나의 명성: " + (myFame + fameDiff) + " (" + fameDiff + ")";
        else
            fameStatus.text = "나의 명성: " + (myFame + fameDiff);
        //moneyStatus.text = "번 돈: 1조 " + income + "만원\n" + "나의 돈: 1조 " + PlayerPrefs.GetInt("Money") + "만원\n" + "나의 명성: " + myFame;
        //moneyStatus.text = "나의 돈: 1조 " + (PlayerPrefs.GetInt("Money") + income) + "만원 (+1조" + income + ")\n" + "나의 명성: " + myFame;

        resultContent.text = "나의 돈: 1조 " + PlayerPrefs.GetInt("Money") + "만원 (+" + income + "만원)\n" + "나의 명성: " + PlayerPrefs.GetInt("MyFame");
        if (fameDiff < 0)
            resultContent.text += " (" + fameDiff + ")";
        else if (fameDiff > 0)
            resultContent.text += " (+" + fameDiff + ")";
        scorePanel.GetComponent<SceneMove>().targetScene = "Ending-Rich";
        scorePanel.SetActive(true); // 결과 보기
    }*/

    public void MeetFan() // 팬을 만나면 명성이 상승. 현재 수치: 5
    {
        Debug.Log("팬을 만남");
        int increase = 5;
        dialContent.text = "\"저 유튜브 잘보고 있어요...! 이것도 컨텐츠예요?\"\n" + "팬과 셀카를 같이 찍었다. (나의 명성 +" + increase + ")";
        fameDiff += increase;
        MakeButtonWait();
    }

    public void PeopleNoticed() // 행인이 알아보면 명성이 하락. 현재 수치: 5
    {
        Debug.Log("행인이 알아봄");
        int decrease = 5;
        dialContent.text = "\"저 사람 그 커버 유튜버 아니야...? 뭐 하는 거지?\"\n" + "나는 황급히 자리를 피했다. (나의 명성 -" + decrease + ")";
        fameDiff -= decrease;
        MakeButtonWait();
    }

    public void BegForMoneyNormal(float p) // 구걸해 돈 벌기: 우선 대화창을 누르면 가능
    {
        int newMoney = Random.Range(0, 4); // 0~3만원 획득
        dialContent.text = "지나가는 사람에게 구걸을 했다.\n" + newMoney + "만원을 얻었다.\n";
        income += newMoney;
        Debug.Log("income: " + income + " | newMoney:" + newMoney + " | turnLeft: " + turn);
    }

    public void SetMoneyFameStatusText() // status 텍스트 설정
    {
        moneyStatus.text = "번 돈: " + income + "만원";
        if (fameDiff < 0)
            fameStatus.text = "나의 명성: " + (myFame + fameDiff) + " (" + fameDiff + ")";
        else if (fameDiff > 0)
            fameStatus.text = "나의 명성: " + (myFame + fameDiff) + " (+" + fameDiff + ")";
        else
            fameStatus.text = "나의 명성: " + (myFame + fameDiff);
        //moneyStatus.text = "번 돈 : " + income + "만원\n" + "나의 돈: " + (PlayerPrefs.GetInt("Money") + income) + "만원\n" + "나의 명성: " + myFame;
        //moneyStatus.text = "나의 돈: " + (PlayerPrefs.GetInt("Money") + income) + "만원 (+" + income + "만원)\n" + "나의 명성: " + myFame;
    }

    IEnumerator MakeButtonWait()
    {
        button.interactable = false;
        yield return new WaitForSeconds(1f);
        button.interactable = true;
    }
}

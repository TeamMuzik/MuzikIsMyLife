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
    public GameObject dialImage; //대화창
    public GameObject clickSpace1; //기존 클릭 공간
    public GameObject clickSpace2; //대화창 클릭 공간
    public GameObject scorePanel; // 점수판
    public TMP_Text resultContent; // 점수 결과
    public Button button;
    public GameObject[] choiceButtons; //0번 동정심, 1번 설득, 3번 묘기
    

    private int income;
    private int turn;
    private int myFame;
    private int fameDiff;

    private bool sym = false;
    private bool per = false;
    private bool trick = false;

    RectTransform dialContentRectTransform;

    private int fortuneId;
    private string isFortune;
    void Start()
    {
        income = 0;
        turn = 4; // 4에서 0까지 감소
        myFame = PlayerPrefs.GetInt("MyFame");
        fameDiff = 0;

        SetMoneyFameStatusText();
        dialTitle.text = "";
        dialContent.text = "서울역에서 구걸을 한다.\n무엇을 해볼까?\n(클릭해서 진행)";
        scorePanel.SetActive(false); // 결과 보기 비활성화

        dialContentRectTransform  = dialContent.GetComponent<RectTransform>(); //다이얼로그 txt 위치값 가져오기
        dialImage.SetActive(false); //대화창 비활성화
        foreach (GameObject btn in choiceButtons) //선택지 버튼들 비활성화
            btn.SetActive(false);
        
        fortuneId = DayFortune.GetTodayFortuneId();
        Debug.Log("운세번호: " + fortuneId);
        isFortune = "";
        
    }

    public void showChoice()
    {
        clickSpace1.SetActive(false); //기존의 클릭 공간 비활성화
        dialImage.SetActive(false); //대화창 이미지 비활성화
        clickSpace2.SetActive(false); //대화창 클릭 공간 비활성화
        dialContent.text = "";
        dialContentRectTransform.anchoredPosition = new Vector2 (0, -200);

        foreach (GameObject btn in choiceButtons) //선택지 버튼들 활성화
            btn.SetActive(true);
    }

    public void TryBegging() // 우선 대화창을 누르면 가능
    {
        // Debug.Log("if문 전 turn: " + turn);
        if (turn <= 0) // 버튼 클릭하지 못하도록
            return;
        
        if (turn == 3) //액션 1회 수행 후 다시 선택지 보여주기
        {   
            turn--;
            showChoice();
            return;
        }

        if (turn == 1) // 부자 엔딩 나지 않고 4회 진행 -> 구걸 게임 종료
        {
            dialContent.text = "";
            dialImage.SetActive(false);

            if (fortuneId == 10)
            {
                income = 0;
                isFortune = "앗 모든 돈을 도둑 맞았다...\n";
            }

            StatusChanger.EarnMoney(income);
            StatusChanger.UpdateMyFame(fameDiff);
            //resultContent.text = "번 돈: " + income + "만원\n" + "나의 돈: " + PlayerPrefs.GetInt("Money") + "만원\n" + "나의 명성: " + myFame;
            resultContent.text = isFortune + "나의 돈: " + PlayerPrefs.GetInt("Money") + "만원 (+" + income + "만원)\n" + "나의 명성: " + PlayerPrefs.GetInt("MyFame");

            if (fameDiff < 0)
                resultContent.text += " (" + fameDiff + ")";
            else if (fameDiff > 0)
                resultContent.text += " (+" + fameDiff + ")";

            scorePanel.GetComponent<SceneMove>().targetScene = "Main";
            scorePanel.SetActive(true); // 결과 보기

            return;
        }

        turn--;
        // Debug.Log("if문 후 turn: " +turn);
        dialTitle.text = "구걸중...";
        float p = Random.value; // 팬을 만날 확률, 혹은 돈을 벌 확률
        Debug.Log("확률: " + p);


        if(fortuneId == 5) //오늘의 운세 5번일 경우 - 2%의 확률로 1조 
            if (p < 0.02f)
            {
                turn = -1;
                BecameRich();
            }

        if (p < 0.01f) // 1%의 확률로 1조
        {
            turn = -1;
            BecameRich();
        }
        else if (myFame >= 50 && p < 0.30f) // 명성이 50 이상이고 30%
        {
            Debug.Log("구걸 게임오버");
            SceneManager.LoadScene("Ending-Narak");
        }
        else if (myFame >= 40 && p < 0.15f) // 명성이 40 이상이고 15%
        {
            MeetFan(); // 15%의 확률로 팬을 만남
        }
        else if (myFame >= 20 && p < 0.05f) // 명성이 20 이상이고 5%
        {
            PeopleNoticed(); // 5%의 확률로 행인이 알아봄
        }
        else if (p < 0.1f) // 10%의 확률로 대성공
        {
            BegForMoneyBigSuccess();
            Debug.Log("대성공");
        }
        else if (p < 0.4f) // 40%의 확률로 성공
        {
            BegForMoneySuccess();
            Debug.Log("성공");
        }
        else // 50%의 확률로 성공
        {
            BegForMoneyFail();
            Debug.Log("실패");
        }

        SetMoneyFameStatusText();

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

    public void BegForMoneyBigSuccess()
    {
        int newMoney = 3; //3만원 획득
        income += newMoney;
        if (sym) //동정심 버튼을 누른 경우
            dialContent.text = "아유 불쌍해라...맛난거라도 사드세요\n" + newMoney + "만원 획득\n";
        else if (per) //설득 버튼을 누른 경우
            dialContent.text = "꼭 야옹을 보시길! 응원할게요 저도요!\n" + "많은 사람들을 설득하는데 성공했다\n" + newMoney + "만원 획득\n";
        else if (trick) //묘기 버튼을 누른 경우
            dialContent.text = "짝짝짝, 와아아~\n" + "저글링 5개를 성공했다\n" + newMoney + "만원 획득\n";
    }

    public void BegForMoneySuccess()
    {
        int newMoney = 1; //1만원 획득
        income += newMoney;
        if (sym)
            dialContent.text = "잔돈이라도 받으세요\n" + newMoney + "만원 획득\n";
        else if (per)
            dialContent.text = "화이팅~\n" + "사람들을 조금 설득하는데 성공한 것 같다\n" + newMoney + "만원 획득\n";
        else if (trick)
            dialContent.text = "짝짝 짝짝\n" + "저글링 3개를 성공했다\n" + newMoney + "만원 획득\n";
    }

    public void BegForMoneyFail()
    {
        int newMoney = 0; //0만원 획득
        income += newMoney;
        if (sym)
            dialContent.text = "아무도 관심을 주지 않았다\n" + newMoney + "만원 획득\n";
        else if (per)
            dialContent.text = "신종 사이비인가?\n" + "설득에 실패했다\n" + newMoney + "만원 획득\n";
        else if (trick)
            dialContent.text = "모두 바닥에 떨어뜨렸다...\n" + newMoney + "만원 획득\n";
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

    public void Sympathy()
    {
        sym = true;
        foreach (GameObject btn in choiceButtons) //선택지 버튼들 비활성화
            btn.SetActive(false);

        dialImage.SetActive(true); //대화창 이미지 활성화
        clickSpace2.SetActive(true); //대화창 클릭 공간 활성화
        dialContent.text = "한푼 줍쇼...한푼 줍쇼...";
    }

    public void Persuade()
    {
        per = true;
        foreach (GameObject btn in choiceButtons) //선택지 버튼들 비활성화
            btn.SetActive(false);

        dialImage.SetActive(true); //대화창 이미지 활성화
        clickSpace2.SetActive(true); //대화창 클릭 공간 활성화
        dialContent.text = "여러분!\n" + "지금부터 왜 저에게 적선을 해주셔야하는지\n" + "이야기하도록 하겠습니다.";
    }

    public void Trick()
    {
        trick = true;
        foreach (GameObject btn in choiceButtons) //선택지 버튼들 비활성화
            btn.SetActive(false);

        dialImage.SetActive(true); //대화창 이미지 활성화
        clickSpace2.SetActive(true); //대화창 클릭 공간 활성화
        dialContent.text = "저글링을 시작한다";
    }

    IEnumerator MakeButtonWait()
    {
        button.interactable = false;
        yield return new WaitForSeconds(1f);
        button.interactable = true;
    }
}

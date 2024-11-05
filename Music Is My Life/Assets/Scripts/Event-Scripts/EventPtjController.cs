using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using static EventSpriteChanger;

public class EventPtjController : MonoBehaviour
{
    public GameObject backgroundObject;
    public Sprite[] backgroundSprites = new Sprite[3];
    public GameObject dialogObject;
    public Sprite[] dialogSprites = new Sprite[3];
    public TMP_Text dialogContent;

    public GameObject[] playerObject = new GameObject[3]; // 스프라이트를 띄울 오브젝트
    public GameObject[] extraObject = new GameObject[3];
    public Sprite[] playerSprites = new Sprite[6]; // 이미지 교체할 스프라이트
    public Sprite[] extraSprites = new Sprite[6]; // [ptjId*2 + 0], [ptjId*2 + 1]

    public GameObject nextButton; // 넘기기 위해 클릭할 버튼
    public GameObject scorePanel; // 결과창
    public TMP_Text scoreText;
    public GameObject conveyorObject;
    public Sprite[] conveyorSprites = new Sprite[5];

    private int ptjId; // 알바 유형 id (0~2)
    private string ptjName;
    private string[] firstDialogTexts = new string[2]; // 첫 다이얼로그의 텍스트 배열을 저장

    private string playerName; // 플레이어 이름
    private string extraName; // 엑스트라 이름
    private int playerScore; // 플레이어 점수
    private int extraScore; // 엑스트라 점수
    private int currentOrderIndex; // 현재 순서 번호
    private EventPtjSelector eventPtjSelector;

    void Start()
    {
        PlayerPrefs.SetInt("SeasonEvent", 1);

        eventPtjSelector = new EventPtjSelector();
        // 버튼 클릭 이벤트에 함수 등록
        nextButton.GetComponent<Button>().onClick.AddListener(OnNextButtonClicked);
        nextButton.SetActive(true);
        scorePanel.SetActive(false);
        // 컨베이어 오브젝트
        conveyorObject.SetActive(false);

        playerScore = 0;
        extraScore = 0;
        currentOrderIndex = 0;

        SetupByMostFrequentPtj();
        ShowNextTextOfFirstDialog();
        for (int i = 0; i < 3; i++)
        {
            playerObject[i].SetActive(i == ptjId);
            extraObject[i].SetActive(i == ptjId);
        }
        backgroundObject.SetActive(true);
        dialogObject.SetActive(true);
    }

    void OnNextButtonClicked()
    {
        if (currentOrderIndex == 1)
        {
            ShowNextTextOfFirstDialog();
        }
        else if (currentOrderIndex == 2)
        {
            nextButton.SetActive(false);
            dialogObject.SetActive(false);
            StartCoroutine(EventPtjCouroutine());
            if (ptjId == 2)
            {
                StartCoroutine(EventPtjConveryorCouroutine());
            }
        }
        else if (currentOrderIndex == 3)
        {
            nextButton.SetActive(false);
            playerObject[ptjId].SetActive(false);
            extraObject[ptjId].SetActive(false);
            scorePanel.SetActive(true);
        }
    }

    void ShowNextTextOfFirstDialog()
    {
        dialogContent.text = firstDialogTexts[currentOrderIndex];
        currentOrderIndex++;
    }

    void SetupByMostFrequentPtj()
    {
        ptjId = eventPtjSelector.CalculateMostFreqPtjId();
        switch (ptjId) // 알바 유형 확인
        {
            case 0:
                ptjName = "Cafe";
                extraName = "나주스";
                firstDialogTexts[0] = "알바생들 중 한 명을 매니저로 채용하려고 하는데\n누구를 뽑아야 할지…";
                firstDialogTexts[1] = "더 많은 주스를 만든 사람을 뽑도록 하겠네\n계약직이 되지 못해도 알바는 계속 할 수 있으니 걱정 말게";
                break;
            case 1:
                ptjName = "Office";
                extraName = "오피수";
                firstDialogTexts[0] = "알바생들 중 한 명을 계약직으로 채용하려고 하는데\n누구를 뽑아야 할지…";
                firstDialogTexts[1] = "더 많은 서류를 처리한 사람을 뽑도록 하겠네\n계약직이 되지 못해도 알바는 계속 할 수 있으니 걱정 말게";
                break;
            case 2:
                ptjName = "Factory";
                extraName = "조인형";
                firstDialogTexts[0] = "알바생들 중 한 명을 파트장으로 채용하려고 하는데\n누구를 뽑아야 할지…";
                firstDialogTexts[1] = "더 많은 인형를 만든 사람을 뽑도록 하겠네\n계약직이 되지 못해도 알바는 계속 할 수 있으니 걱정 말게";
                conveyorObject.SetActive(true);
                SetSprite(conveyorObject, conveyorSprites[0]);
                break;
            default:
                throw new System.MissingFieldException("ptjType이 등록되지 않았습니다.");
        }
        playerName = PlayerPrefs.GetString("PlayerName");
        SetImage(backgroundObject, backgroundSprites[ptjId]);
        SetImage(dialogObject, dialogSprites[ptjId]);
        SetSprite(playerObject[ptjId], playerSprites[ptjId * 2 + 1]);
        SetSprite(extraObject[ptjId], extraSprites[ptjId * 2 + 1]);
    }

    IEnumerator EventPtjConveryorCouroutine()
    {
        for (int i = 0; i < 4 * 8; i++)
        {
            SetSprite(conveyorObject, conveyorSprites[i % 5]);
            yield return new WaitForSeconds(0.125f);
        }
    }

    IEnumerator EventPtjCouroutine()
    {
        int ptjMoney = PlayerPrefs.GetInt("Money");
        Debug.Log(ptjMoney);
        int startPoint, endPoint;

        if (ptjMoney >= 70)
            (startPoint, endPoint) = (3, 6);
        else if (ptjMoney >= 50)
            (startPoint, endPoint) = (3, 5);
        else
            (startPoint, endPoint) = (2, 5);

        Debug.Log($"Player's startPoint: {startPoint}, endPoint: {endPoint}");
        Debug.Log($"Extra's startPoint: 2, endPoint: 5");

        for (int i = 0; i < 8; i++)
        {
            ChangePlayerAndExtraSprite(0);
            yield return new WaitForSeconds(0.25f);
            ChangePlayerAndExtraSprite(1);
            yield return new WaitForSeconds(0.25f);
            if (i % 2 == 1) // 1초에 한번씩 돈 증가
            {
                playerScore += Random.Range(startPoint, endPoint + 1) * 1000;
                extraScore += Random.Range(2, 5 + 1) * 1000;
                Debug.Log($"시간: {(float)(i+1)*0.5}초 | playerScore: {playerScore}, extraScore: {extraScore}");
            }
        }
        EventPTJResult();
    }

    void ChangePlayerAndExtraSprite(int index)
    {
        if (index >= 0 && index < 2)
        {
            SetSprite(playerObject[ptjId], playerSprites[ptjId * 2 + index]);
            SetSprite(extraObject[ptjId], extraSprites[ptjId * 2 + index]);
        }
        else
        {
            Debug.LogError("인덱스가 잘못되었습니다.");
        }
    }

    void EventPTJResult()
    {
        // 무승부
        if (playerScore == extraScore)
        {
            dialogContent.text = $"어디 보자…\n{playerName}: {playerScore}개, {extraName}: {extraScore}개\n 동점이므로 둘 다 승진시키겠네."; // ??
            StatusChanger.EarnMoney(30);
            scoreText.text = "알바생에서 계약직으로 승진했다!\n돈 +30만원";
        }
        else if (playerScore > extraScore)
        {
            dialogContent.text = $"어디 보자…\n{playerName}: {playerScore}개, {extraName}: {extraScore}개\n축하하네 {playerName}";
            StatusChanger.EarnMoney(30);
            scoreText.text = "알바생에서 계약직으로 승진했다!\n돈 +30만원";
        }
        else
        {
            dialogContent.text = $"어디 보자…\n{playerName}: {playerScore}개, {extraName}: {extraScore}개\n축하하네 {extraName}";
            scoreText.text = "졌다... 아쉽지만 어쩔 수 없지...\n더 열심히 하자~!";
        }
        currentOrderIndex++;
        dialogObject.SetActive(true);
        nextButton.SetActive(true);
    }
}
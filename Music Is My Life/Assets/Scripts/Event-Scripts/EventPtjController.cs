
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using static EventSpriteManager;

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

    private int ptjId; // 알바 유형 id (0~2)
    private string ptjName;
    private string[] firstDialogTexts = new string[2]; // 첫 다이얼로그의 텍스트 배열을 저장

    private string playerName; // 플레이어 이름
    private string extraName; // 엑스트라 이름
    private string winnerName;
    private int playerScore; // 플레이어 점수
    private int extraScore; // 엑스트라 점수
    private int currentOrderIndex; // 현재 순서 번호
    private EventPtjSelector eventPtjSelector;

    void Start()
    {
        eventPtjSelector = new EventPtjSelector();
        // 버튼 클릭 이벤트에 함수 등록
        nextButton.GetComponent<Button>().onClick.AddListener(OnNextButtonClicked);
        nextButton.SetActive(true);
        scorePanel.SetActive(false);

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
            StartCoroutine(EventPTJCouroutine());
        }
        else if (currentOrderIndex == 3)
        {
            nextButton.SetActive(false);
            ShowEventPtjResult();
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
                break;
            default:
                throw new System.MissingFieldException("ptjType이 등록되지 않았습니다.");
        }
        playerName = PlayerPrefs.GetString("PlayerName");
        SetImage(backgroundObject, backgroundSprites[ptjId]);
        SetImage(dialogObject, dialogSprites[ptjId]);
        SetSprite(playerObject[ptjId], playerSprites[ptjId * 2 + 0]);
        SetSprite(extraObject[ptjId], extraSprites[ptjId * 2 + 0]);
    }

    IEnumerator EventPTJCouroutine()
    {
        int ptjMoney = PlayerPrefs.GetInt($"CumulativeIncome_{ptjName}");

        int startPoint, endPoint;
        if (ptjMoney >= 100)
            (startPoint, endPoint) = (3, 6);
        else if (ptjMoney >= 70)
            (startPoint, endPoint) = (2, 5);
        else if (ptjMoney >= 50)
            (startPoint, endPoint) = (1, 4);
        else
            (startPoint, endPoint) = (1, 3);

        for (int i = 0; i < 8; i++)
        {
            ChangePlayerAndExtraSprite(0);
            yield return new WaitForSeconds(0.25f);
            ChangePlayerAndExtraSprite(1);
            yield return new WaitForSeconds(0.25f);
            if (i % 2 == 1) // 1초에 한번씩 돈 증가
            {
                playerScore += Random.Range(startPoint, endPoint);
                extraScore += Random.Range(2, 5);
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
        }
        else
        {
            winnerName = playerScore > extraScore ? playerName : extraName;
            dialogContent.text = $"어디 보자…\n{playerName}: {playerScore}개, {extraName}: {extraScore}개\n축하하네 {winnerName}";
        }
        currentOrderIndex++;
        dialogObject.SetActive(true);
        nextButton.SetActive(true);
    }

    void ShowEventPtjResult()
    {
        playerObject[ptjId].SetActive(false);
        extraObject[ptjId].SetActive(false);
        if (winnerName.Equals(playerName))
        {
            scoreText.text = "알바생에서 계약직으로 승진했다!\n돈 +30만원";
            StatusChanger.EarnMoney(30);
        }
        else
        {
            scoreText.text = "졌다... 아쉽지만 어쩔 수 없지...\n더 열심히 하자~!";
        }
        scorePanel.SetActive(true);
    }
}
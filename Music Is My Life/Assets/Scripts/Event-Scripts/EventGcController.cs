using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EventGcController : MonoBehaviour
{
    public GameObject[] panelObject;
    public GameObject dialogObject;
    public TMP_Text dialogContent;

    public TMP_Text playerScoreText;
    public TMP_Text rivalScoreText;

    public GameObject nextButton; // 넘기기 위해 클릭할 버튼
    public GameObject scorePanel; // 결과창
    public TMP_Text scoreText;

    private string playerName; // 플레이어 이름
    private string rivalName; // 라이벌 이름
    private string winnerName;
    private int playerScore; // 플레이어 점수
    private int rivalScore; // 라이벌 점수
    private int currentOrderIndex; // 현재 순서 번호

    void Start()
    {
        PlayerPrefs.SetInt("SeasonEvent", 2);

        // 버튼 클릭 이벤트에 함수 등록
        nextButton.GetComponent<Button>().onClick.AddListener(OnNextButtonClicked);
        nextButton.SetActive(true);
        scorePanel.SetActive(false);

        dialogObject.SetActive(true);
        playerScoreText.text = "0";
        rivalScoreText.text = "0";

        playerName = PlayerPrefs.GetString("PlayerName");
        rivalName = "Team Muzik";
        playerScore = 0;
        rivalScore = 0;
        currentOrderIndex = 0;

        for (int i = 0; i < 3; i++)
        {
            panelObject[i].SetActive(i == 0);
        }
    }

    void OnNextButtonClicked()
    {
        if (currentOrderIndex == 0)
        {
            ShowNextPanel(0);
            nextButton.SetActive(false);
            StartCoroutine(EventGcCouroutine());
        }
        else if (currentOrderIndex == 2)
        {
            nextButton.SetActive(false);
            ShowEventGcResult();
        }
    }

    void ShowNextPanel(int index)
    {
        panelObject[index].SetActive(false);
        panelObject[index + 1].SetActive(true);
        currentOrderIndex++;
    }

    IEnumerator EventGcCouroutine()
    {
        int myFame = PlayerPrefs.GetInt($"MyFame");

        int startPoint, endPoint;
        if (myFame >= 60)
            (startPoint, endPoint) = (3000, 6000);
        else if (myFame >= 40)
            (startPoint, endPoint) = (2000, 5000);
        else if (myFame >= 20)
            (startPoint, endPoint) = (1000, 4000);
        else
            (startPoint, endPoint) = (1000, 3000);

        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(0.5f);
            if (i % 2 == 1) // 1초에 한번씩 돈 증가
            {
                playerScore += Random.Range(startPoint, endPoint);
                rivalScore += Random.Range(2000, 5000);
                playerScoreText.text = playerScore.ToString();
                rivalScoreText.text = rivalScore.ToString();
                Debug.Log($"시간: {(float)(i + 1) * 0.5}초 | playerScore: {playerScore}, rivalScore: {rivalScore}");
            }
        }
        EventGcResult();
    }

    void EventGcResult()
    {
        // 무승부
        if (playerScore == rivalScore)
        {
            dialogContent.text = $"공동 우승입니다!";
        }
        else
        {
            winnerName = playerScore > rivalScore ? playerName : rivalName;
            dialogContent.text = $"축하드립니다\n우승은 {winnerName}입니다!";
        }
        ShowNextPanel(1);
        currentOrderIndex++;
        nextButton.SetActive(true);
    }

    void ShowEventGcResult()
    {
        if (winnerName.Equals(playerName))
        {
            scoreText.text = "기타 콘테스트에서 우승했다!\n나의 명성 +15";
            StatusChanger.UpdateMyFame(+15);
        }
        else
        {
            scoreText.text = "졌다... 아쉽지만 어쩔 수 없지...\n더 열심히 하자~!";
        }
        scorePanel.SetActive(true);
    }
}
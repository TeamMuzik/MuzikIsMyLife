using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EventGcController : MonoBehaviour
{
    public GameObject[] panelObject;
    public GameObject playerWinPrize;
    public GameObject rivalWinPrize;

    public TMP_Text playerScoreText;
    public TMP_Text rivalScoreText;

    public GameObject nextButton; // 넘기기 위해 클릭할 버튼
    public GameObject scorePanel; // 결과창
    public TMP_Text scoreText;

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

        playerWinPrize.SetActive(false);
        rivalWinPrize.SetActive(false);

        playerScoreText.text = "0";
        rivalScoreText.text = "0";
        
        playerScore = 0;
        rivalScore = 0;
        currentOrderIndex = 0;

        for (int i = 0; i < 2; i++)
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
            scorePanel.SetActive(true);
            rivalWinPrize.SetActive(false);
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
        //기존 60, 40, 20 | 변경 후 40 20 10
        if (myFame >= 40)
            (startPoint, endPoint) = (3, 6);
        else if (myFame >= 20)
            (startPoint, endPoint) = (2, 5);
        else if (myFame >= 10)
            (startPoint, endPoint) = (1, 4);
        else
            (startPoint, endPoint) = (1, 3);

        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(0.5f);
            playerScore += Random.Range(startPoint, endPoint + 1) * 1000;
            rivalScore += Random.Range(2, 5 + 1) * 1000;

            playerScoreText.text = playerScore.ToString();
            rivalScoreText.text = rivalScore.ToString();
            Debug.Log($"시간: {(i + 1) * 0.5f}초 | playerScore: {playerScore}, rivalScore: {rivalScore}");
        }
        yield return new WaitForSeconds(1f);
        EventGcResult();
    }

    void EventGcResult()
    {
        // 무승부
        if (playerScore == rivalScore)
        {
            playerWinPrize.SetActive(true);
            rivalWinPrize.SetActive(true);
            scoreText.text = "기타 콘테스트에서 공동 우승했다!\n나의 명성 +15";
            StatusChanger.UpdateMyFame(+15);
        }
        else if (playerScore > rivalScore)
        {
            playerWinPrize.SetActive(true);
            scoreText.text = "기타 콘테스트에서 우승했다!\n나의 명성 +15";
            StatusChanger.UpdateMyFame(+15);
        }
        else
        {
            rivalWinPrize.SetActive(true);
            scoreText.text = "졌다... 아쉽지만 어쩔 수 없지...\n더 열심히 하자~!";
        }
        currentOrderIndex++;
        nextButton.SetActive(true);
    }
}
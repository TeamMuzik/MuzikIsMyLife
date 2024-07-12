using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EventAvController : MonoBehaviour
{
    public GameObject[] panelObject;
    public GameObject bandWinPrize;
    public GameObject rivalWinPrize;

    public TMP_Text bandScoreText;
    public TMP_Text rivalScoreText;

    public GameObject nextButton; // 넘기기 위해 클릭할 버튼
    public GameObject scorePanel; // 결과창
    public TMP_Text scoreText;

    private int bandScore; // 밴드 점수
    private int rivalScore; // 라이벌 점수
    private int currentOrderIndex; // 현재 순서 번호

    void Start()
    {
        PlayerPrefs.SetInt("SeasonEvent", 3);

        // 버튼 클릭 이벤트에 함수 등록
        nextButton.GetComponent<Button>().onClick.AddListener(OnNextButtonClicked);
        nextButton.SetActive(true);
        scorePanel.SetActive(false);

        bandWinPrize.SetActive(false);
        rivalWinPrize.SetActive(false);

        bandScoreText.text = "0";
        rivalScoreText.text = "0";
        
        bandScore = 0;
        rivalScore = 0;
        currentOrderIndex = 0;

        panelObject[0].SetActive(true);
    }

    void OnNextButtonClicked()
    {
        if (currentOrderIndex == 0)
        {
            currentOrderIndex++;
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


    IEnumerator EventGcCouroutine()
    {
        int bandFame = PlayerPrefs.GetInt($"BandFame");

        int startPoint, endPoint;
        if (bandFame >= 100)
            (startPoint, endPoint) = (3, 6);
        else if (bandFame >= 70)
            (startPoint, endPoint) = (2, 5);
        else if (bandFame >= 50)
            (startPoint, endPoint) = (1, 4);
        else
            (startPoint, endPoint) = (1, 3);

        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(1f);
            bandScore += Random.Range(startPoint, endPoint + 1) * 1000;
            rivalScore += Random.Range(2, 5 + 1) * 1000;

            bandScoreText.text = bandScore.ToString();
            rivalScoreText.text = rivalScore.ToString();
            Debug.Log($"시간: {(float)(i + 1) * 1}초 | bandScore: {bandScore}, rivalScore: {rivalScore}");
        }
        yield return new WaitForSeconds(1f);
        EventGcResult();
    }

    void EventGcResult()
    {
        // 무승부
        if (bandScore == rivalScore)
        {
            bandWinPrize.SetActive(true);
            rivalWinPrize.SetActive(true);
            scoreText.text = "야옹이 한국에서 최고의 해외 아티스트 중 하나로 뽑혔다!\n야옹의 명성 +30";
            StatusChanger.UpdateBandFame(+30);
        }
        else if (bandScore > rivalScore)
        {
            bandWinPrize.SetActive(true);
            scoreText.text = "야옹이 한국에서 최고의 해외 아티스트로 뽑혔다!\n야옹의 명성 +30";
            StatusChanger.UpdateBandFame(+30);
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
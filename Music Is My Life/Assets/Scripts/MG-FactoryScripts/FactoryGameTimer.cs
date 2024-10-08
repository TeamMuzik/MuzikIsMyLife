using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactoryGameTimer : MonoBehaviour
{
    private FactoryGame FactoryGameInstance;
    private CafeGameButton cafeGameButtonInstance;
    public float TotalTime;
    // [SerializeField] private float StageTime;
    [SerializeField] private TMP_Text TotalTimerTxt;
    [SerializeField] private TMP_Text MistakeTimerTxt;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text stressText;
    [SerializeField] private TextMeshProUGUI ContentInScorePanel;
    private static int highScore = 0; // 최고 점수
    // [SerializeField] private TMP_Text StageTimerTxt;
    public static float totalTime;
    // public static float stageTime;

    int minute = 0;
    int second = 0;

    [SerializeField]
    private GameObject EndPanel;
    public GameObject StartPanel;
    public GameObject TutorialPanel;
    [SerializeField]
    public GameObject MistakePanel;
    public GameObject howToPlay;
    public GameObject ready;
    public GameObject start;
    private static int playCount = 0; // 플레이 횟수
    private int fortuneId;
    private string isFortune;

    private void Start()
    {
        FactoryGameInstance = FindObjectOfType<FactoryGame>();
        cafeGameButtonInstance = GetComponent<CafeGameButton>();
        TotalTime = 31;
        StartPanel.SetActive(false);
        EndPanel.SetActive(false);
        howToPlay.SetActive(false);
        TutorialPanel.SetActive(false);

        highScore = PlayerPrefs.GetInt("FactoryGameHighScore", 0);
        // StageTime = 16;

        playCount = PlayerPrefs.GetInt("FactoryGamePlayCount");
        playCount++; // 플레이 횟수 증가
        PlayerPrefs.SetInt("FactoryGamePlayCount", playCount);
        PlayerPrefs.Save();
        Debug.Log("Current playCount: " + playCount);

        isFortune = "";
        fortuneId = DayFortune.GetTodayFortuneId();

        // 처음 실행할 때는 게임 방법이 나오게
        if (playCount == 1)
        {
            howToPlay.SetActive(true);
        }
        else
        {
            Tutorial();
        }

    }

    public void Tutorial()
    {
        TutorialPanel.SetActive(true);
        StartCoroutine(ReadyStart());
    }

    public IEnumerator ReadyStart()
    {
        ready.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        start.SetActive(true);
        yield return new WaitForSeconds(1f);

        cafeGameButtonInstance.FactoryGameStartButton();
        TutorialPanel.SetActive(false);
    }

    public IEnumerator TotalTimer()
    {
        totalTime = TotalTime;
        while (totalTime > 0)
        {
            totalTime -= Time.deltaTime;
            minute = (int)totalTime / 60;
            second = (int)totalTime % 60;
            TotalTimerTxt.text = minute.ToString("00") + ":" + second.ToString("00");
            yield return null;

            if (totalTime <= 0)
            {
                StopAllCoroutines();
                int dollCount = FactoryGameInstance.money;

                if (fortuneId == 1 || fortuneId == 7)//오늘의 운세 1번(알바비 +5) 또는 오늘의 운세 7번(알바 하드모드)
                    isFortune = "(운세적용)";

                if (fortuneId == 1) //오늘의 운세 1번 (알바비 +5)
                    FactoryGameInstance.money += 5; ;


                // 알바 결과 매핑
                (string resultRes, string stressRes) = MGResultManager.PartTimeDayResult(2);
                resultText.text = resultRes;
                ContentInScorePanel.SetText(dollCount + "개의 인형을 만들었다.\n 번 돈 " + FactoryGameInstance.money.ToString() + "만원");

                if (FactoryGameInstance.money > highScore)
                {
                    highScore = FactoryGameInstance.money;
                    PlayerPrefs.SetInt("FactoryGameHighScore", highScore);
                    PlayerPrefs.Save();
                }
                Debug.Log("Current High Score: " + highScore);

                stressText.text = stressRes + "\n" + isFortune;

                EndPanel.SetActive(true);
                StartPanel.SetActive(false);
                StatusChanger.EarnMoney(FactoryGameInstance.money);
            }
        }
    }

    public IEnumerator BlinkText(float Time)
    {
        MistakePanel.SetActive(true);
        int minute = (int)Time / 60;
        int second = (int)Time % 60;
        MistakeTimerTxt.text = minute.ToString("00") + ":" + second.ToString("00");
        yield return new WaitForSeconds(.25f);
        MistakeTimerTxt.text = "";
        yield return new WaitForSeconds(.25f);
        FactoryGameKeyboard.allowControl = true;
        MistakeTimerTxt.text = minute.ToString("00") + ":" + second.ToString("00");
        yield return new WaitForSeconds(.25f);
        MistakeTimerTxt.text = "";
        yield return new WaitForSeconds(.25f);
        MistakeTimerTxt.text = minute.ToString("00") + ":" + second.ToString("00");
        MistakePanel.SetActive(false);
    }


    // public IEnumerator StageTimer()
    // {
    //     stageTime = StageTime;
    //     while(stageTime > 0)
    //     {
    //         stageTime -= Time.deltaTime;
    //         int second = (int)stageTime % 60;
    //         StageTimerTxt.text = second.ToString() + "초";
    //         yield return null;

    //         if(stageTime <= 0)
    //         {
    //             FactoryGameKeyboard.keyState = 0;
    //             foreach (GameObject keyboard in FactoryGameInstance.spawnedKeyboards)
    //             {
    //                 Destroy(keyboard);
    //             }
    //             FactoryGameInstance.increaseStageNum();
    //             FactoryGameInstance.SpawnKeyBoards();
    //             stageTime = StageTime;
    //         }
    //     }
    // }
}

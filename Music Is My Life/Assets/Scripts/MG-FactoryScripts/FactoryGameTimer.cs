using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactoryGameTimer : MonoBehaviour
{
    private FactoryGame FactoryGameInstance;
    public float TotalTime;
    // [SerializeField] private float StageTime;
    [SerializeField] private TMP_Text TotalTimerTxt;
    [SerializeField] private TMP_Text MistakeTimerTxt;
    [SerializeField] private TMP_Text moneyNumInGameText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text stressText;
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

    private void Start()
    {
        FactoryGameInstance = FindObjectOfType<FactoryGame>();
        TotalTime = 61;
        // StageTime = 16;
    }

    public IEnumerator TotalTimer()
    {
        totalTime = TotalTime;
        while(totalTime > 0)
        {
            totalTime -= Time.deltaTime;
            minute = (int)totalTime / 60;
            second = (int)totalTime % 60;
            TotalTimerTxt.text = minute.ToString("00") + ":" + second.ToString("00");
            yield return null;

            if(totalTime <= 0)
            {
                StopAllCoroutines();
                int result = MGResultManager.PartTimeDayResult();
                switch (result)
                {
                    case 1:
                        resultText.text = "3일 연속으로 알바를 완벽하게 성공했다!";
                        stressText.text = "스트레스 -20";
                        break;
                    case 2:
                        resultText.text = "바쁜 하루였다...";
                        stressText.text = "스트레스 +20";
                        break;
                    default:
                        resultText.text = "오늘도 열심히 알바를 했다.";
                        stressText.text = "스트레스 +10";
                        break;
                }
                moneyNumInGameText.SetText(FactoryGameInstance.money.ToString()+" 만원");
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

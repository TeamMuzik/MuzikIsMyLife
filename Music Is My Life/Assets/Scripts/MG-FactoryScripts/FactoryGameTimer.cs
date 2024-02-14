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
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text stressText;
    [SerializeField] private TextMeshProUGUI ContentInScorePanel;
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
        TutorialPanel.SetActive(true);
        StartPanel.SetActive(false);
        EndPanel.SetActive(false);
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

            if (totalTime <= 0)
            {
                StopAllCoroutines();

                // 알바 결과 매핑
                (string resultRes, string stressRes) = MGResultManager.PartTimeDayResult(2);
                resultText.text = resultRes;
                stressText.text = stressRes;
                ContentInScorePanel.SetText(FactoryGameInstance.money.ToString()+"개의 인형을 만들었다.\n 번 돈 "+FactoryGameInstance.money.ToString()+"만원");

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

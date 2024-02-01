using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartTimeGameTimer : MonoBehaviour
{
    private PartTimeGame PartTimeGameInstance;
    [SerializeField] private float TotalTime;
    [SerializeField] private float StageTime;
    [SerializeField] private TMP_Text TotalTimerTxt;
    [SerializeField] private TMP_Text StageTimerTxt;
    public static float totalTime;
    public static float stageTime;

    [SerializeField]
    private GameObject EndPanel;
    public GameObject StartPanel;
    public GameObject TutorialPanel;

    private void Start()
    {
        PartTimeGameInstance = FindObjectOfType<PartTimeGame>();
        TotalTime = 61;
        StageTime = 16;
        TutorialPanel.SetActive(true);
    }

    public IEnumerator StartTotalTimer()
    {
        totalTime = TotalTime;
        while(totalTime > 0)
        {
            totalTime -= Time.deltaTime;
            int minute = (int)totalTime / 60;
            int second = (int)totalTime % 60;
            TotalTimerTxt.text = minute.ToString("00") + ":" + second.ToString("00");
            yield return null;

            if(totalTime <= 0)
            {
                StopAllCoroutines();
                EndPanel.SetActive(true);
                StartPanel.SetActive(false);
                StatusChanger.EarnMoney(PartTimeGameInstance.money);
            }
        }
    }

    public IEnumerator StartStageTimer()
    {
        stageTime = StageTime;
        while(stageTime > 0)
        {
            stageTime -= Time.deltaTime;
            int second = (int)stageTime % 60;
            StageTimerTxt.text = second.ToString() + "초";
            yield return null;

            if(stageTime <= 0)
            {  
                PartTimeGameKeyBoard.keyState = 0;
                foreach (GameObject keyboard in PartTimeGame.spawnedKeyboards)
                {
                    Destroy(keyboard);
                }
                PartTimeGameInstance.increaseStageNum();
                PartTimeGameInstance.SpawnKeyBoards();
                stageTime = StageTime;
            }
        }
    }
}

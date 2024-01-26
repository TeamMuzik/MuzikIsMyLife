using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartTimeGameTimer : MonoBehaviour
{
    private PartTimeGame PartTimeGameInstance;
    [SerializeField] private float Totaltime;
    [SerializeField] private TMP_Text TotalTimerTxt;
    public static float totalTime;

    [SerializeField]
    private GameObject EndPanel;
    public GameObject StartPanel;
    public GameObject TutorialPanel;

    private void Start()
    {
        PartTimeGameInstance = FindObjectOfType<PartTimeGame>();
        Totaltime = 61;
        TutorialPanel.SetActive(true);
    }

    public IEnumerator StartTotalTimer()
    {
        totalTime = Totaltime;
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
}

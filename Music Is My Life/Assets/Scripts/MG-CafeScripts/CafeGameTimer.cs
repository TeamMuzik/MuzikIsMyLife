using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CafeGameTimer : MonoBehaviour
{
    private CafeGame cafeGameInstance;

    [SerializeField] private TMP_Text TotalTimerTxt;
    
    [SerializeField] private TMP_Text Order1TimerTxt;
    [SerializeField] private TMP_Text Order2TimerTxt;
    [SerializeField] private TMP_Text Order3TimerTxt;
    [SerializeField] private TMP_Text Order4TimerTxt;
    [SerializeField] private TMP_Text Order5TimerTxt;

    [SerializeField] private float Totaltime;
    [SerializeField] private float OrderTime;
    
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text stressText;
    [SerializeField]
    private TextMeshProUGUI ContentInScorePanel;

    public static float totalTime;

    public static float order1Time;
    public static float order2Time;
    public static float order3Time;
    public static float order4Time;
    public static float order5Time;

    [SerializeField]
    private GameObject EndPanel;
    public GameObject StartPanel;
    public GameObject TutorialPanel;

    private int fortuneId;
    private string isFortune;

    private void Start()
    {
        cafeGameInstance = FindObjectOfType<CafeGame>();
        Totaltime = 61;
        OrderTime = 16;
        TutorialPanel.SetActive(true);
        StartPanel.SetActive(false);
        EndPanel.SetActive(false);

        isFortune = "";
        fortuneId = DayFortune.GetTodayFortuneId();
    }

    public void startCoroutine()
    {
        StartCoroutine(StartTotalTimer());
        StartCoroutine(StartOrder1Timer());
        StartCoroutine(StartOrder2Timer());
        StartCoroutine(StartOrder3Timer());
        StartCoroutine(StartOrder4Timer());
        StartCoroutine(StartOrder5Timer());
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

                if (fortuneId == 1 || fortuneId == 7)//오늘의 운세 1번(알바비 +5) 또는 오늘의 운세 7번(알바 하드모드)
                    isFortune = "(운세적용)";
                else if (fortuneId == 1) //오늘의 운세 1번 (알바비 +5)
                    cafeGameInstance.money += 5;
                
                // 알바 결과 매핑
                (string resultRes, string stressRes) = MGResultManager.PartTimeDayResult(0);

                resultText.text = resultRes;
                ContentInScorePanel.SetText(cafeGameInstance.money.ToString()+"개의 음료를 만들었다.\n 번 돈 "+cafeGameInstance.money.ToString()+"만원");
                stressText.text = stressRes + "\n" + isFortune;
                


                EndPanel.SetActive(true);
                StartPanel.SetActive(false);
                StatusChanger.EarnMoney(cafeGameInstance.money);

                cafeGameInstance.DestroyOrder(cafeGameInstance.Order1, cafeGameInstance.Order1Name);
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order2, cafeGameInstance.Order1Name);
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order3, cafeGameInstance.Order1Name);
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order4, cafeGameInstance.Order1Name);
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order5, cafeGameInstance.Order1Name);
            }
        }
    }

    public IEnumerator StartOrder1Timer()
    {
        order1Time = OrderTime;
        while(order1Time > 0)
        {
            order1Time -= Time.deltaTime;
            int second = (int)order1Time % 60;
            Order1TimerTxt.text = second.ToString();
            yield return null;

            if(order1Time <= 0)
            {
                Order1TimerTxt.text = "";
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order1, cafeGameInstance.Order1Name);
                cafeGameInstance.ReceiptManager();
                int randNum = Random.Range(2, 5);
                yield return new WaitForSeconds(randNum);
                cafeGameInstance.SpawnFruits_1();
                order1Time = OrderTime;
            }
        }
    }

    public IEnumerator StartOrder2Timer()
    {
        order2Time = OrderTime;
        while(order2Time > 0)
        {
            order2Time -= Time.deltaTime;
            int second = (int)order2Time % 60;
            Order2TimerTxt.text = second.ToString();
            yield return null;

            if(order2Time <= 0)
            {
                Order2TimerTxt.text = "";
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order2, cafeGameInstance.Order2Name);
                cafeGameInstance.ReceiptManager();
                int randNum = Random.Range(2, 5);
                yield return new WaitForSeconds(randNum);
                cafeGameInstance.SpawnFruits_2();
                order2Time = OrderTime;
            }
        }
    }

    public IEnumerator StartOrder3Timer()
    {
        order3Time = OrderTime;
        while(order3Time > 0)
        {
            order3Time -= Time.deltaTime;
            int second = (int)order3Time % 60;
            Order3TimerTxt.text = second.ToString();
            yield return null;

            if(order3Time <= 0)
            {
                Order3TimerTxt.text = "";
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order3, cafeGameInstance.Order3Name);
                cafeGameInstance.ReceiptManager();
                int randNum = Random.Range(2, 5);
                yield return new WaitForSeconds(randNum);
                cafeGameInstance.SpawnFruits_3();
                order3Time = OrderTime;
            }
        }
    }

    public IEnumerator StartOrder4Timer()
    {
        order4Time = OrderTime;
        while(order4Time > 0)
        {
            order4Time -= Time.deltaTime;
            int second = (int)order4Time % 60;
            Order4TimerTxt.text = second.ToString();
            yield return null;

            if(order4Time <= 0)
            {
                Order4TimerTxt.text = "";
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order4, cafeGameInstance.Order4Name);
                cafeGameInstance.ReceiptManager();
                int randNum = Random.Range(2, 5);
                yield return new WaitForSeconds(randNum);
                cafeGameInstance.SpawnFruits_4();
                order4Time = OrderTime;
            }
        }
    }

    public IEnumerator StartOrder5Timer()
    {
        order5Time = OrderTime;
        while(order5Time > 0)
        {
            order5Time -= Time.deltaTime;
            int second = (int)order5Time % 60;
            Order5TimerTxt.text = second.ToString();
            yield return null;

            if(order5Time <= 0)
            {
                Order5TimerTxt.text = "";
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order5, cafeGameInstance.Order5Name);
                cafeGameInstance.ReceiptManager();
                int randNum = Random.Range(2, 5);
                yield return new WaitForSeconds(randNum);
                cafeGameInstance.SpawnFruits_5();
                order5Time = OrderTime;
            }
        }
    }
}

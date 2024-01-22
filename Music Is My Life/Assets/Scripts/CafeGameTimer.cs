using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CafeGameTimer : MonoBehaviour
{
    private CafeGame cafeGameInstance;
    
    [SerializeField]
    private TextMeshProUGUI moneyNumTextInEnd;

    [SerializeField] private TMP_Text TotalTimerTxt;
    
    [SerializeField] private TMP_Text Order1TimerTxt;
    [SerializeField] private TMP_Text Order2TimerTxt;
    [SerializeField] private TMP_Text Order3TimerTxt;
    [SerializeField] private TMP_Text Order4TimerTxt;
    [SerializeField] private TMP_Text Order5TimerTxt;

    [SerializeField] private float Totaltime;
    [SerializeField] private float OrderTime;

    public static float totalTime;

    public static float order1Time;
    public static float order2Time;
    public static float order3Time;
    public static float order4Time;
    public static float order5Time;

    [SerializeField]
    private GameObject EndPanel;

    private void Start()
    {
        cafeGameInstance = FindObjectOfType<CafeGame>();
        Totaltime = 121;
        OrderTime = 16;
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
                EndPanel.SetActive(true);
                moneyNumTextInEnd.SetText(cafeGameInstance.money.ToString());
                for (int i = 0; i < cafeGameInstance.FruitsButton.Length; i++)
                {
                    cafeGameInstance.FruitsButton[i].SetActive(false);
                }

                StopAllCoroutines();
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
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order1);
                cafeGameInstance.BoxManager();
                yield return new WaitForSeconds(2f);
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
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order2);
                cafeGameInstance.BoxManager();
                yield return new WaitForSeconds(2f);
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
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order3);
                cafeGameInstance.BoxManager();
                yield return new WaitForSeconds(2f);
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
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order4);
                cafeGameInstance.BoxManager();
                yield return new WaitForSeconds(2f);
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
                cafeGameInstance.DestroyOrder(cafeGameInstance.Order5);
                cafeGameInstance.BoxManager();
                yield return new WaitForSeconds(2f);
                cafeGameInstance.SpawnFruits_5();
                order5Time = OrderTime;
            }
        }
    }
}

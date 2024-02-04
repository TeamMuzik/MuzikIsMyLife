using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CafeGameButton : MonoBehaviour
{
    private CafeGame cafeGameInstance;
    private CafeGameTimer cafeGameTimerInstance;
    private FactoryGame FactoryGameInstance;
    private FactoryGameTimer FactoryGameTimerInstance;

    void Start()
    {
        cafeGameInstance = FindObjectOfType<CafeGame>();
        cafeGameTimerInstance = FindObjectOfType<CafeGameTimer>();
        FactoryGameInstance = FindObjectOfType<FactoryGame>();
        FactoryGameTimerInstance = FindObjectOfType<FactoryGameTimer>();
    }

    public void Checker() //블랜더 클릭시 실행되는 함수
    {
        if (cafeGameInstance.TotalClickedFruits.Count != 0) //블랜더가 비어있으면 함수를 불러오지 않음 (돈버그 방지)
        {
            CheckOrder(cafeGameInstance.Order1, ref CafeGameTimer.order1Time);
            CheckOrder(cafeGameInstance.Order2, ref CafeGameTimer.order2Time);
            CheckOrder(cafeGameInstance.Order3, ref CafeGameTimer.order3Time);
            CheckOrder(cafeGameInstance.Order4, ref CafeGameTimer.order4Time);
            CheckOrder(cafeGameInstance.Order5, ref CafeGameTimer.order5Time);
        }
    }

    private void CheckOrder(List<GameObject> order, ref float orderTime) //모든 오더들과 클릭한 과일을 비교함
    {
        //HashSet을 이용하여 순서에 상관없도록 함 
        HashSet<string> orderNames = new HashSet<string>(order.ConvertAll(obj => obj.name)); //주문에 있는 과일 오브젝트의 이름을 HashSet에 저장
        HashSet<string> clickedObjectNames = new HashSet<string>(cafeGameInstance.TotalClickedFruits.Select(obj => obj.name + "(Clone)")); //클릭한 과일 오브젝트의 이름 뒤에 클론을 붙여 HashSet에 저장

        if (order.Count == cafeGameInstance.TotalClickedFruits.Count) //과일의 개수와 상관없이 종류만 똑같으면 check되던 버그 방지
        {
            if (orderNames.SetEquals(clickedObjectNames))
            {
                cafeGameInstance.DestroyOrder(order);
                cafeGameInstance.moneyManager();
                cafeGameInstance.BoxManager();
                Garbage();
                orderTime = 0f;
            }
        }
    }

    public void Garbage()
    {
        cafeGameInstance.TotalClickedFruits.Clear();

        for (int i = 0; i < cafeGameInstance.TotalFruitImageObject.Count; i++)
        {
            Destroy(cafeGameInstance.TotalFruitImageObject[i]);
        }
        cafeGameInstance.TotalFruitImageObject.Clear();
    }

    public void CafeGameGameStartButton()
    {
        cafeGameTimerInstance.TutorialPanel.SetActive(false);
        cafeGameTimerInstance.StartPanel.SetActive(true);
        cafeGameInstance.SpawnFruits_1();
        cafeGameInstance.SpawnFruits_2();
        cafeGameInstance.SpawnFruits_3();
        cafeGameInstance.SpawnFruits_4();
        cafeGameInstance.SpawnFruits_5();
        cafeGameTimerInstance.startCoroutine();
    }

    public void PartTimeGameStartButton()
    {
        FactoryGameTimerInstance.TutorialPanel.SetActive(false);
        FactoryGameTimerInstance.StartPanel.SetActive(true);
        FactoryGameTimerInstance.StartCoroutine(FactoryGameTimerInstance.TotalTimer());
        // FactoryGameTimerInstance.StartCoroutine(FactoryGameTimerInstance.StageTimer());
        FactoryGameInstance.SpawnKeyBoards();
    }
}
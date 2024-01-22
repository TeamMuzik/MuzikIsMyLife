using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeGameButton : MonoBehaviour
{
    private CafeGame cafeGameInstance;

    void Start()
    {
        cafeGameInstance = FindObjectOfType<CafeGame>();
    }

    public void Checker()
    {
        HashSet<string> Order1Names = new HashSet<string>();
        HashSet<string> Order2Names = new HashSet<string>();
        HashSet<string> Order3Names = new HashSet<string>();
        HashSet<string> Order4Names = new HashSet<string>();
        HashSet<string> Order5Names = new HashSet<string>(); //주문된 과일들의 이름 리스트

        HashSet<string> clickedObjectNames = new HashSet<string>();//클릭된 과일들의 이름 리스트

        foreach (var orderObject in cafeGameInstance.Order1)
        {
            Order1Names.Add(orderObject.name);
        }

        foreach (var orderObject in cafeGameInstance.Order2)
        {
            Order2Names.Add(orderObject.name);
        }

        foreach (var orderObject in cafeGameInstance.Order3)
        {
            Order3Names.Add(orderObject.name);
        }

        foreach (var orderObject in cafeGameInstance.Order4)
        {
            Order4Names.Add(orderObject.name);
        }

        foreach (var orderObject in cafeGameInstance.Order5)
        {
            Order5Names.Add(orderObject.name);
        }

        foreach (var clickedObject in cafeGameInstance.TotalClickedFruits)
        {
            clickedObjectNames.Add(clickedObject.name+"(Clone)");
        }

        if (Order1Names.SetEquals(clickedObjectNames))
        {
            Debug.Log("맞음");
            cafeGameInstance.Destroy(cafeGameInstance.Order1);
            cafeGameInstance.BoxManager();
            cafeGameInstance.SpawnFruits_1();
            cafeGameInstance.moneyManager();
            return;
        }

        if (Order2Names.SetEquals(clickedObjectNames))
        {
            Debug.Log("맞음");
            cafeGameInstance.Destroy(cafeGameInstance.Order2);
            cafeGameInstance.BoxManager();
            cafeGameInstance.SpawnFruits_2();
            cafeGameInstance.moneyManager();
            return;
        }

        if (Order3Names.SetEquals(clickedObjectNames))
        {
            Debug.Log("맞음");
            cafeGameInstance.Destroy(cafeGameInstance.Order3);
            cafeGameInstance.BoxManager();
            cafeGameInstance.SpawnFruits_3();
            cafeGameInstance.moneyManager();
            return;
        }

        if (Order4Names.SetEquals(clickedObjectNames))
        {
            Debug.Log("맞음");
            cafeGameInstance.Destroy(cafeGameInstance.Order4);
            cafeGameInstance.BoxManager();
            cafeGameInstance.SpawnFruits_4();
            cafeGameInstance.moneyManager();
            return;
        }

        if (Order5Names.SetEquals(clickedObjectNames))
        {
            Debug.Log("맞음");
            cafeGameInstance.Destroy(cafeGameInstance.Order5);
            cafeGameInstance.BoxManager();
            cafeGameInstance.SpawnFruits_5();
            cafeGameInstance.moneyManager();
            return;
        }
    }

    public void Garbage()
    {
        cafeGameInstance.TotalClickedFruits.Clear();
    }
}
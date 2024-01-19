using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blender : MonoBehaviour
{
    private CafeGame cafeGameInstance;


    void Start()
    {
        cafeGameInstance = FindObjectOfType<CafeGame>();
    }

    public void Checker()
    {
        if (cafeGameInstance.TotalOrder.Count != cafeGameInstance.TotalClickedFruits.Count)
        {
            Debug.Log("틀림1");
            cafeGameInstance.DestroyOrderAndClicked();
            cafeGameInstance.SpawnFruits();
            return;
        }

        HashSet<string> orderObjectNames = new HashSet<string>(); //주문된 과일들의 이름 리스트
        HashSet<string> clickedObjectNames = new HashSet<string>();//클릭된 과일들의 이름 리스트

        foreach (var orderObject in cafeGameInstance.TotalOrder)
        {
            orderObjectNames.Add(orderObject.name);
        }

        foreach (var clickedObject in cafeGameInstance.TotalClickedFruits)
        {
            clickedObjectNames.Add(clickedObject.name+"(Clone)");
        }

        if (orderObjectNames.SetEquals(clickedObjectNames))
        {
            Debug.Log("맞음");
            cafeGameInstance.DestroyOrderAndClicked();
            cafeGameInstance.SpawnFruits();
        }
        else
        {
            Debug.Log("틀림2");
            cafeGameInstance.DestroyOrderAndClicked();
            cafeGameInstance.SpawnFruits();
        }
    }
}
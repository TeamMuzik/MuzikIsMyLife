using System.Collections.Generic;
using UnityEngine;


public class FunitureController : MonoBehaviour
{
    public GameObject[] allFurnitureObj;
    private List<GameObject> ownedFurnitureObj = new List<GameObject>();

    void Start()
    {
        LoadAllFurnitureData();
    }

    public void LoadAllFurnitureData()
    {
        foreach (GameObject furnitureObj in allFurnitureObj)
        {
            Furniture furniture = furnitureObj.GetComponent<Furniture>();

            furniture.LoadFurnitureStatus();
            Debug.Log("가구명: " + furniture.furnitureName + " | IsOwned: " + furniture.IsOwned + " | isEquipped: " + furniture.IsEquipped);

            if (furniture.IsOwned)
            {
                ownedFurnitureObj.Add(furnitureObj); // 나중에 보유 중인 가구 확인 위해
            }
            if (furniture.IsEquipped)
            {
                furnitureObj.SetActive(true);
            }
            else
            {
                furnitureObj.SetActive(false);
            }
        }
    }

}

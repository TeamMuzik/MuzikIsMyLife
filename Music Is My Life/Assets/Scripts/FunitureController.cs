using System.Collections.Generic;
using UnityEngine;


public class FunitureController : MonoBehaviour
{
    public GameObject[] allFurnitureObj;
    private List<GameObject> ownedFurnitureObj = new List<GameObject>();

    public void DefaultFunitureSetting()
    {
        // 정한 가구명으로...
        string[] defaultFurnitureStrings = { "회색 방", "정리되지 않은 침대", "갈색 수납장", "갈색 책상", "캐릭터-앉기", "보라 쿠션 의자" };
        foreach(string furnitureName in defaultFurnitureStrings)
        {
            PlayerPrefs.SetInt($"{furnitureName}_IsOwned", 1);
            PlayerPrefs.SetInt($"{furnitureName}_IsEquipped", 1);
        }
    }

    void Start()
    {
        DefaultFunitureSetting();
        LoadAllFurnitureData();
    }

    public void LoadAllFurnitureData()
    {
        foreach (GameObject furnitureObj in allFurnitureObj)
        {
            Furniture furniture = furnitureObj.GetComponent<Furniture>();

            furniture.LoadFurnitureData();
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

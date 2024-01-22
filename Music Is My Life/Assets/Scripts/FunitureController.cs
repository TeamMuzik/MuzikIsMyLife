using System.Collections.Generic;
using UnityEngine;


public class FunitureController : MonoBehaviour
{
    public GameObject[] allFurnitureObj;
    private List<GameObject> ownedFurnitureObj = new List<GameObject>();

    public void DefaultFunitureSetting()
    {
        // 정한 가구명으로...
        string[] defaultFurnitureCI = { "방_0", "침대_앉은", "수납장_0", "기타_0", "책상_0", "캐릭터_앉은", "음향장비_0", "노트북_0", "의자_0"};
        foreach(string Category_Index in defaultFurnitureCI)
        {
            PlayerPrefs.SetInt($"{Category_Index}_IsOwned", 1);
            PlayerPrefs.SetInt($"{Category_Index}_IsEquipped", 1);
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

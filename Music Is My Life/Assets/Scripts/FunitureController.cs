using System.Collections.Generic;
using UnityEngine;


public class FunitureController : MonoBehaviour
{
    public GameObject[] allFurnitureObj;
    private List<GameObject> ownedFurnitureObj = new List<GameObject>();

    public void DefaultFunitureSetting()
    {
        // 정한 가구명으로...
        string[] defaultFurnitureCI = { "ROOM_0", "BED_0", "GUITAR_0", "SHELF_0", "DESK_0", "CHARACTER_0", "SOUNDEQUIP_0", "COMPUTER_0", "CHAIR_0"};
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

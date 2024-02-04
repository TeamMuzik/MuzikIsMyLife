using UnityEngine;

public class Furniture : MonoBehaviour
{
    public string furnitureName; // 가구 이름
    public string Category_Index; // 가구 카테고리와 번호
    public string description; // 가구 설명
    private bool isOwned; // 가구 보유 여부
    private bool isEquipped; // 가구 장착 여부

    public bool IsOwned
    {
        get { return isOwned; }
        set
        {
            isOwned = value;
            SaveFurnitureStatus();
        }
    }
    public bool IsEquipped
    {
        get { return isEquipped; }
        set
        {
            isEquipped = value;
            SaveFurnitureStatus();
        }
    }

    public void SaveFurnitureStatus() // 가구 정보 저장
    {
        PlayerPrefs.SetInt($"{Category_Index}_IsOwned", isOwned ? 1 : 0);
        PlayerPrefs.SetInt($"{Category_Index}_IsEquipped", isEquipped ? 1 : 0);
    }

    public void LoadFurnitureStatus() // 가구 정보 로드
    {
        isOwned = PlayerPrefs.GetInt($"{Category_Index}_IsOwned", 0) == 1;
        isEquipped = PlayerPrefs.GetInt($"{Category_Index}_IsEquipped", 0) == 1;
    }
}
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public string furnitureName; // 가구 이름
    public string description; // 가구 설명
    //private bool isOwned; // 가구 보유 여부
    private bool isEquipped; // 가구 장착 여부

    /*public bool IsOwned
    {
        get { return isOwned; }
        set
        {
            isOwned = value;
            SaveFurnitureData();
        }
    }*/
    public bool IsEquipped
    {
        get { return isEquipped; }
        set
        {
            isEquipped = value;
            SaveFurnitureData();
        }
    }

    public void SaveFurnitureData() // 가구 정보 저장
    {
        //PlayerPrefs.SetInt($"{furnitureName}_IsOwned", isOwned ? 1 : 0);
        PlayerPrefs.SetInt($"{furnitureName}_IsEquipped", isEquipped ? 1 : 0);
    }

    public void LoadFurnitureData() // 가구 정보 로드
    {
        //isOwned = PlayerPrefs.GetInt($"{furnitureName}_IsOwned", 0) == 1;
        isEquipped = PlayerPrefs.GetInt($"{furnitureName}_IsEquipped", 0) == 1;
    }

}
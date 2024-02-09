using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreProductManager : MonoBehaviour
{
    public TMP_Text playerMoney; // 플레이어가 보유 중인 돈
    public GameObject[] audioProducts; // 모든 상품
    public GameObject[] goodsProducts; // 모든 상품

    private void Update()
    {
        playerMoney.text = "잔고: " + PlayerPrefs.GetInt("Money") + "만원"; // 내 잔고 변경
    }

    void Start()
    {
        playerMoney.text = "잔고: " + PlayerPrefs.GetInt("Money") + "만원";
        // 음향기기와 굿즈에 판매할 아이템 종류랑 아이템 남은 개수 넣기
        LoadProductsData(audioProducts);
        LoadProductsData(goodsProducts);
    }

    public void LoadProductsData(GameObject[] allProductObj)
    {
        foreach (GameObject slotObj in allProductObj)
        {
            StoreProduct product = slotObj.GetComponentInChildren<StoreProduct>();
            product.LoadFurnitureStatus();
            Button button = slotObj.GetComponentInChildren<Button>();
            TextMeshProUGUI buttonText = slotObj.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log("상품명: " + product.Category_Index + " | 가격: " + product.price);
            if (product.IsOwned)
            {
                buttonText.text = "SOLD OUT";
                button.interactable = false;
            }
            else
            {
                buttonText.text = product.price + "만원";
            }
        }
    }
}

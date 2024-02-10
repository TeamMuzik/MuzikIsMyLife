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

        LoadProductsData(audioProducts);
        LoadGoodsProductsData(goodsProducts);
    }

    public void LoadProductsData(GameObject[] allProductObj)
    {
        foreach (GameObject slotObj in allProductObj)
        {
            StoreProduct product = slotObj.GetComponentInChildren<StoreProduct>();
            product.LoadFurnitureStatus();
            Button button = slotObj.GetComponentInChildren<Button>();
            TextMeshProUGUI buttonText = slotObj.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();
            // Debug.Log("상품명: " + product.Category_Index + " | 가격: " + product.price);
            if (!product.SaleStatus()) // 판매중인지
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

    public void LoadGoodsProductsData(GameObject[] allProductObj)
    {
        foreach (GameObject slotObj in allProductObj)
        {
            GoodsProduct product = slotObj.GetComponentInChildren<GoodsProduct>();
            Button button = slotObj.GetComponentInChildren<Button>();
            TextMeshProUGUI buttonText = slotObj.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();
            if (!product.SaleStatus()) // 판매중인지
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

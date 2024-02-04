using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreProduct : Furniture // Furniture을 상속받음
{
    public int price; // 가격
    public bool replaceable;
    public TMP_Text priceText;
    private StoreProductManager storeProductManager;

    private void Start()
    {
        priceText.text = price + "만원";
    }

    public void BuyProduct()
    {
        Button button = GetComponentInChildren<Button>();
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (StatusChanger.SpendMoney(price)) // 물건 구매 시도
        {
            if (replaceable)
            {
                ReplaceProduct();
            }
            IsOwned = true; // 구매 완료
            IsEquipped = true; // 장착 완료

            if (buttonText != null)
            {
                buttonText.text = "구매 완료"; // 텍스트를 변경하여 구매 완료로 설정
            }
            storeProductManager = FindObjectOfType<StoreProductManager>();
            storeProductManager.playerMoney.text = "잔고: " + PlayerPrefs.GetInt("Money") + "만원"; // 내 잔고 변경
        }
        else
        {
            buttonText.text = "잔고 부족"; // 텍스트를 변경
            Debug.Log("구매할 수 없습니다.");
        }
    }

    public void ReplaceProduct()
    {
        string[] CI = Category_Index.Split("_");
        string category = CI[0];
        int index = int.Parse(CI[1]);
        int previousIndex = PlayerPrefs.GetInt(category + "_CURRENT");
        PlayerPrefs.SetInt(category + "_" + previousIndex + "_IsEquipped", 0);
        PlayerPrefs.SetInt(category + "_CURRENT", index);
    }
}

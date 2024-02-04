using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreProduct : Furniture // Furniture을 상속받음
{
    public int price; // 가격
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
            this.IsOwned = true; // 구매 완료
            // funitureController에서 장착 상태를 변경해야 함...
            this.IsEquipped = true; // 장착 완료
            if (buttonText != null)
            {
                buttonText.text = "구매 완료"; // 텍스트를 변경하여 구매 불가로 설정
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
}

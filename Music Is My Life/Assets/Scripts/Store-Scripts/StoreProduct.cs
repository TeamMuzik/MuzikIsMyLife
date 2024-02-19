using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class StoreProduct : Furniture // Furniture을 상속받음
{
    public int price; // 가격
    public bool replaceable;
    public Button button;

    private TextMeshProUGUI buttonText;

    public void BuyProduct()
    {
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (StatusChanger.SpendMoney(price)) // 물건 구매 시도
        {
            UpgradeCoverAndDecreaseStress();
            if (replaceable)
            {
                ReplaceProduct();
            }
            IsOwned = true; // 구매 완료
            IsEquipped = true; // 장착 완료
            if (buttonText != null)
            {
                SetButtonTextAndColor("구매 완료", "#00B028"); // 텍스트를 변경하여 구매 완료로 설정
                button.interactable = false;
                StartCoroutine(MapSoldOutAfterDelay());
            }
        }
        else
        {
            SetButtonTextAndColor("잔고 부족", "#FF262C"); // 텍스트를 변경, 글자를 빨간색으로
            Debug.Log("구매할 수 없습니다.");
            StartCoroutine(MapPriceTextAfterDelay());
        }
    }

    IEnumerator MapPriceTextAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        SetButtonTextAndColor(price.ToString() + "만원", "#323232");
    }

    IEnumerator MapSoldOutAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        SetButtonTextAndColor("SOLD OUT", "#323232");
    }

    private void SetButtonTextAndColor(string text, string colorCode)
    {
        if (buttonText != null)
        {
            buttonText.text = text; // 텍스트 변경
            buttonText.color = ColorUtility.TryParseHtmlString(colorCode, out Color color) ? color : Color.black; // 컬러 코드를 Color로 변환하여 적용
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

    public void UpgradeCoverAndDecreaseStress()
    {
        int subsMin = PlayerPrefs.GetInt("Subs_Min");
        int subsMax = PlayerPrefs.GetInt("Subs_Max");
        int subsMultiplier = PlayerPrefs.GetInt("Subs_Multiplier");
        int stress;
        switch (price)
        {
            case 10:
                PlayerPrefs.SetInt("Subs_Max", subsMax + 500);
                stress = -5;
                //stress = -10;
                break;
            case 40:
                PlayerPrefs.SetInt("Subs_Min", subsMin + 1500);
                stress = -10;
                //stress = -30;
                break;
            case 100:
                PlayerPrefs.SetInt("Subs_Multiplier", subsMultiplier * 2);
                stress = -10;
                break;
            case 150:
                PlayerPrefs.SetInt("Subs_Multiplier", subsMultiplier * 3);
                stress = -10;
                break;
            default:
                throw new System.Exception("음향기기 상품 가격으로  10/40/100/150이 아닌 값이 입력되었습니다.");
        }
        StatusChanger.UpdateStress(stress); // 스트레스 0
        Debug.Log("음향기기 구입 -> 커버 구독자 수치, 스트레스 수치 업데이트" + stress);
    }
}

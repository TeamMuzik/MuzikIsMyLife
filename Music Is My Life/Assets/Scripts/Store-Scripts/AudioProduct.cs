using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AudioProduct : MonoBehaviour // Furniture을 상속받음
{
    public string category;
    public int index;
    public bool replaceable;
    public int price; // 가격
    public bool isOwned;
    public Button button;

    private TextMeshProUGUI buttonText;

    public void LoadOwnedStatus()
    {
        isOwned = PlayerPrefs.GetInt($"{category}_{index}_IsOwned", 0) == 1;
    }

    public void BuyAudioProduct()
    {
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText == null)
        {
            throw new System.Exception("buttonText가 없습니다.");
        }
        if (buttonText.text.Equals("장착하기")) // 이미 산 물건
        {
            ReplaceAudioProduct();
            return;
        }
        if (StatusChanger.SpendMoney(price)) // 물건 구매 시도
        {
            UpgradeCoverAndDecreaseStress();
            UpdateAudioProductData();
            Debug.Log("구매 성공");

            if (replaceable) // 교체 가능한 가구
            {
                ReplaceAudioProduct();
                SetButtonTextAndColor("구매 완료", "#00B028"); // 텍스트를 변경하여 구매 완료로 설정
                button.interactable = false;
                if (category.Equals("GUITAR")) // 기타라면 장착 클릭 가능
                {
                    StartCoroutine(MapEquipAfterDelay());
                    button.interactable = true;
                }
                else
                {
                    StartCoroutine(MapSoldOutAfterDelay());
                }
            }
            else
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

    IEnumerator MapEquipAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        SetButtonTextAndColor("장착하기", "#323232");
    }

    IEnumerator MapSoldOutAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        SetButtonTextAndColor("SOLD OUT", "#323232");
    }
    IEnumerator MapPriceTextAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        SetButtonTextAndColor(price.ToString() + "만원", "#323232");
    }

    private void SetButtonTextAndColor(string text, string colorCode)
    {
        if (buttonText != null)
        {
            buttonText.text = text; // 텍스트 변경
            buttonText.color = ColorUtility.TryParseHtmlString(colorCode, out Color color) ? color : Color.black; // 컬러 코드를 Color로 변환하여 적용
        }
    }

    // 구매에 대한 로직
    private void UpdateAudioProductData()
    {
        PlayerPrefs.SetInt($"{category}_{index}_IsOwned", 1);
        PlayerPrefs.SetInt($"{category}_CURRENT", index);
    }

    private void ReplaceAudioProduct()
    {
        PlayerPrefs.SetInt($"{category}_CURRENT", index);
    }

    private void UpgradeCoverAndDecreaseStress()
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class JjirasiGame : MonoBehaviour
{
    public int ClickNum;
    public int FameNum;
    public TMP_Text clickTxt;
    public TMP_Text fameTxt;
    public TMP_Text incfameTxt;
    public float LimitTime;
    public TMP_Text text_Timer;
    public Button Jjirasi;
    public Button Main;
    public GameObject jjirasi;
    private bool isClicked = false;
    public Image jjirasiImage; // 스프라이트 변경을 위한 Image 변수
    public Sprite normalSprite; // 기본 스프라이트
    public Sprite clickSprite; // 클릭 시 보여질 스프라이트



    void Start()
    {
        ClickNum = 0;
        FameNum = 0;
        clickTxt.text = "클릭 수 : 0";
        fameTxt.text = "명성 : 0";
        text_Timer.text = "남은 시간: 30";
        LimitTime = 30;
        incfameTxt.gameObject.SetActive(false);
        Main.gameObject.SetActive(false);
        jjirasiImage.sprite = normalSprite;

    }

    public void Click()
  {
      ClickNum++;
      clickTxt.text = "클릭 수 : " + ClickNum.ToString();
      Debug.Log("ClickNum: " + ClickNum);

      FameNum = ClickNum / 10; // FameNum을 Click 함수에서 갱신합니다.
      fameTxt.text = "명성 : " + FameNum.ToString();
      Debug.Log("FameNum: " + FameNum);

      if (!isClicked)
      {
          jjirasiImage.sprite = clickSprite; // 클릭 시 스프라이트 변경
          isClicked = true;
          StartCoroutine(ResetSpriteAfterDelay(0.1f)); // 0.5초 후 스프라이트를 원래대로 되돌리기
      }
  }

  IEnumerator ResetSpriteAfterDelay(float delay)
  {
      yield return new WaitForSeconds(delay);
      jjirasiImage.sprite = normalSprite; // 기본 스프라이트로 되돌리기
      isClicked = false; // 클릭 상태 초기화
  }



    private void Hide()
    {
        clickTxt.gameObject.SetActive(false);
        fameTxt.gameObject.SetActive(false);
        text_Timer.gameObject.SetActive(false);
        Jjirasi.gameObject.SetActive(false);
        Main.gameObject.SetActive(true);
        incfameTxt.gameObject.SetActive(true);
        jjirasi.gameObject.SetActive(false);
    }

    void Update()
    {
        if (LimitTime > 0)
        {
            LimitTime -= Time.deltaTime;
            text_Timer.text = "남은 시간 : " + Mathf.Round(LimitTime);
        }
        else
        {
            Hide();
            incfameTxt.text = "증가한 명성: " + FameNum.ToString();
            StatusChanger.UpdateBandFame(FameNum);
            jjirasiImage.gameObject.SetActive(false);
        }
    }
}

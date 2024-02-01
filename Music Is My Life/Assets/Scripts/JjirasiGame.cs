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
    private Animator animator;

    void Start()
    {
        ClickNum = 0;
        FameNum = 0;
        clickTxt.text = "클릭 수 : 0";
        fameTxt.text = "얻은 명성 : 0";
        text_Timer.text = "남은 시간: 10";
        LimitTime = 10;
        incfameTxt.gameObject.SetActive(false);
        Main.gameObject.SetActive(false);
        animator = jjirasi.GetComponent<Animator>();
    }

    public void Click()
    {
        ClickNum++;
        clickTxt.text = "클릭 수 :" + ClickNum.ToString();
        Debug.Log("ClickNum: " + ClickNum);

        FameNum = ClickNum / 10; // FameNum을 Click 함수에서 갱신합니다.
        fameTxt.text = "명성 : " + FameNum.ToString();
        Debug.Log("FameNum: " + FameNum);

        if (!isClicked)
        {
            animator.SetBool("Click", true);
            isClicked = true;
        }
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
            StatusChanger.UpdateFame(FameNum);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{

    public static bool isInBox;
    private FactoryGame FactoryGameInstance;
    private FactoryGameTimer FactoryGameTimerInstance;

    void Awake(){
        isInBox = false;
        FactoryGameInstance = FindObjectOfType<FactoryGame>();
        FactoryGameTimerInstance = FindObjectOfType<FactoryGameTimer>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");

        if (eventData.pointerDrag != null)
        {
            if (this.name.Replace("박스", "") == eventData.pointerDrag.name.Replace("(Clone)", "")) //인형을 올바른 박스에 넣었을 때
            {
                //Debug.Log("박스에 인형이 위치함");
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition; //인형을 박스의 위치로 이동

                FactoryGameInstance.moneyManager(); //돈 증가
                FactoryGameInstance.increaseStageNum(); //만든 인형의 개수 증가
                FactoryGameInstance.PlaySuccessSound(); //성공 사운드 재생
 
                Destroy(eventData.pointerDrag); //인형 삭제
                
            }
            else //인형을 잘못된 박스에 넣었을 때
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = FactoryGameDoll.OriginalPos; //잘못된 박스 위에 인형을 놓으면 드래그 전 위치로 
                StartCoroutine(FactoryGameTimerInstance.BlinkText(FactoryGameTimer.totalTime)); //타이머 빨간색으로 점멸
                FactoryGameTimer.totalTime -= 2f; //전체 시간 3초 감소
                FactoryGameInstance.PlayMistakeSound(); //실패 사운드 재생
            }
                
        }
    }

    
}

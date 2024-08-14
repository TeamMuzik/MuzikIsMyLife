using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FactoryGameDoll : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static float moveSpeed = 4.5f;
    public static Vector2 OriginalPos;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private bool isDragging = false;

    private FactoryGame FactoryGameInstance;
    private FactoryGameTimer FactoryGameTimerInstance;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        FactoryGameInstance = FindObjectOfType<FactoryGame>();
        FactoryGameTimerInstance = FindObjectOfType<FactoryGameTimer>();
    }

    void Update()
    {
        if (!isDragging)
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;


        if (FactoryGameTimer.totalTime <= 0)
        {
            Destroy(gameObject);
        }

        if (FactoryGameTimer.totalTime > 0)
        {
            if (rectTransform.anchoredPosition.y < -400)
            {
                Destroy(gameObject);
                FactoryGameTimer.totalTime -= 2f;
                FactoryGameInstance.StartBlinkText();
                FactoryGameInstance.PlayMistakeSound();
            }
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("드래그시작");
        OriginalPos = this.transform.position; //드래그 하기 전 원래 위치 저장
        isDragging = true;
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("드래그중");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        // Vector2 currentPose = eventData.position;
        // this.transform.position = currentPose;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("드래그끝");
        // Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = OriginalPos; //드래그 끝나면 원래 위치로 돌아감
        
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("마우스포인터온");
    }

}

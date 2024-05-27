using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FactoryGameDoll : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static float moveSpeed = 3f;
    private Vector3 drageOffset;
    private Vector2 OriginalPos;
    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isDragging = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>();
    }

    void Update()
    {
        if (!isDragging)
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        OriginalPos = this.transform.position; //드래그 하기 전 원래 위치 저장
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        // Vector2 currentPose = eventData.position;
        // this.transform.position = currentPose;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = OriginalPos; //드래그 끝나면 원래 위치로 돌아감
        isDragging = false;
    }

}

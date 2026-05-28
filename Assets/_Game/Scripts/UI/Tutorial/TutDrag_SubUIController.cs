using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutDrag_SubUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Action EventBeginDrag;
    public Action EventEndDrag;
    public Action EventPointerDown;
    public Action EventPointerUp;
    public Action EventClick;
    public void SetEventBeginDrag(Action callback) => this.EventBeginDrag = callback;
    public void SetEventEndDrag(Action callback) => this.EventEndDrag = callback;
    public void SetEventPointerDown(Action callback) => this.EventPointerDown = callback;
    public void SetEventPointerUp(Action callback) => this.EventPointerUp = callback;
    public void SetEventClick(Action callback) => this.EventClick = callback;
    public void OnBeginDrag(PointerEventData eventData)
    {
        this.EventBeginDrag?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.EventEndDrag?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.EventPointerDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.EventPointerUp?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.EventClick?.Invoke();
    }
}

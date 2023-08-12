using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Animator _animator;
    public void OnPointerDown(PointerEventData eventData)
    {
        _animator.Play("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _animator.Play("OnPointerUp");
    }
}

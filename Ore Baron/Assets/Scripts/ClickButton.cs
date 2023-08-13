using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Animator Animator;
    public int OreIndex;
    public BigNumber OrePerClick;
    public void OnPointerDown(PointerEventData eventData)
    {
        Animator.Play("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Animator.Play("OnPointerUp");
        GameController.Instance.AddOre(OreIndex, OrePerClick);
    }
}

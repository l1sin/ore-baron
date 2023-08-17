using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Animator Animator;
    public int OreIndex;
    public Image Image;

    public static ClickButton Instance;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Animator.Play("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Animator.Play("OnPointerUp");
        GameController.Instance.AddOre(OreIndex, GameController.Instance.OrePerClick * GameController.Instance.AdClickBonus);
        SoundManager.Instance.PlayMineSound();
    }
}

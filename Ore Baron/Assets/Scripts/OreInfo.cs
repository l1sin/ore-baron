using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OreInfo : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI AmountText;
    public TextMeshProUGUI PriceText;
    public Button SellButton;
    public Button ChangeOreButton;
    public int OreIndex;
    public GameObject Lock;

    public void OnEnable()
    {
        SellButton.onClick.AddListener(Sell);
        ChangeOreButton.onClick.AddListener(ChangeOre);
    }
    public void OnDisable()
    {
        SellButton.onClick.RemoveListener(Sell);
        ChangeOreButton.onClick.RemoveListener(ChangeOre);
    }

    public void Sell()
    {
        GameController.Instance.SellOre(OreIndex);
    }

    public void ChangeOre()
    {
        ClickButton.Instance.OreIndex = OreIndex;
        ClickButton.Instance.Image.sprite = Icon.sprite;
    }

    public void Unlock()
    {
        Lock.SetActive(false);
    }
}

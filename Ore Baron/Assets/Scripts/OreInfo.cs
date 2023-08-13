using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OreInfo : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI AmountText;
    public TextMeshProUGUI PriceText;
    public Button Button;
    public int OreIndex;

    public void OnEnable()
    {
        Button.onClick.AddListener(Sell);
    }
    public void OnDisable()
    {
        Button.onClick.RemoveListener(Sell);
    }

    public void Sell()
    {
        GameController.Instance.SellOre(OreIndex);
    }
}

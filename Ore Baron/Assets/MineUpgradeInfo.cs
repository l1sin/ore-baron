using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineUpgradeInfo : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI AmountText;
    public TextMeshProUGUI PriceText;
    public Button BuyButton;
    public int OreIndex;
    public GameObject Lock;

    public void OnEnable()
    {
        BuyButton.onClick.AddListener(BuyMine);
    }
    public void OnDisable()
    {
        BuyButton.onClick.RemoveListener(BuyMine);
    }

    public void BuyMine()
    {
        GameController.Instance.BuyMineUpgrade(OreIndex);
    }

    public void Unlock()
    {
        Lock.SetActive(false);
    }
}

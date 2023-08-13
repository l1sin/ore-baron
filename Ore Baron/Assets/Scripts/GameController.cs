using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public BigNumber Money;
    public BigNumber OrePerClick;
    public TextMeshProUGUI MoneyText;
    public List<OreType> Ores;
    public List<OreInfo> OreInfos;
    public GameObject OreInfoPrefab;
    public GameObject OreMenuContent;


    public static GameController Instance;

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
        DontDestroyOnLoad(gameObject);
        UpdateAllOres();
    }

    public void UpdateAllOres()
    {
        for (int i = 0; i < Ores.Count; i++)
        {
            OreInfos[i].Icon = Ores[i].Icon;
            OreInfos[i].NameText.text = Ores[i].Name;
            OreInfos[i].AmountText.text = $"Amount: {Ores[i].Amount} pcs";
            OreInfos[i].PriceText.text = $"Price: {Ores[i].Price} $/pcs ";
        }
    }

    public void UpdateMoney()
    {
        MoneyText.text = Money.ToString();
    }
    public void UpdateOre(int i)
    {
        OreInfos[i].Icon = Ores[i].Icon;
        OreInfos[i].NameText.text = Ores[i].Name;
        OreInfos[i].AmountText.text = $"Amount: {Ores[i].Amount} pcs";
        OreInfos[i].PriceText.text = $"Price: {Ores[i].Price} $/pcs ";
    }

    public void SetOresAmount()
    {
        for (int i = 0; i < Ores.Count; i++)
        {
            OreInfos[i].AmountText.text = $"Amount: {Ores[i].Amount} pcs";
        }
    }

    public void AddMoney(BigNumber money)
    {
        Money += money;
        UpdateMoney();
    }

    public void AddOre(int index, BigNumber amount)
    {
        Ores[index].Amount += amount;
        SetOresAmount();
    }

    public void SellOre(int oreIndex)
    {
        Instance.Money += Instance.Ores[oreIndex].Amount * Instance.Ores[oreIndex].Price;
        Ores[oreIndex].Amount = new BigNumber(10);
        UpdateOre(oreIndex);
        UpdateMoney();
    }


    [Serializable]
    public class OreType
    {
        public Image Icon;
        public string Name;
        public BigNumber Amount;
        public BigNumber Price;
    }
}

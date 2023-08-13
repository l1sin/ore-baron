using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Ores.
    public BigNumber Money;
    public BigNumber OrePerClick;
    public TextMeshProUGUI MoneyText;
    public List<OreType> Ores;
    public List<OreInfo> OreInfos;
    public List<MineInfo> MineInfos;
    public GameObject OreInfoPrefab;
    public GameObject OreMenuContent;
    public List<Button> BuyButtons;

    // Mines.
    public List<MineType> Mines;

    private float _tickTimer;
    private float _tickTime = 1f;

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
        SetMinesPrices();
        UpdateAllMines();
        UpdateMoney();
        _tickTimer = _tickTime;
    }

    public void Update()
    {
        _tickTimer -= Time.deltaTime;
        if (_tickTimer <= 0)
        {
            Tick();
            _tickTimer = _tickTime;
        }
    }

    public void SetMinesPrices()
    {
        for (int i = 0; i < Mines.Count; i++)
        {
            Mines[i].CurrentMinesPrice = Mines[i].MinesPrice * Mathf.FloorToInt(Mathf.Pow(1.6f, Mines[i].MinesAmount));
        }
    }

    public void Tick()
    {
        for (int i = 0; i < Mines.Count; i++)
        {
            AddOre(i, new BigNumber(0, Mines[i].MinesIncome * Mines[i].MinesAmount));
        }
    }

    public void UpdateAllMines()
    {
        for (int i = 0; i < Ores.Count; i++)
        {
            UpdateMine(i);
        }
    }
    public void UpdateMine(int i)
    {
        MineInfos[i].Icon.sprite = Ores[i].Icon;
        MineInfos[i].NameText.text = Ores[i].Name + " mine";
        MineInfos[i].AmountText.text = $"Amount: {Mines[i].MinesAmount} pcs";
        MineInfos[i].PriceText.text = $"Price: {Mines[i].CurrentMinesPrice} $/pcs ";
        MineInfos[i].IncomeTotal.text = $"Mining: {Mines[i].MinesIncome * Mines[i].MinesAmount} /sec ";
    }

    public void UpdateBuyMinesButtons()
    {
        for (int i = 0; i < Ores.Count; i++)
        {
            if (Money.CompareTo(Mines[i].CurrentMinesPrice) == -1)
            {
                MineInfos[i].BuyButton.interactable = false;
            }
            else
            {
                MineInfos[i].BuyButton.interactable = true;
            }
        }
    }

    public void UpdateAllOres()
    {
        for (int i = 0; i < Ores.Count; i++)
        {
            UpdateOre(i);
        }
    }
    public void UpdateOre(int i)
    {
        OreInfos[i].Icon.sprite = Ores[i].Icon;
        OreInfos[i].NameText.text = Ores[i].Name;
        OreInfos[i].AmountText.text = $"Amount: {Ores[i].Amount} pcs";
        OreInfos[i].PriceText.text = $"Price: {Ores[i].Price} $/pcs ";
    }

    public void UpdateMoney()
    {
        MoneyText.text = Money.ToString();
        UpdateBuyMinesButtons();
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
        Money += Ores[oreIndex].Amount * Ores[oreIndex].Price;
        Ores[oreIndex].Amount = new BigNumber(10);
        UpdateOre(oreIndex);
        UpdateMoney();
    }

    public void BuyMine(int oreIndex)
    {
        Money -= Mines[oreIndex].CurrentMinesPrice;
        Mines[oreIndex].MinesAmount++;
        Mines[oreIndex].CurrentMinesPrice = Mines[oreIndex].MinesPrice * (int)Mathf.Pow(Mines[oreIndex].MinesAmount, 2);
        UpdateMine(oreIndex);
        UpdateMoney();
    }


    [Serializable]
    public class OreType
    {
        public Sprite Icon;
        public string Name;
        public BigNumber Amount;
        public BigNumber Price;
    }

    [Serializable]
    public class MineType
    {
        public int MinesAmount;
        public int MinesIncome;
        public BigNumber MinesPrice;
        public BigNumber CurrentMinesPrice;
    }
}

using System;
using System.Collections;
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
    public List<MineUpgradeInfo> MineUpgradeInfos;
    public GameObject OreInfoPrefab;
    public GameObject OreMenuContent;

    // Mines.
    public List<MineType> Mines;

    private float _tickTimer;
    private float _tickTime = 1f;

    public static GameController Instance;

    // Premium.
    public bool PremiumDoubleMine;
    public bool PremiumDoubleClick;
    public bool Payed;

    public int PremiumMineBonus;
    public int PremiumClickBonus;

    // Ads.
    public bool AdDoubleMine;
    public bool AdDoubleClick;

    public int AdMineBonus;
    public int AdClickBonus;

    public float MineAdTime = 60f;

    public float ClickAdTime = 60f;

    public Button MineAdButton;
    public Button ClickAdButton;

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
        CalculatePrice();
        SetPremium(false);
        SetAd(false);
        UpdateAllOres();
        SetAllMinePrices();
        SetAllMineUpgradePrices();
        UpdateAllMines();
        UpdateAllMineUpgrades();
        SetAllMineIncome();
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
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (PremiumDoubleMine) PremiumDoubleMine = false;
            else PremiumDoubleMine = true;
            SetPremium(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (PremiumDoubleClick) PremiumDoubleClick = false;
            else PremiumDoubleClick = true;
            SetPremium(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (AdDoubleMine) AdDoubleMine = false;
            else AdDoubleMine = true;
            SetAd(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (AdDoubleClick) AdDoubleClick = false;
            else AdDoubleClick = true;
            SetAd(true);
        }
#endif
    }

    public IEnumerator DropMineAd()
    {
        yield return new WaitForSeconds(MineAdTime);
        ToggleMineAdBonus(false);
    }
    public IEnumerator DropClickAd()
    {
        yield return new WaitForSeconds(ClickAdTime);
        ToggleClickAdBonus(false);
    }

    public void BuyPremiumMine()
    {
        PremiumDoubleMine = true;
        Payed = true;
        SetPremium(true);
    }
    public void BuyPremiumClick ()
    {
        PremiumDoubleClick = true;
        Payed = true;
        SetPremium(true);
    }

    public void ToggleMineAdBonus(bool state)
    {
        if (state)
        {
            AdDoubleMine = true;
            SetAd(true);
            MineAdButton.interactable = false;
            StartCoroutine(DropMineAd());
        }
        else
        {
            AdDoubleMine = false;
            SetAd(true);
            MineAdButton.interactable = true;
        }
    }
    public void ToggleClickAdBonus(bool state)
    {
        if (state)
        {
            AdDoubleClick = true;
            SetAd(true);
            ClickAdButton.interactable = false;
            StartCoroutine(DropClickAd());
        }
        else
        {
            AdDoubleClick = false;
            SetAd(true);
            ClickAdButton.interactable = true;
        }
    }
    public void SetPremium(bool updateValues)
    {
        if (PremiumDoubleMine) PremiumMineBonus = 2;
        else PremiumMineBonus = 1;

        if (PremiumDoubleClick) PremiumClickBonus = 2;
        else PremiumClickBonus = 1;

        if (updateValues)
        {
            SetAllMineIncome();
            UpdateAllMines();
        }
    }
    public void SetAd(bool updateValues)
    {
        if (AdDoubleMine) AdMineBonus = 2;
        else AdMineBonus = 1;

        if (AdDoubleClick) AdClickBonus = 2;
        else AdClickBonus = 1;

        if (updateValues)
        {
            SetAllMineIncome();
            UpdateAllMines();
        }
    }
    public void SetAllMinePrices()
    {
        for (int i = 0; i < Mines.Count; i++)
        {
            SetMinePrice(i);
        }
    }
    public void SetAllMineUpgradePrices()
    {
        for (int i = 0; i < Mines.Count; i++)
        {
            SetMineUpgradePrice(i);
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

    public void UpdateAllMineUpgrades()
    {
        for (int i = 0; i < Ores.Count; i++)
        {
            UpdateMineUpgrade(i);
        }
    }
    public void UpdateMineUpgrade(int i)
    {
        MineUpgradeInfos[i].Icon.sprite = Ores[i].Icon;
        MineUpgradeInfos[i].NameText.text = Ores[i].Name + " mine upgrade";
        MineUpgradeInfos[i].AmountText.text = $"Amount: {Mines[i].MinesUpgrades} pcs";
        MineUpgradeInfos[i].PriceText.text = $"Price: {Mines[i].CurrentUpgradeMinesPrice} $/pcs ";
    }

    public void UpdateBuyMineUpgradeButtons()
    {
        for (int i = 0; i < Ores.Count; i++)
        {
            if (Money.CompareTo(Mines[i].CurrentUpgradeMinesPrice) == -1)
            {
                MineUpgradeInfos[i].BuyButton.interactable = false;
            }
            else
            {
                MineUpgradeInfos[i].BuyButton.interactable = true;
            }
        }
    }

    public void UpdateMoney()
    {
        MoneyText.text = Money.ToString();
        UpdateBuyMinesButtons();
        UpdateBuyMineUpgradeButtons();
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
        SetMinePrice(oreIndex);
        UpdateMine(oreIndex);
        UpdateMoney();
        Unlock(oreIndex);
    }
    public void SetMinePrice(int oreIndex)
    {
        Mines[oreIndex].CurrentMinesPrice = Mines[oreIndex].MinesPrice * ((int)Mathf.Pow(1.20f, Mines[oreIndex].MinesAmount));
    }

    public void BuyMineUpgrade(int oreIndex)
    {
        Money -= Mines[oreIndex].CurrentUpgradeMinesPrice;
        Mines[oreIndex].MinesUpgrades++;
        SetMineIncome(oreIndex);
        SetMineUpgradePrice(oreIndex);
        UpdateMineUpgrade(oreIndex);
        UpdateMine(oreIndex);
        UpdateMoney();
    }

    public void SetMineIncome(int oreIndex)
    {
        Mines[oreIndex].MinesIncome = (MineType.BaseIncome + MineType.BaseIncome * Mines[oreIndex].MinesUpgrades) * PremiumMineBonus * AdMineBonus;
    }
    public void SetAllMineIncome()
    {
        for (int i = 0; i < Ores.Count; i++)
        {
            SetMineIncome(i);
        }
    }
    public void SetMineUpgradePrice(int oreIndex)
    {
        Mines[oreIndex].CurrentUpgradeMinesPrice = Mines[oreIndex].MineUpgradePrice * ((int)Mathf.Pow(1.20f, Mines[oreIndex].MinesUpgrades));
    }

    public void Unlock(int oreIndex)
    {
        OreInfos[oreIndex].Unlock();
        MineUpgradeInfos[oreIndex].Unlock();
        if (oreIndex + 1 < Ores.Count) MineInfos[oreIndex + 1].Unlock();

    }

    public void CalculatePrice()
    {
        for (int i = 0; i < Ores.Count; i++)
        {
            Mines[i].MinesPrice = Ores[i].Price * (10 + i * (90 + 10 * i)); 
        }
        for (int i = 0; i < Ores.Count; i++)
        {
            Mines[i].MineUpgradePrice = Ores[i].Price * (50 + i * (150 + 10 * i));
        }
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
        public static int BaseIncome = 3;
        public int MinesUpgrades;
        public int MinesAmount;
        public int MinesIncome;
        public BigNumber MinesPrice;
        public BigNumber CurrentMinesPrice;
        public BigNumber MineUpgradePrice;
        public BigNumber CurrentUpgradeMinesPrice;
    }
}

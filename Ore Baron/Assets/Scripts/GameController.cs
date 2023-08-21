using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public List<Sprite> OreSprites;

    // Mines.
    public List<MineType> Mines;

    private float _tickTimer;
    private float _tickTime = 1f;

    private float _saveTimer;
    public float SaveTime = 10f;

    public static GameController Instance;

    // Ads.
    public bool AdDoubleMine;
    public bool AdDoubleClick;

    public int AdMineBonus;
    public int AdClickBonus;

    public float MineAdTime = 60f;

    public float ClickAdTime = 60f;

    // Other.
    public Button MineAdButton;
    public Button ClickAdButton;

    public GameObject MineAdIcon;
    public GameObject ClickAdIcon;

    public EarthInfo EarthInfo;
    public BigNumber EarthPrice;
    public GameObject WinMenu;
    public bool WinGame;

    public Localization Localization;
    public string lang = "ru";

    public GameObject RateWindow;
    public float RateTime = 300f;

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
        LoadGame();
        LoadLocalization();
        CalculatePrice();
        SetAd(false);
        UpdateAllOres();
        SetAllMinePrices();
        SetAllMineIncome();
        SetAllMineUpgradePrices();
        UpdateAllMines();
        UpdateAllMineUpgrades();
        UpdateMoney();
        UpdateEarthInfo();
        UnlockOnLoad();
        StartCoroutine(RateDelay());
        _tickTimer = _tickTime;
        _saveTimer = SaveTime;
#if UNITY_EDITOR
        Debug.Log("FullScreenAd");
#elif UNITY_WEBGL
        Yandex.FullScreenAd();
#endif
    }

    public void Update()
    {
        _saveTimer -= Time.deltaTime;
        if (_saveTimer <= 0)
        {
            SaveGame();
            _saveTimer = SaveTime;
        }

        _tickTimer -= Time.deltaTime;
        if (_tickTimer <= 0)
        {
            Tick();
            _tickTimer = _tickTime;
        }
    }

    public IEnumerator RateDelay()
    {
        yield return new WaitForSeconds(RateTime);
#if UNITY_EDITOR
        Debug.Log("CallRate");
#elif UNITY_WEBGL
        Yandex.CallRate();
#endif
    }
    public void ShowRateWindow()
    {
        RateWindow.SetActive(true);
    }
    public void LoadLocalization()
    {
        string language = "";
        if (Yandex.Instance != null) language = Yandex.Instance.Lang;
        TextAsset ta = Resources.Load<TextAsset>($"Localization/{language}");
        if (ta == null) ta = Resources.Load<TextAsset>($"Localization/{lang}");
        Localization = JsonUtility.FromJson<Localization>(ta.text);
    }

    public IEnumerator DropMineAd()
    {
        yield return new WaitForSeconds(MineAdTime);
        ToggleMineAdBonus(0);
    }
    public IEnumerator DropClickAd()
    {
        yield return new WaitForSeconds(ClickAdTime);
        ToggleClickAdBonus(0);
    }

    public void ToggleMineAdBonus(int state)
    {
        if (state == 1)
        {
            AdDoubleMine = true;
            SetAd(true);
            MineAdButton.interactable = false;
            MineAdIcon.SetActive(false);
            StartCoroutine(DropMineAd());
        }
        else
        {
            AdDoubleMine = false;
            SetAd(true);
            MineAdButton.interactable = true;
            MineAdIcon.SetActive(true);
        }
    }
    public void ToggleClickAdBonus(int state)
    {
        if (state == 1)
        {
            AdDoubleClick = true;
            SetAd(true);
            ClickAdButton.interactable = false;
            ClickAdIcon.SetActive(false);
            StartCoroutine(DropClickAd());
        }
        else
        {
            AdDoubleClick = false;
            SetAd(true);
            ClickAdButton.interactable = true;
            ClickAdIcon.SetActive(true);
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
        MineInfos[i].AmountText.text = $"{Localization.Amount}: {Mines[i].MinesAmount} {Localization.Pcs}";
        MineInfos[i].PriceText.text = $"{Localization.Price}: {Mines[i].CurrentMinesPrice} $/{Localization.Pcs} ";
        MineInfos[i].IncomeTotal.text = $"{Localization.Mining}: {Mines[i].MinesIncome * Mines[i].MinesAmount} {Localization.Pcs}/{Localization.Sec} ";
        if (Mines[i].MinesAmount >= 25)
        {
            MineInfos[i].Complete();
            MineInfos[i].PriceText.text = $"{Localization.Sold}";
        }
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
        OreInfos[i].AmountText.text = $"{Localization.Amount}: {Ores[i].Amount} {Localization.Pcs}";
        OreInfos[i].PriceText.text = $"{Localization.Price}: {Ores[i].Price} $/{Localization.Pcs} ";
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
        MineUpgradeInfos[i].AmountText.text = $"{Localization.Amount}: {Mines[i].MinesUpgrades} {Localization.Pcs}";
        MineUpgradeInfos[i].PriceText.text = $"{Localization.Price}: {Mines[i].CurrentUpgradeMinesPrice} $/{Localization.Pcs} ";
        if (Mines[i].MinesUpgrades >= 25)
        {
            MineUpgradeInfos[i].Complete();
            MineUpgradeInfos[i].PriceText.text = $"{Localization.Sold}";
        }
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
            OreInfos[i].AmountText.text = $"{Localization.Amount}: {Ores[i].Amount} {Localization.Pcs}";
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
        Mines[oreIndex].CurrentMinesPrice = Mines[oreIndex].MinesPrice * ((int)Mathf.Pow(2f, Mines[oreIndex].MinesAmount));
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
        Mines[oreIndex].MinesIncome = (MineType.BaseIncome + MineType.BaseIncome * Mines[oreIndex].MinesUpgrades) * AdMineBonus;
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
        Mines[oreIndex].CurrentUpgradeMinesPrice = Mines[oreIndex].MineUpgradePrice * ((int)Mathf.Pow(2f, Mines[oreIndex].MinesUpgrades));
    }

    public void Unlock(int oreIndex)
    {
        OreInfos[oreIndex].Unlock();
        MineUpgradeInfos[oreIndex].Unlock();
        if (oreIndex + 1 < Ores.Count) MineInfos[oreIndex + 1].Unlock();
        else
        {
            EarthInfo.Unlock();
            UpdateEarthInfo();
        }
    }

    public void UnlockOnLoad()
    {
        for (int i = 0; i < Mines.Count; i++)
        {
            if (Mines[i].MinesAmount > 0) Unlock(i);
        }
    }

    public void CalculatePrice()
    {
        for (int i = 0; i < Ores.Count; i++)
        {
            Ores[i].Price = new BigNumber(0, 1) * new BigNumber(0, 5).Power(i);
            Ores[i].Icon = OreSprites[i];
            Ores[i].Name = Localization.OreNames[i];
            OreInfos[i].Icon.sprite = Ores[i].Icon;
            OreInfos[i].NameText.text = Ores[i].Name;
        }
        for (int i = 0; i < Ores.Count; i++)
        {
            Mines[i].MinesPrice = Ores[i].Price * (10 + i * 50);
            MineInfos[i].Icon.sprite = Ores[i].Icon;
            MineInfos[i].NameText.text = Localization.MineNames[i];
        }
        for (int i = 0; i < Ores.Count; i++)
        {
            Mines[i].MineUpgradePrice = Ores[i].Price * (50 + i * 70);
            MineUpgradeInfos[i].Icon.sprite = Ores[i].Icon;
            MineUpgradeInfos[i].NameText.text = Localization.MineUpgradeNames[i];
        }
        EarthPrice = Mines[Mines.Count - 1].MinesPrice * 50;
    }

    public void BuyEarth()
    {
        Money -= EarthPrice;
        EarthInfo.Complete();
        UpdateEarthInfo();
        UpdateMoney();
        WinGame = true;
        WinMenu.SetActive(true);
    }
    public void UpdateEarthInfo()
    {
        if (!WinGame)
        {
            EarthInfo.PriceText.text = $"{Localization.Price}: {EarthPrice} $";
        }
        else
        {
            EarthInfo.PriceText.text = $"{Localization.Sold}";
            EarthInfo.Complete();
        }
    }

    public void SaveGame()
    {
        SaveFile save = new SaveFile();
        save.Money = Money;
        save.Ores = Ores;
        save.Mines = Mines;
        save.WinGame = WinGame;

        string json = JsonUtility.ToJson(save);
        File.WriteAllText($"{Application.persistentDataPath}/save.json", json);
        Debug.Log("LocalSave");
#if UNITY_EDITOR

#elif UNITY_WEBGL
        Yandex.SaveExtern(json);
#endif
    }

    public void LoadGame()
    {
        SaveFile save = Yandex.Instance.Save;

        Money = save.Money;
        Ores = save.Ores;
        Mines = save.Mines;
        WinGame = save.WinGame;
    }


    [Serializable]
    public class OreType
    {
        [NonSerialized] public Sprite Icon;
        [NonSerialized] public string Name;
        public BigNumber Amount;
        [NonSerialized] public BigNumber Price;
    }

    [Serializable]
    public class MineType
    {
        [NonSerialized] public static int BaseIncome = 3;
        public int MinesUpgrades;
        public int MinesAmount;
        [NonSerialized] public int MinesIncome;
        [NonSerialized] public BigNumber MinesPrice;
        [NonSerialized] public BigNumber CurrentMinesPrice;
        [NonSerialized] public BigNumber MineUpgradePrice;
        [NonSerialized] public BigNumber CurrentUpgradeMinesPrice;
    }

    [Serializable]
    public class SaveFile
    {
        // Ores.
        public BigNumber Money;
        public List<OreType> Ores;

        // Mines.
        public List<MineType> Mines;

        // Premium.
        public bool PremiumDoubleMine;
        public bool PremiumDoubleClick;
        public bool Payed;

        // Other.
        public bool WinGame;

        public bool Init;
    }
}

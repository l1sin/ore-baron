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
    public GameObject OreInfoPrefab;
    public GameObject OreMenuContent;
    public List<Sprite> OreSprites;

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

    // Other.
    public Button MineAdButton;
    public Button ClickAdButton;

    public GameObject PremiumMineButtonObject;
    public GameObject PremiumClickButtonObject;

    public GameObject PremiumMineCompletedIcon;
    public GameObject PremiumClickCompletedIcon;

    public EarthInfo EarthInfo;
    public BigNumber EarthPrice;
    public GameObject WinMenu;
    public bool WinGame;

    public bool Load;
    public bool Save;

    public Localization Localization;
    public Language lang;
    public enum Language
    {
        en,
        ru
    }

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
        if (File.Exists($"{Application.dataPath}/save.json") && Load)
        {
            string json = File.ReadAllText($"{Application.dataPath}/save.json");
            LoadGame(json);
        }
        DontDestroyOnLoad(gameObject);
        LoadLocalization();
        CalculatePrice();
        SetPremium(false);
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
        _tickTimer = _tickTime;

        Localization en = new Localization();
        string locjson = JsonUtility.ToJson(en);
        File.WriteAllText($"{Application.dataPath}/loctemplate.json", locjson);
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

    public void LoadLocalization()
    {
        TextAsset json = Resources.Load<TextAsset>($"Localization/{lang}");
        Localization = JsonUtility.FromJson<Localization>(json.text);
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
        SaveGame();
    }
    public void BuyPremiumClick()
    {
        PremiumDoubleClick = true;
        Payed = true;
        SetPremium(true);
        SaveGame();
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
        if (PremiumDoubleMine)
        {
            PremiumMineBonus = 2;
            PremiumMineButtonObject.SetActive(false);
            PremiumMineCompletedIcon.SetActive(true);

        }
        else
        {
            PremiumMineBonus = 1;
            PremiumMineButtonObject.SetActive(true);
            PremiumMineCompletedIcon.SetActive(false);
        }

        if (PremiumDoubleClick)
        {
            PremiumClickBonus = 2;
            PremiumClickButtonObject.SetActive(false);
            PremiumClickCompletedIcon.SetActive(true);
        }
        else
        {
            PremiumClickBonus = 1;
            PremiumClickButtonObject.SetActive(true);
            PremiumClickCompletedIcon.SetActive(false);
        }

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
        if (Save) SaveGame();
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
        MineInfos[i].IncomeTotal.text = $"{Localization.Mining}: {Mines[i].MinesIncome * Mines[i].MinesAmount} /{Localization.Sec} ";
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

    public string SaveGame()
    {
        SaveFile save = new SaveFile();
        save.Money = Money;
        save.Ores = Ores;
        save.Mines = Mines;
        save.PremiumDoubleMine = PremiumDoubleMine;
        save.PremiumDoubleClick = PremiumDoubleClick;
        save.Payed = Payed;
        save.WinGame = WinGame;

        string json = JsonUtility.ToJson(save);
        File.WriteAllText($"{Application.dataPath}/save.json", json);

        return json;
    }

    public void LoadGame(string json)
    {
        SaveFile save = JsonUtility.FromJson<SaveFile>(json);

        Money = save.Money;
        Ores = save.Ores;
        Mines = save.Mines;
        PremiumDoubleMine = save.PremiumDoubleMine;
        PremiumDoubleClick = save.PremiumDoubleClick;
        Payed = save.Payed;
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
    }
}

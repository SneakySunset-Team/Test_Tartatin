using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class TTUpgradeItem : MonoBehaviour
{
    [SerializeField]
    TTUpgradeData _upgradeData;
    
    [SerializeField]
    Button _upgradeBtn;

    [SerializeField]
    Image _upgradeIconImage;
    
    [SerializeField]
    LocalizeStringEvent _upgradeDescriptionText;
    
    [SerializeField]
    LocalizeStringEvent _upgradeDescriptionText_Max;
    
    [SerializeField]
    LocalizeStringEvent _upgradeBtnTxt;

    [SerializeField]
    TextMeshProUGUI _upgradeBtnTxt_Max;
    
    IntVariable _upgradeCost;
    IntVariable _currentValue;
    IntVariable _nextValue;
    LocalizedString _upgradeDescription;
    
    private EUpgradeType _upgradeType;

    public void Initialize(EUpgradeType upgradeType)
    {
        _upgradeType = upgradeType;
        _upgradeBtn.onClick.AddListener(()=> _upgradeData.LevelUpStat(upgradeType));
        _upgradeIconImage.sprite = _upgradeData.upgradeData[upgradeType].icon;
        
        _upgradeCost = _upgradeBtnTxt.StringReference["cost"] as IntVariable;
        _currentValue = _upgradeDescriptionText.StringReference["current-upgrade-value"] as  IntVariable;
        _nextValue = _upgradeDescriptionText.StringReference["next-upgrade-value"] as  IntVariable;
        
        // Create a NEW LocalizedString instance for THIS specific upgrade item
        var description = _upgradeData.upgradeData[upgradeType].localizationTableEntryReference.GetLocalizedString();
        
        _upgradeDescriptionText.StringReference["upgrade-type"] = description;
        _upgradeDescriptionText.RefreshString();
        
        _upgradeDescriptionText_Max.StringReference["upgrade-type"] = description;
        _upgradeDescriptionText_Max.RefreshString();
        
        gameObject.SetActive(true);
    }

    public void UpdateItem()
    {
        var uData = _upgradeData.upgradeData[_upgradeType];
        var currentLevelData = uData.levelData[uData.currentLevel];
        if (!_upgradeData.IsUpgradeAvailable(_upgradeType))
        {
            _upgradeBtn.interactable = false;
            
            _upgradeBtnTxt.gameObject.SetActive(false);
            _upgradeBtnTxt_Max.gameObject.SetActive(true);
            
            _upgradeDescriptionText.gameObject.SetActive(false);
            _upgradeDescriptionText_Max.gameObject.SetActive(true);
        }
        else
        {
            _upgradeBtnTxt.gameObject.SetActive(true);
            _upgradeBtnTxt_Max.gameObject.SetActive(false);
            
            _upgradeDescriptionText.gameObject.SetActive(true);
            _upgradeDescriptionText_Max.gameObject.SetActive(false);
            
            var nextLevelData = uData.levelData[uData.currentLevel + 1];
            bool canUpgrade = _upgradeData.CanUpgrade(_upgradeType);
            _upgradeCost.Value = currentLevelData.levelUpCost;
            _currentValue.Value = currentLevelData.value;
            _nextValue.Value = nextLevelData.value;
            _upgradeBtn.interactable = canUpgrade;
        }
    }
}
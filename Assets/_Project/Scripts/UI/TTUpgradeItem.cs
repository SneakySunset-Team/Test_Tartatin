using System;
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
    LocalizeStringEvent _upgradeBtnTxt;

    IntVariable _upgradeCost;
    IntVariable _currentValue;
    IntVariable _nextValue;
    LocalizedString _upgradeDescription;
    
    private EUpgradeType _upgradeType;
    
    public void Initialize(EUpgradeType upgradeType)
    {
        _upgradeType = upgradeType;
        _upgradeBtn.onClick.AddListener(()=> _upgradeData.LevelUpStat(upgradeType));
        _upgradeCost = _upgradeBtnTxt.StringReference["cost"] as IntVariable;
        _currentValue = _upgradeDescriptionText.StringReference["current-value"] as  IntVariable;
        _nextValue = _upgradeDescriptionText.StringReference["next-value"] as  IntVariable;
        _upgradeDescription = _upgradeDescriptionText.StringReference["current-value"] as LocalizedString;
        
        
        _upgradeDescription.TableEntryReference = _upgradeData.upgradeData[upgradeType]._descriptionTableEntryReference;
    }

    public void UpdateItem()
    {
        if (_upgradeData.IsUpgradeAvailable(_upgradeType))
        {
            gameObject.SetActive(false);
        }
        else
        {
            if(_upgradeData.IsUpgradeAvailable(_upgradeType))
            gameObject.SetActive(true);
            
        }
    }
}

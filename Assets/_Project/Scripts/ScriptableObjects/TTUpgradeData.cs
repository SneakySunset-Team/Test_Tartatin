using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Modules.Localization.Editor;
#endif
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.Serialization;

public enum EUpgradeType {StartGold, TurretDamage, TurretFireRate, MineGoldPerTickAmount}

[CreateAssetMenu(menuName = "UpgradeData")]
public class TTUpgradeData : SerializedScriptableObject
{
    [field : SerializeField]
    public Dictionary<EUpgradeType, TTStatUpgradeData> upgradeData { get; private set; }

    public void LevelUpStat(EUpgradeType upgradeType)
    {
        upgradeData[upgradeType].currentLevel++;
        STTGameManager.Instance.DeductDollars(upgradeData[upgradeType].levelData[upgradeData[upgradeType].currentLevel - 1].levelUpCost);
    }

    public int GetStatValue(EUpgradeType upgradeType)
    {
        var data = upgradeData[upgradeType];
        return data.levelData[data.currentLevel].value;
    }
    
    public bool IsUpgradeAvailable(EUpgradeType upgradeType)
    {
        var levelData= upgradeData[upgradeType].levelData;
        int currentLevel = upgradeData[upgradeType].currentLevel;
        return levelData != null && levelData.Length > currentLevel + 1;
    }

    public bool CanUpgrade(EUpgradeType upgradeType)
    {
        var levelData= upgradeData[upgradeType].levelData;
        int currentLevel = upgradeData[upgradeType].currentLevel;
        return STTGameManager.Instance.currentDollars >= levelData[currentLevel].levelUpCost;
    }

    public void ResetUpgrade(EUpgradeType upgradeType) => upgradeData[upgradeType].currentLevel = 0;
    
    
    #if UNITY_EDITOR
    [OnInspectorInit]
    private void Init()
    {
        if (upgradeData == null)
        {
            upgradeData = new Dictionary<EUpgradeType, TTStatUpgradeData>();
            foreach (EUpgradeType upgradeType in Enum.GetValues(typeof(EUpgradeType)))
            {
                upgradeData.Add(upgradeType, new TTStatUpgradeData());
            }
        }
    }
    #endif
}

[System.Serializable]
public struct SStatLevelData
{
    [FoldoutGroup("Data")]
    public int levelUpCost;
    [FoldoutGroup("Data")]
    public int value;
}
    
[System.Serializable]
public class TTStatUpgradeData
{
    public SStatLevelData[] levelData;
    public int currentLevel;
    public Sprite icon;
    [FormerlySerializedAs("ttLocalizationTableEntryReference")]
    [InlineProperty, HideLabel]
    public TTLocalizationTableEntryReference localizationTableEntryReference;
}





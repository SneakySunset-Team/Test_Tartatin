using Sirenix.OdinInspector;
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
    public Dictionary<EUpgradeType, SStatUpgradeData> upgradeData { get; private set; }

    public void LevelUpStat(EUpgradeType upgradeType)
    {
        upgradeData[upgradeType].currentLevel++;
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
        return levelData != null && levelData.Length > currentLevel;
    }

    public bool CanUpgrade(EUpgradeType upgradeType)
    {
        var levelData= upgradeData[upgradeType].levelData;
        int currentLevel = upgradeData[upgradeType].currentLevel;
        return TTGameManager.Instance.currentDollars >= levelData[currentLevel].levelUpCost;
    }
    
    
    #if UNITY_EDITOR
    [OnInspectorInit]
    private void Init()
    {
        if (upgradeData == null)
        {
            upgradeData = new Dictionary<EUpgradeType, SStatUpgradeData>();
            foreach (EUpgradeType upgradeType in Enum.GetValues(typeof(EUpgradeType)))
            {
                upgradeData.Add(upgradeType, new SStatUpgradeData());
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
public class SStatUpgradeData
{
    public SStatLevelData[] levelData;
    public int currentLevel;
    public Sprite icon;
    public TableEntryReference _descriptionTableEntryReference;
}
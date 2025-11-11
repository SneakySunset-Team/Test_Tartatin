using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class TTRunEconomyManager
{
    [HideInInspector]
    public Action OnGoldChangeEvent;
    [HideInInspector]
    public Action OnPriceChangeEvent;
    [HideInInspector]
    public Action OnDollarsGainEvent;
    
    
    
    [SerializeField]
    TTUpgradeData _upgradeData;
    
    [field : SerializeField, BoxGroup("Gold"), DisableInEditorMode]
    [Tooltip("Set by Upgrade System")]
    public int currentGold { get; private set; }

    [field : SerializeField, BoxGroup("Gold")]
    public int currentPrice { get; private set; } = 1;
    
    [field : SerializeField]
    public int runDollarsGain { get; private set; } = 0;
    
    public void AddGold(int amount)
    {
        currentGold += amount;
        OnGoldChangeEvent?.Invoke();
    }

    public void DeductGold(int amount)
    {
        currentGold -= amount;
        OnGoldChangeEvent?.Invoke();
    }

    public void IncreasePrice()
    {
        currentPrice = currentPrice * 2;
        OnPriceChangeEvent?.Invoke();
    }

    public void AddDollars(int amount)
    {
        runDollarsGain += amount;
        OnDollarsGainEvent?.Invoke();
    }
    
    public void OnStart()
    {
        currentGold = _upgradeData.GetStatValue(EUpgradeType.StartGold);
        OnPriceChangeEvent?.Invoke();
        OnGoldChangeEvent?.Invoke();
        OnDollarsGainEvent?.Invoke();
    }
}

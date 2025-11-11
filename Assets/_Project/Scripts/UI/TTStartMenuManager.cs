using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TTStartMenuManager : MonoBehaviour
{
    [SerializeField]
    Button _playBtn;

    [SerializeField]
    Button _upgradeBtn;

    [SerializeField]
    Button _skinBtn;

    [SerializeField]
    Toggle _frToggle;
    
    [SerializeField]
    LocalizeStringEvent _dollarsTxt;
    
    private IntVariable _dollars;
    
    void Start()
    {
        _playBtn.onClick.AddListener(()=> TTMenuManager.Instance.ChangeState(EMenuState.Loading));
        _frToggle.onValueChanged.AddListener((bool value) => ChangeLocal(value));
    }

    void OnEnable()
    {
        _dollars = _dollarsTxt.StringReference["dollars"] as  IntVariable;
        _dollars.Value = TTGameManager.Instance.currentDollars;
        TTGameManager.Instance.OnDollarsChange += OnDollarChange;
    }

    void OnDisable()
    {
        TTGameManager.Instance.OnDollarsChange -= OnDollarChange;
        
    }

    private void ChangeLocal(bool toFrench)
    {
        string name = toFrench ? "fr" : "en";
        var local = LocalizationSettings.AvailableLocales.GetLocale(new System.Globalization.CultureInfo(name));
        LocalizationSettings.SelectedLocale = local;
    }

    private void OnDollarChange()
    {
        _dollars.Value = TTGameManager.Instance.currentDollars;
    }
}

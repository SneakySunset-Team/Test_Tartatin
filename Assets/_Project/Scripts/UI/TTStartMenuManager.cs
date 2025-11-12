using Sirenix.OdinInspector;
using Sirenix.Utilities;
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
    Toggle _frToggle;
    
    [SerializeField]
    Button _playBtn, _upgradeBtn, _skinBtn;

    [SerializeField]
    Button _cheatGetDollarsBtn, _cheatResetDollarsBtn;

    [SerializeField]
    LocalizeStringEvent _dollarsTxt;
    
    [SerializeField]
    GameObject _upgradePanel, _skinPanel;
    
    private IntVariable _dollars;

    void Start()
    {
        _playBtn.onClick.AddListener(()=> STTMenuManager.Instance.ChangeState(EMenuState.Loading));
        _frToggle.onValueChanged.AddListener((bool value) => ChangeLocal(value));
        bool isFr = PlayerPrefs.GetString("current-local") == "fr";
        _frToggle.isOn = isFr;
        
        
        _cheatGetDollarsBtn.onClick.AddListener(()=> STTGameManager.Instance.AddDollars(100));
        _cheatResetDollarsBtn.onClick.AddListener(()=> STTGameManager.Instance.DeductDollars(STTGameManager.Instance.currentDollars));
        _dollars = _dollarsTxt.StringReference["dollars"] as  IntVariable;
        _dollars.Value = STTGameManager.Instance.currentDollars;
        _upgradeBtn.onClick.AddListener(() => _upgradePanel.SetActive(true));
        _skinBtn.onClick.AddListener(() => _skinPanel.SetActive(true));
    }

    void OnEnable()
    {
        STTGameManager.Instance.OnDollarsChangeEvent += OnDollarChange;
        // Subscription happens after the callback on state change
        OnDollarChange();
    }

    void OnDisable()
    {
        STTGameManager.Instance.OnDollarsChangeEvent -= OnDollarChange;
    }

    
    private void ChangeLocal(bool toFrench)
    {
        string name = toFrench ? "fr" : "en";
        var local = LocalizationSettings.AvailableLocales.GetLocale(new System.Globalization.CultureInfo(name));
        LocalizationSettings.SelectedLocale = local;
        PlayerPrefs.SetString("current-local", name);
        PlayerPrefs.Save();
    }

    private void OnDollarChange()
    {
        if (_dollars == null || STTGameManager.Instance == null) return;
        _dollars.Value = STTGameManager.Instance.currentDollars;
    }
}

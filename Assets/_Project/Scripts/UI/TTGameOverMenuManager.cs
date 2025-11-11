using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class TTGameOverMenuManager : MonoBehaviour
{
    [SerializeField]
    Button _tryAgainBtn, _mainMenuBtn;

    [SerializeField]
    LocalizeStringEvent _dollarsStringTxt;
    
    private IntVariable _dollars;

    void Start()
    {
        _tryAgainBtn.onClick.AddListener(()=> STTMenuManager.Instance.ChangeState(EMenuState.Loading));
        _mainMenuBtn.onClick.AddListener(()=> STTMenuManager.Instance.ChangeState(EMenuState.Start));
    }

    void OnEnable()
    {
        _dollars = _dollarsStringTxt.StringReference["dollars"] as  IntVariable;
        _dollars.Value = STTRunManager.Instance.runEconomyManager.runDollarsGain;
    }
}



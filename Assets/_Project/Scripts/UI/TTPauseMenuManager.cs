using System;
using UnityEngine;
using UnityEngine.UI;

public class TTPauseMenuManager : MonoBehaviour
{
    [SerializeField]
    Button _resumeBtn, _retryBtn, _mainMenuBtn, _cheatAddGoldBtn;

    void Awake()
    {
        _resumeBtn.onClick.AddListener(()=> STTMenuManager.Instance.ChangeState(EMenuState.Play));
        _retryBtn.onClick.AddListener(()=> STTMenuManager.Instance.ChangeState(EMenuState.Loading));
        _mainMenuBtn.onClick.AddListener(()=> STTMenuManager.Instance.ChangeState(EMenuState.Start));
        _cheatAddGoldBtn.onClick.AddListener(()=> STTRunManager.Instance.runEconomyManager.AddGold(100));
    }
}

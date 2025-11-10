using System;
using UnityEngine;
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

    void Start()
    {
        _playBtn.onClick.AddListener(()=> TTMenuManager.Instance.ChangeState(EMenuState.Loading));
    }
}

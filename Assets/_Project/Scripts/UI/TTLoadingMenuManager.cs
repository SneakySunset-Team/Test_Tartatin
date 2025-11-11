using System;
using UnityEngine;
using UnityEngine.UI;

public class TTLoadingMenuManager : MonoBehaviour
{
    [SerializeField]
    Image _loadingBar;

    void OnEnable()
    {
        STTMenuManager.Instance.OnLoadingProgressEvent += UpdateProgress;
        _loadingBar.fillAmount = 0;
    }

    void OnDisable()
    {
        STTMenuManager.Instance.OnLoadingProgressEvent -= UpdateProgress;
    }
    
    private void UpdateProgress(float progress)
    {
        _loadingBar.fillAmount = 1 - progress;
    }
}

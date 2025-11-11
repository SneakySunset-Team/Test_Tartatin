using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TTGameManager : TTSingleton<TTGameManager>
{
    [HideInInspector]
    public Action OnRunStartedEvent;
    [HideInInspector]
    public Action OnRunFinishedEvent;
    
    [HideInInspector]
    public Action OnDollarsChange;
    
    #if UNITY_EDITOR
    [SerializeField, BoxGroup("Level"), HideInPlayMode]
    SceneAsset _levelSceneAsset;
    #endif
    
    [SerializeField, BoxGroup("Level"), ReadOnly]
    string _levelSceneName;

    [field: SerializeField, BoxGroup("Currency")]
    public int currentDollars { get; private set; }
    
    [field: SerializeField, BoxGroup("Currency")]
    public TTUpgradeData _upgradeData { get; private set; }
    
    public void LoadLevelSceneAsync(Action<float> progressUpdateCallback, Action completeCallback) => StartCoroutine(LoadLevelSceneCoroutine(progressUpdateCallback, completeCallback));

    private IEnumerator LoadLevelSceneCoroutine(Action<float> progressUpdateCallback, Action completeCallback)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(_levelSceneName, LoadSceneMode.Single);
        while (asyncOp.isDone == false)
        {
            progressUpdateCallback?.Invoke(asyncOp.progress);
            yield return null;
        }
        completeCallback?.Invoke();
    }

    public void OnRunStarted()
    {
        OnRunStartedEvent?.Invoke();
    }

    public void OnRunFinished()
    {
        OnRunFinishedEvent?.Invoke();
    }

    public void AddDollars(int amount)
    {
        currentDollars += amount;
        OnDollarsChange?.Invoke();
    }

    public void DeductDollars(int amount)
    {
        currentDollars -= amount;
        OnDollarsChange?.Invoke();
    }
    
    #if UNITY_EDITOR
    void OnValidate()
    {
        _levelSceneName = _levelSceneAsset != null ? _levelSceneAsset.name : "";
    }
    #endif
}
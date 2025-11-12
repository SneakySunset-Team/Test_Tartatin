using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class STTGameManager : TTSingleton<STTGameManager>
{
    private const string PlayerPrefDollar = "dollar";
    private const string PlayerPrefSkinTableEntry = "skinTableEntry";
    
    [HideInInspector]
    public Action OnRunStartedEvent;
    [HideInInspector]
    public Action OnRunFinishedEvent;
    
    [HideInInspector]
    public Action OnDollarsChangeEvent;
    
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

    [SerializeField]
    SpriteRenderer _skinSpriteRenderer;

    [SerializeField]
    float _minLoadingTime = 1f;
    
    protected override void Awake()
    {
        base.Awake();
        currentDollars = PlayerPrefs.GetInt(PlayerPrefDollar, 0);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // Set Skin on start
        {
            var skinManager = FindFirstObjectByType<TTSkinPageManager>(FindObjectsInactive.Include);
            string skinStringValue = PlayerPrefs.GetString(PlayerPrefSkinTableEntry);
            long keyId = 0;
            if (long.TryParse(skinStringValue, out keyId)){}
            Sprite skin = skinManager.GetSkin(keyId);
            SetSkinSprite(skin);
        }
    }

    public void LoadLevelSceneAsync(Action<float> progressUpdateCallback, Action completeCallback) 
        => StartCoroutine(LoadLevelSceneCoroutine(progressUpdateCallback, completeCallback));

    private IEnumerator LoadLevelSceneCoroutine(Action<float> progressUpdateCallback, Action completeCallback)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(_levelSceneName, LoadSceneMode.Single);
        // minProgress in order to have the progress bar load for at least some time
        float minProgress = _minLoadingTime;
        while (asyncOp.isDone == false || minProgress > 0)
        {
            minProgress -= Time.unscaledDeltaTime;
            float progress = Mathf.Min(minProgress, asyncOp.progress);
            progressUpdateCallback?.Invoke(progress);
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
        OnDollarsChangeEvent?.Invoke();
        PlayerPrefs.SetInt(PlayerPrefDollar, currentDollars);
        PlayerPrefs.Save();
    }

    public void DeductDollars(int amount)
    {
        currentDollars -= amount;
        OnDollarsChangeEvent?.Invoke();
        PlayerPrefs.SetInt(PlayerPrefDollar, currentDollars);
        PlayerPrefs.Save();
    }

    public void SetSkinSprite(Sprite sprite, long skinId = -1)
    {
        _skinSpriteRenderer.sprite = sprite;
        if (skinId != -1)
        {
            PlayerPrefs.SetString(PlayerPrefSkinTableEntry, skinId.ToString());  
            PlayerPrefs.Save();
        }
    }
    
    #if UNITY_EDITOR
    void OnValidate()
    {
        _levelSceneName = _levelSceneAsset != null ? _levelSceneAsset.name : "";
    }
    #endif
}
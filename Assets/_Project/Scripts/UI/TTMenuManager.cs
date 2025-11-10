using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public enum EMenuState {Start, Loading, Play, Pause, GameOver}

[Serializable]
public class TTMenuManager :  TTSingleton<TTMenuManager>
{
    [field : SerializeField, ReadOnly, HideInEditorMode] public EMenuState currentState { get; private set; } = EMenuState.Start;
    [field : SerializeField, ReadOnly, HideInEditorMode] public EMenuState previousState { get; private set; } = EMenuState.Start;
    [field : SerializeField, HideInPlayMode] private EMenuState _startState = EMenuState.Start;
    [OdinSerialize, DictionaryDrawerSettings()] private Dictionary<EMenuState, SMenuSettings> _menuSettings = new Dictionary<EMenuState, SMenuSettings>();
    [field : SerializeField, ReadOnly, HideInEditorMode] public bool isGamePaused { get; private set; } = false;
     
      
    [SerializeField] private bool _allowGizmos;
    [ShowIf("_allowGizmos"), SerializeField, FoldoutGroup("Gizmos")] private EMenuState _menuGizmos;
    [ShowIf("_allowGizmos"), SerializeField, FoldoutGroup("Gizmos")] private int _menuGizmosResolution;
    [ShowIf("_allowGizmos"), SerializeField, FoldoutGroup("Gizmos")] private float _menuLineRadius;
    [ShowIf("_allowGizmos"), SerializeField, FoldoutGroup("Gizmos")] private float _menuInnerLineRadius;
    [ShowIf("_allowGizmos"), SerializeField, FoldoutGroup("Gizmos")] private float _menuGizmosAnchoreRadius;
    [ShowIf("_allowGizmos"), SerializeField, FoldoutGroup("Gizmos")] private float _menuGizmosLinePointRadius;
    
    private void Start()
    {
        foreach (EMenuState menuState in Enum.GetValues(typeof(EMenuState)))
        {
            if (!_menuSettings.ContainsKey(menuState))
            {
                Debug.LogWarning($"No Menu Settings for the menu : {menuState.ToString()}");
                continue;
            }
            _menuSettings[menuState].Initialize();
        }
        ChangeState(_startState);
    }

    public void ChangeState(EMenuState newState)
    {
        previousState = currentState;
        currentState = newState;
        OnStateExit();
        OnStateEnter();
    }

    private void OnStateEnter()
    {
        _menuSettings[currentState].menuCanvas.gameObject.SetActive(true);
        StartCoroutine(TransitionsCoroutine());
        
        switch (currentState)
        {
            case EMenuState.Start:
                SetToPaused(true);
                break;
            case EMenuState.Loading:
                SetToPaused(true);
                break;
            case EMenuState.Play:
                SetToPaused(false);
                break;
            case EMenuState.Pause:
                SetToPaused(true);
                break;
            case EMenuState.GameOver:
                SetToPaused(true);
                break;
        }
    }
    
    private void OnStateExit()
    {
    }
    
    private IEnumerator TransitionsCoroutine()
    {
        int activeTransitionsNumber = 0;
        foreach (var transition in _menuSettings[currentState].transitions)
        {
            activeTransitionsNumber++;
            transition.DoTransition(true, ()=> activeTransitionsNumber = activeTransitionsNumber - 1);
        }
        
        foreach (var transition in _menuSettings[previousState].transitions)
        {
            activeTransitionsNumber++;
            transition.DoTransition(false, ()=> activeTransitionsNumber = activeTransitionsNumber - 1);
        }
        
        yield return new WaitUntil(() => activeTransitionsNumber == 0);
        OnTransitionsOver();
    }

    private void OnTransitionsOver()
    {
        _menuSettings[previousState].menuCanvas.gameObject.SetActive(false);
        switch (currentState)
        {
            case EMenuState.Loading :
                // TODO : Add update progress bar Callback
                TTGameManager.Instance.LoadLevelSceneAsync(null, ()=>ChangeState(EMenuState.Play));
                break;
        }
    }

    private void SetToPaused(bool isPaused)
    {
        isGamePaused = isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    [ContextMenu("Reset Menu Settings")]
    public void MenuBtn_ResetMenuSettings()
    {
        _menuSettings.Clear();
        foreach (EMenuState menuState in Enum.GetValues(typeof(EMenuState)))
        {
            _menuSettings.Add(menuState, new SMenuSettings());
        }
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!_allowGizmos || Application.isPlaying) return;

        foreach (var ms in _menuSettings)
        {
            if(ms.Value.menuCanvas && ms.Value.menuCanvas.gameObject.activeInHierarchy && _menuGizmos != ms.Key) ms.Value.menuCanvas.gameObject.SetActive(false);
            if(ms.Value.menuCanvas && !ms.Value.menuCanvas.gameObject.activeInHierarchy && _menuGizmos == ms.Key) ms.Value.menuCanvas.gameObject.SetActive(true);
        }
        
        SMenuSettings menuSetting = _menuSettings.Where(x => x.Key == _menuGizmos).First().Value;
        if (menuSetting.transitions == null) return;
        
        foreach (var transition in menuSetting.transitions)
        {
            var moveTransition = transition as TTUITransition_Move;
            if (moveTransition == null || moveTransition.canvasGroup == null || !(moveTransition is TTUITransition_Move)) continue;
            RectTransform target = moveTransition.canvasGroup.transform as RectTransform;
            Vector2 offset = target.position - target.TransformPoint(target.anchoredPosition);

            Vector2? previousPos = null;
            for (int i = 0; i < _menuGizmosResolution + 1; i++)
            {
                float xLerp = moveTransition.xAnimationCurve != null ? Mathf.Lerp(target.anchoredPosition.x + moveTransition.movement.x, target.anchoredPosition.x, moveTransition.xAnimationCurve.Evaluate((float)i / _menuGizmosResolution)) : 0;
                float yLerp = moveTransition.yAnimationCurve != null ? Mathf.Lerp(target.anchoredPosition.y + moveTransition.movement.y, target.anchoredPosition.y, moveTransition.yAnimationCurve.Evaluate((float)i / _menuGizmosResolution)) : 0;
                Vector3 lerpPos = new Vector3(xLerp, yLerp, 0);

                // Convert lerpPos from anchored position to world position
                Vector3 worldLerpPos = target.TransformPoint(lerpPos) + (Vector3)offset;
                if (previousPos == null) previousPos = worldLerpPos;
                if (i == 0 || i == _menuGizmosResolution)
                {
                    Gizmos.color = Color.black;
                    Handles.color = Color.black;
                    Gizmos.DrawSphere(worldLerpPos, _menuGizmosAnchoreRadius);
                }

                Gizmos.DrawSphere(worldLerpPos, _menuGizmosLinePointRadius);
                Handles.color = moveTransition.gizmosColor;
                Handles.DrawLine(previousPos.Value, worldLerpPos, _menuLineRadius);
                Handles.color = Color.black;
                Handles.DrawLine(previousPos.Value, worldLerpPos, _menuInnerLineRadius);
                previousPos = worldLerpPos;
            }
        }
    }
#endif
}

[Serializable]
public struct SMenuSettings
{
    [SerializeField]
    public Canvas menuCanvas;
        
    [SerializeField]
    public TTUITransition[] transitions;

    public void Initialize()
    {
        if(transitions == null) return;
        transitions.ForEach(transition => transition.Initialize());
    }

    public SMenuSettings(int foo)
    {
        transitions = new TTUITransition[0];
        menuCanvas = null;
    }
}


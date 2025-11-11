using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public enum EPoolItem {Ennemy, Turret, Mine, Bullet}
public class TTRunManager : TTSingleton<TTRunManager>
{
    [SerializeField, FoldoutGroup("Pooling")] TTPoolingData _poolingData;
    [SerializeField, FoldoutGroup("Pooling")] Transform _activePoolFolder;
    [SerializeField, FoldoutGroup("Pooling")] Transform _inactivePoolFolder;

    [field : SerializeField, HideInInspector] public TTPoolingSystem<EPoolItem, MonoBehaviour> pool { get; private set; }
    [field : SerializeField] public TTWaveManager waveManager { get; private set; }
    [field : SerializeField] public TTEconomyManager economyManager { get; private set; }
    [field : SerializeField, HideInInspector] public TTBuildingManager buildingManager { get; private set; }

    public void GameOver()
    {
        Debug.Log("Game Over");
        TTGameManager.Instance.OnRunFinished();
        TTMenuManager.Instance.ChangeState(EMenuState.GameOver);
    }
    
    protected override void Awake()
    {
        base.Awake();
        pool = new TTPoolingSystem<EPoolItem, MonoBehaviour>
            (_poolingData.poolingElements, _inactivePoolFolder, _activePoolFolder);
    }

    // yield a frame to wait for Play Menu to be Enabled
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        TTGameManager.Instance.OnRunStarted();
        buildingManager.OnStart();
        economyManager.OnStart();
        waveManager.OnStart();
    }

    void OnEnable()
    {
        buildingManager.OnEnable();
    }

    void OnDisable()
    {
        buildingManager.OnDisable();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        waveManager.OnDrawGizmos();
    }
#endif
}
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public enum EPoolItem {Ennemy, Turret, Mine, Bullet}
public class STTRunManager : TTSingleton<STTRunManager>
{
    [SerializeField, FoldoutGroup("Pooling")] TTPoolingData _poolingData;
    [SerializeField, FoldoutGroup("Pooling")] Transform _activePoolFolder;
    [SerializeField, FoldoutGroup("Pooling")] Transform _inactivePoolFolder;

    [field : SerializeField, HideInInspector] public TTPoolingSystem<EPoolItem, MonoBehaviour> pool { get; private set; }
    [field : SerializeField] public TTWaveManager waveManager { get; private set; }
    [field : SerializeField] public TTRunEconomyManager runEconomyManager { get; private set; }
    [field : SerializeField, HideInInspector] public TTBuildingManager buildingManager { get; private set; }

    public void GameOver()
    {
        Debug.Log("Game Over");
        STTGameManager.Instance.AddDollars(runEconomyManager.runDollarsGain);
        STTGameManager.Instance.OnRunFinished();
        STTMenuManager.Instance.ChangeState(EMenuState.GameOver);
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
        yield return new WaitForSeconds(1f);
        STTGameManager.Instance.OnRunStarted();
        buildingManager.OnStart();
        runEconomyManager.OnStart();
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
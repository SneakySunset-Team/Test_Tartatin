using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class TTRunManager : TTSingleton<TTRunManager>
{
    public enum EPoolItem {Ennemy, Turret, Mine}
    [SerializeField, ReadOnly, HideInEditorMode]
    public TTPoolingSystem<EPoolItem, MonoBehaviour> pool;
    
    
    [SerializeField, FoldoutGroup("Pooling")] TTPoolingData _poolingData;
    [SerializeField, FoldoutGroup("Pooling")] Transform _activePoolFolder;
    [SerializeField, FoldoutGroup("Pooling")] Transform _inactivePoolFolder;

    [SerializeField, FoldoutGroup("Wave Management")]
    float _spawningPositionY;

    [SerializeField, FoldoutGroup("Wave Management")]
    float _spawningWidth;
    
    [SerializeField, FoldoutGroup("Wave Management/Gizmos")]
    string _gizmosIconPath;
    
    [SerializeField, FoldoutGroup("Wave Management/Gizmos")]
    Color _gizmosLineColor = Color.red;
    
    [SerializeField, FoldoutGroup("Wave Management/Gizmos")]
    float _gizmosLineWidth = 1;

    [field : SerializeField, BoxGroup("Gold")] 
    public int _currentGoldamount { get; private set; }

    [field : SerializeField, BoxGroup("Gold")]
    public int _currentPrice { get; private set; } = 1;
    
    void Awake()
    {
        pool = new TTPoolingSystem<EPoolItem, MonoBehaviour>(_poolingData.poolingElements, _activePoolFolder, _inactivePoolFolder);
    }

    private void SpawnEnnemy()
    {
        float randomPositionValue = UnityEngine.Random.Range(-1f, 1f);
        Vector3 spawnPosition = new Vector3(randomPositionValue * _spawningWidth, _spawningPositionY, 0);
        pool.Get(EPoolItem.Ennemy, spawnPosition);
    }

    public void AddGold(int amount)
    {
        _currentGoldamount += amount;
    }

    public void IncreatePrice()
    {
        _currentPrice = _currentPrice * 2;
    }
    
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(new Vector3(-_spawningWidth, _spawningPositionY, 0), _gizmosIconPath);
        Gizmos.DrawIcon(new Vector3(_spawningWidth, _spawningPositionY, 0), _gizmosIconPath);
        Handles.color = _gizmosLineColor;
        Handles.DrawLine(new Vector3(-_spawningWidth, _spawningPositionY, 0), new Vector3(_spawningWidth, _spawningPositionY, 0), _gizmosLineWidth);
    }
#endif
}
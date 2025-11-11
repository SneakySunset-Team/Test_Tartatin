using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class TTWaveManager
{
    [SerializeField, FoldoutGroup("Wave Management")]
    float _spawningPositionY;

    [SerializeField, FoldoutGroup("Wave Management")]
    float _spawningWidth;
    
    [SerializeField, FoldoutGroup("Wave Management")]
    float _timeBeforeFirstWave = 30f;

    [SerializeField, FoldoutGroup("Wave Management")]
    float _timeInBetweenWaves = 10f;

    
    [SerializeField, FoldoutGroup("Wave Management")]
    int _minNumberOfEnnemies, _maxNumberOfEnnemies;

    [SerializeField, FoldoutGroup("Wave Management")]
    AnimationCurve _waveCurve;

    [SerializeField, FoldoutGroup("Wave Management")]
    int _numberOfWavesBeforeReachingMaximum;

    [SerializeField, FoldoutGroup("Wave Management")]
    float _minTimerInBetweenSpawns = .2f;
    
    [SerializeField, FoldoutGroup("Wave Management")]
    float _maxTimerInBetweenSpawns = 1f;
    
    [SerializeField, FoldoutGroup("Wave Management")]
    int _numOfEnnemieByWavePostCurve = 2;
    
    [SerializeField, FoldoutGroup("Wave Management/Gizmos")]
    string _gizmosIconPath;
    
    [SerializeField, FoldoutGroup("Wave Management/Gizmos")]
    Color _gizmosLineColor = Color.red;
    
    [SerializeField, FoldoutGroup("Wave Management/Gizmos")]
    float _gizmosLineWidth = 1;

    
    public void OnStart()
    {
        STTRunManager.Instance.StartCoroutine(WaveCoroutine());
    }
    
    private void SpawnEnnemy()
    {
        float randomPositionValue = UnityEngine.Random.Range(-1f, 1f);
        Vector3 spawnPosition = new Vector3(randomPositionValue * _spawningWidth, _spawningPositionY, 0);
        STTRunManager.Instance.pool.Get(EPoolItem.Ennemy, spawnPosition);
    }
    
    #if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.DrawIcon(new Vector3(-_spawningWidth, _spawningPositionY, 0), _gizmosIconPath);
        Gizmos.DrawIcon(new Vector3(_spawningWidth, _spawningPositionY, 0), _gizmosIconPath);
        Handles.color = _gizmosLineColor;
        Handles.DrawLine(new Vector3(-_spawningWidth, _spawningPositionY, 0), new Vector3(_spawningWidth, _spawningPositionY, 0), _gizmosLineWidth);
    }
    #endif

    IEnumerator WaveCoroutine()
    {
        int waveNumber = 0;
        yield return new WaitForSeconds(_timeBeforeFirstWave);

        while (true)
        {
            waveNumber++;
            int numberOfEnnemies = waveNumber > _numberOfWavesBeforeReachingMaximum ?
                _numberOfWavesBeforeReachingMaximum + (waveNumber - _numberOfWavesBeforeReachingMaximum) * _numOfEnnemieByWavePostCurve :
                Mathf.RoundToInt(Mathf.Lerp(_minNumberOfEnnemies, _maxNumberOfEnnemies, _waveCurve.Evaluate((float)waveNumber / _numberOfWavesBeforeReachingMaximum)));
                    
                
            STTRunManager.Instance.StartCoroutine(SpawnWaveCoroutine(numberOfEnnemies));
            yield return new WaitForSeconds(_timeInBetweenWaves);
        }
    }

    IEnumerator SpawnWaveCoroutine(int ennemyNumber)
    {
        for (int i = 0; i < ennemyNumber; i++)
        {
            SpawnEnnemy();
            yield return new WaitForSeconds(Random.Range(_minTimerInBetweenSpawns, _maxTimerInBetweenSpawns));
        }
    }
}

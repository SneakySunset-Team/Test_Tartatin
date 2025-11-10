using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class TTMineBehaviour : MonoBehaviour
{
    IEnumerator _miningRoutine;
    WaitForSeconds _miningWait = new WaitForSeconds(1f);
    
    [SerializeField, HideInPlayMode]
    int _baseGoldAmountByTick;
    
    [SerializeField, HideInEditorMode]
    private int _currentGoldAmountByTick;
    

    void OnEnable()
    {
        _currentGoldAmountByTick = _baseGoldAmountByTick;
        _miningRoutine = MiningCoroutine();
        StartCoroutine(_miningRoutine);
    }

    void OnDisable()
    {
        StopCoroutine(_miningRoutine);
    }

    IEnumerator MiningCoroutine()
    {
        while (true)
        {
            yield return _miningWait;
            TTRunManager.Instance.AddGold(_currentGoldAmountByTick);
        }
    }
}

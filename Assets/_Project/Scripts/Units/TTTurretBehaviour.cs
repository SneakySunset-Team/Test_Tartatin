using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TTTurretBehaviour : MonoBehaviour
{
    [SerializeField]
    private TTUpgradeData _upgradeData;
    
    [SerializeField]
    float _range;

    [SerializeField]
    int _gizmoCircleResolution;

    [SerializeField]
    private TTBulletBehaviour _bulletPrefab;
    
    [SerializeField]
    private Transform _firePoint;

    [SerializeField]
    CircleCollider2D _detectionCollider;
    
    WaitUntil _waitUntilEnnemiesInRange;
    List<TTEnnemyBehaviour> _ennemiesInRange = new List<TTEnnemyBehaviour>();
    TTGridElement _gridElement;
    bool _isHidden;
    WaitUntil _waitUntilNotHidden;
    
    void OnEnable()
    {
        _gridElement = gameObject.GetComponent<TTGridElement>();
        _gridElement.OnHideEvent += OnHide;
        _gridElement.OnShowEvent += OnShow;
        
        _waitUntilEnnemiesInRange = new WaitUntil(()=> _ennemiesInRange.Count > 0);
        _waitUntilNotHidden = new WaitUntil(() => !_isHidden);
        StartCoroutine(FireCoroutine());
        
        _detectionCollider.radius = _range / transform.localScale.y;
    }

    void OnDisable()
    {
        StopAllCoroutines();
        ClearEnnemies();
        _gridElement.OnHideEvent -= OnHide;
        _gridElement.OnShowEvent -= OnShow;
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            // Uncached because it could be dynamic
            yield return new WaitForSeconds(1f / _upgradeData.GetStatValue(EUpgradeType.TurretFireRate));
            yield return _waitUntilEnnemiesInRange;
            yield return _waitUntilNotHidden;
            Fire();
        }
    }

    private void Fire()
    {
        if(_ennemiesInRange.Count == 0) return;
        _ennemiesInRange.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);
        if(_ennemiesInRange.Count == 0) return;
        
        var bullet = STTRunManager.Instance.pool.Get(EPoolItem.Bullet, _firePoint.position) as  TTBulletBehaviour;
        if (bullet.ennemy != null)
        {
            Debug.LogWarning("Pooled an already active bullet");
        }
        bullet.Initialize(_ennemiesInRange[0], _upgradeData.GetStatValue(EUpgradeType.TurretDamage));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ennemy" && other.TryGetComponent<TTEnnemyBehaviour>(out var ennemy))
        {
            RegisterEnnemy(ennemy);
        }
    }

    private void RegisterEnnemy(TTEnnemyBehaviour ennemy)
    {
        if (_ennemiesInRange.Contains(ennemy))
        {
            Debug.LogWarning("Ennemy entered range while already beeing registered in ennemy list", ennemy);
            return;
        }
        ennemy.OnDisableEvent += UnregisterEnnemy;
        _ennemiesInRange.Add(ennemy);
    }

    private void UnregisterEnnemy(TTEnnemyBehaviour ennemy)
    {
        if (!_ennemiesInRange.Contains(ennemy))
        {
            Debug.LogWarning("Ennemy exit range while not beeing registered in ennemy list", ennemy);
            return;
        }
        ennemy.OnDisableEvent -= UnregisterEnnemy;
        _ennemiesInRange.Remove(ennemy);
    }

    private void ClearEnnemies()
    {
        for (int i = _ennemiesInRange.Count - 1; i >= 0; i--)
        {
            UnregisterEnnemy(_ennemiesInRange[i]);
        }
    }

    private void OnHide()
    {
        _isHidden = true;
    }

    private void OnShow()
    {
        _isHidden = false;
    }
    
    void OnDrawGizmos()
    {
        Vector3[] lineList = new Vector3[_gizmoCircleResolution];
        for (int i = 0; i < _gizmoCircleResolution; i++)
        {
            lineList[i] = transform.position + Quaternion.AngleAxis(360 * (float)i / _gizmoCircleResolution, Vector3.forward) * Vector3.up * _range;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLineStrip(lineList, true);
    }
}
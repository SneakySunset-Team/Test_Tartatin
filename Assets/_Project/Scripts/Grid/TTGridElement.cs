using Lean.Touch;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TTGridElement : MonoBehaviour
{
    [ReadOnly]
    public bool isAnchored = false;

    [SerializeField, ReadOnly, HideInEditorMode]
    public List<TTCell> cells = new List<TTCell>();

    [SerializeField]
    int _baseHp;

    [SerializeField, HideInEditorMode]
    int _currentHp;

    void OnEnable()
    {
        isAnchored = false;
        _currentHp = _baseHp;
        cells = new List<TTCell>();
    }

    void Update()
    {
        if (!isAnchored)
        {
            transform.position = LeanTouch.Fingers[0].GetWorldPosition(10, Camera.main);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ennemy")
        {
            // TODO : Get Ennemy Damage
            int ennemyDamage = 1;
            _currentHp -= ennemyDamage;
            if (_currentHp <= 0)
            {
                cells.ForEach(cell => cell.ClearGridElement());
                TTRunManager.Instance.pool.Release(this);
            }
            else
            {
                // TODO : Update Hp World UI
            }
        }
    }
}

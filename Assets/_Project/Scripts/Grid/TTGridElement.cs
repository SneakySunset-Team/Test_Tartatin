using Lean.Touch;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TTGridElement : MonoBehaviour
{
    public Action OnHideEvent;
    public Action OnShowEvent;
    
    [SerializeField, ReadOnly, HideInEditorMode]
    public List<TTCell> cells = new List<TTCell>();
    
    [SerializeField]
    int _baseHp;

    [SerializeField, HideInEditorMode]
    int _currentHp;
    
    [SerializeField, HideInPlayMode]
    SpriteRenderer _spriteRenderer;

    [SerializeField, HideInPlayMode]
    Image _hpBarFill;

    [SerializeField]
    FMODUnity.EventReference _fmodOnHit, _fmodOnDestroy;
    
    
    
    void OnEnable()
    {
        _currentHp = _baseHp;
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        _hpBarFill.fillAmount = 1;
        if (_currentHp <= 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot(_fmodOnDestroy);
            ClearCells();
            STTRunManager.Instance.pool.Release(this);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(_fmodOnHit);
            _hpBarFill.fillAmount = (float)_currentHp / _baseHp;
        }
    }

    public void ClearCells()
    {
        cells.ForEach(cell => cell.ClearGridElement()); 
        cells.Clear();
    }
    
    public Sprite GetSprite() => _spriteRenderer.sprite;
    
    public Color GetColor() => _spriteRenderer.color;

    public void Hide()
    {
        _hpBarFill.transform.parent.gameObject.SetActive(false);
        _spriteRenderer.enabled = false;
        OnHideEvent?.Invoke();
    }

    public void Show()
    {
        _hpBarFill.transform.parent.gameObject.SetActive(true);
        _spriteRenderer.enabled = true;
        OnShowEvent?.Invoke();
    }
}

using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class TTEnnemyBehaviour : MonoBehaviour
{
    public Action<TTEnnemyBehaviour> OnDisableEvent;
    
    [SerializeField] float _moveSpeed;
    [field : SerializeField] public int damage { get; private set; } = 3;
    [field : SerializeField, HideInPlayMode] private int _startHpNumber = 7;
    [field : SerializeField, HideInEditorMode] public int hpNumber { get; private set; }
    [SerializeField, HideInPlayMode] private Image _hpBarFill;
    [SerializeField] FMODUnity.EventReference _fmodDamage, _fmodDestroy;
    Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        hpNumber = _startHpNumber;
        _hpBarFill.fillAmount = 1;
        OnDisableEvent = null;
    }

    void OnDisable()
    {
        OnDisableEvent?.Invoke(this);
        transform.position = Vector2.zero;
    }
    
    void FixedUpdate()
    {
        _rb.linearVelocityY = - _moveSpeed * Time.fixedDeltaTime;
    }

    public void TakeDamage(int damage)
    {
        hpNumber -= damage;
        _hpBarFill.fillAmount = (float)hpNumber / _startHpNumber;
        if (hpNumber <= 0)
        {
            STTRunManager.Instance.runEconomyManager.AddDollars(1);
            STTRunManager.Instance.pool.Release(this);
            FMODUnity.RuntimeManager.PlayOneShot(_fmodDestroy);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(_fmodDamage);
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out TTGridElement element))
        {
            element.TakeDamage(damage);
            STTRunManager.Instance.pool.Release(this);
        }
    }
}

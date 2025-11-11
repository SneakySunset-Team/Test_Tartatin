using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class TTEnnemyBehaviour : MonoBehaviour
{
    public Action<TTEnnemyBehaviour> OnDisableEvent;
    
    Rigidbody2D _rb;
    [SerializeField] float _moveSpeed;
    [field : SerializeField] public int damage { get; private set; } = 3;
    [field : SerializeField, HideInPlayMode] private int _startHpNumber = 7;
    [field : SerializeField, HideInEditorMode] public int hpNumber { get; private set; }
    [SerializeField, HideInPlayMode] private Image _hpBarFill;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        hpNumber = _startHpNumber;
        _hpBarFill.fillAmount = 1;
    }

    void OnDisable()
    {
        OnDisableEvent?.Invoke(this);
    }
    
    void FixedUpdate()
    {
        _rb.linearVelocityY = - _moveSpeed * Time.fixedDeltaTime;
    }

    public void TakeDamage(int damage)
    {
        hpNumber -= damage;
        _hpBarFill.fillAmount = (float)hpNumber / (float)_startHpNumber;
        if (hpNumber <= 0)
        {
            TTRunManager.Instance.pool.Release(this);
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<TTGridElement>(out TTGridElement element))
        {
            element.TakeDamage(damage);
            TTRunManager.Instance.pool.Release(this);
        }
    }
}

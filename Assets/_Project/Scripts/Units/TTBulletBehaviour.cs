using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class TTBulletBehaviour : MonoBehaviour
{
    public TTEnnemyBehaviour ennemy { get; private set; }
    
    [field : ReadOnly] int _damage;
    [SerializeField] float _speed;
    [SerializeField] float _lifeTime;
    
    Rigidbody2D _rb;

    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        StartCoroutine(LifeCoroutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        ennemy = null;
    }

    public void Initialize(TTEnnemyBehaviour ennemy, int damage)
    {
        this.ennemy = ennemy;
        _damage = damage;
    }

    void FixedUpdate()
    {
        if (ennemy && ennemy.gameObject.activeInHierarchy)
        {
            transform.LookAt(ennemy.transform);
            transform.up = ennemy.transform.position - transform.position;
        }
        else if(ennemy)
        {
            ennemy = null;
        }
        _rb.linearVelocity = transform.up * (_speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ennemy" && other.gameObject.TryGetComponent<TTEnnemyBehaviour>(out var ennemy))
        {
            ennemy.TakeDamage(_damage);
            STTRunManager.Instance.pool.Release(this);
        }
    }

    IEnumerator LifeCoroutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        STTRunManager.Instance.pool.Release(this);
    }
}

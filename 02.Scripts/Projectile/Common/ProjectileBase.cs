using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class ProjectileBase : MonoBehaviour
{
    public enum E_ProjectileAttribute { Fire, Thunder }
    
    [SerializeField] protected ProjectileStats stats;

    protected Rigidbody rb;
    protected Transform shooter;
    [SerializeField] protected Transform target;
    protected bool fired;
    
    protected float currLifeTime;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected void Start()
    {
        currLifeTime = 0;
    }

    private void FixedUpdate()
    {
        OnFire();
    }

    protected abstract void OnFire();

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!fired) return;

        if (shooter && other.transform == shooter) return; // 자기충돌 방지

        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamagable>()?.TakeDamage(stats.damage);
            OnHit();
        }
        else if (other.CompareTag("Monster"))
        {
            other.GetComponent<EnemyController>()?.TakeDamage(stats.damage);
            OnHit();
        }
    }

    protected virtual void OnHit()
    {
        Despawn();
    }

    protected void Despawn()
    {
        fired = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Factory.Instance.ReturnObject(stats.poolKey, gameObject);
    }
}
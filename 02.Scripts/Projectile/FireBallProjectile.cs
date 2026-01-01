using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FireBallProjectile : ArcProjectile
{
    private readonly string key = "Fire_Explosion";

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if(other.gameObject.CompareTag("Player"))
        {
            GameObject effect = Factory.Instance.GetObject(key);
            effect.transform.position = other.transform.position + Vector3.up * 0.7f;

            DOVirtual.DelayedCall(1.5f, () => Factory.Instance.ReturnObject(key, effect));
        }
    }
}

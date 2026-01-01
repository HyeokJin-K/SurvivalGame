
using UnityEngine;

public class HitContext
{
    public Collider collider;
    public Vector3 hitPoint;
    public float damage;
    public AudioClip hitSFX;
    public ParticleSystem hitParticle;

    public void Init(Collider collider, Vector3 hitPoint, float damage, AudioClip hitSFX = null, ParticleSystem hitParticle = null)
    {
        this.collider = collider;
        this.hitPoint = hitPoint;
        this.damage = damage;
        this.hitSFX = hitSFX;
        this.hitParticle = hitParticle;
    }
}


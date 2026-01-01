using System.Collections;
using UnityEngine;

public abstract class AttackDataBase : ScriptableObject
{
    public int attackId;
    public float motionAttackPowerRate = 1f;
    
    public AudioClip attackAudioClip;
    public AudioClip hitAudioClip;
    public ParticleSystem hitParticle;
}

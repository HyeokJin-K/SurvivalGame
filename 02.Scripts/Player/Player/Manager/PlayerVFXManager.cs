using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerVFXManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerVFXPool playerVFXPool;
    public static PlayerVFXManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        if(!player) player = GetComponent<Player>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="seconds">해당 시간이 지난 후 파티클 출력 (default: 0초)</param>
    public void PlayParticleAtPlayer(ParticleSystem particle, float seconds = 0f)
    {
        if (seconds == 0f)
        {
            var ps = playerVFXPool.Get(particle);
            ps.transform.position = player.transform.position;
            ps.Play();
            return;
        }

        StartCoroutine(PlayParticleAtPlayerCoroutine());
        IEnumerator PlayParticleAtPlayerCoroutine()
        {
            yield return new WaitForSeconds(seconds);
            var ps = playerVFXPool.Get(particle);
            ps.transform.position = player.transform.position;
            ps.transform.rotation = player.transform.rotation;
            ps.Play();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="seconds">해당 시간이 지난 후 파티클 출력 (default: 0초)</param>
    public void PlayParticle(ParticleSystem particle, Vector3 position, Quaternion rotation, float seconds = 0f)
    {
        if (seconds == 0f)
        {
            var ps = playerVFXPool.Get(particle);
            ps.transform.position = position;
            ps.transform.rotation = rotation;
            ps.Play();
            return;
        }

        StartCoroutine(PlayParticleCoroutine());
        IEnumerator PlayParticleCoroutine()
        {
            yield return new WaitForSeconds(seconds);
            var ps = playerVFXPool.Get(particle);
            ps.transform.position = position;
            ps.transform.rotation = rotation;
            ps.Play();
        }
    }
}
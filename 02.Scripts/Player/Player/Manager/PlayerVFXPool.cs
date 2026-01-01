
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXPool : MonoBehaviour
{
    private Dictionary<ParticleSystem, Queue<ParticleSystem>> _playerVFXpools = new Dictionary<ParticleSystem, Queue<ParticleSystem>>();
    private GameObject _vfxPools;
    private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(2f);
    public ParticleSystem Get(ParticleSystem particleSystem)
    {
        if (!_playerVFXpools.ContainsKey(particleSystem))
        {
            _playerVFXpools[particleSystem] = new Queue<ParticleSystem>();

            GameObject psPool = new GameObject($"PlayerVFXPool {particleSystem.name}");
            if (!_vfxPools) _vfxPools = new GameObject("PlayerVFXPools");
            psPool.transform.SetParent(_vfxPools.transform);
            
            for (var i = 0; i < 10; i++)
            {
                ParticleSystem instance = Instantiate(particleSystem, psPool.transform);
                instance.gameObject.SetActive(false);
                _playerVFXpools[particleSystem].Enqueue(instance);
            }
        }
        
        Queue<ParticleSystem> pool = _playerVFXpools[particleSystem];
        ParticleSystem ps;

        if (pool.Count > 0)
        {
            ps = pool.Dequeue();
        }
        else
        {
            ps = Instantiate(particleSystem, transform);
        }

        ps.gameObject.SetActive(true);
        ps.transform.position = transform.position;
        
        StartCoroutine(ReturnToPool(ps, pool));
        return ps;
    }

    private IEnumerator ReturnToPool(ParticleSystem ps, Queue<ParticleSystem> pool)
    {
        yield return _waitForSeconds;
        ps.Stop();
        ps.gameObject.SetActive(false);
        pool.Enqueue(ps);
    }
}

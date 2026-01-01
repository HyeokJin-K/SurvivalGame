using UnityEngine;

public class EnemyAttackVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem swingTrailPrefab;
    [SerializeField] private ParticleSystem hitPrefab;

    public void PlaySwing()
    {
        if (!swingTrailPrefab) return;
        Debug.Log("PlaySwing");
        
        swingTrailPrefab.gameObject.SetActive(true);
    }

    public void PlayHit(Vector3 hitPosition)
    {
        if (!hitPrefab) return;

        swingTrailPrefab.gameObject.SetActive(false);
        var vfx = Instantiate(hitPrefab, hitPosition, Quaternion.identity);
        vfx.Play();
    }
}
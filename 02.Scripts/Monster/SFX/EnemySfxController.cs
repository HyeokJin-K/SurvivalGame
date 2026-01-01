using UnityEngine;

public enum EnemySfxType
{
    Attack,
    Hit,
}

public class EnemySfxController : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] hitClips;

    [Header("Options")]
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    [SerializeField] private bool randomPitch = true;
    [Range(0f, 0.3f)]
    [SerializeField] private float pitchRange = 0.1f;

    private void Awake()
    {
        if (!sfxSource)
        {
            sfxSource = GetComponent<AudioSource>();
            if (!sfxSource)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
            }
        }

        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 1f; // 3D 사운드
    }

    public void PlayAttack()
    {
        Play(EnemySfxType.Attack);
    }

    public void PlayHit()
    {
        Play(EnemySfxType.Hit);
    }

    public void Play(EnemySfxType type)
    {
        var clip = GetRandomClip(type);
        if (!clip || !sfxSource) return;

        if (randomPitch)
        {
            sfxSource.pitch = 1f + Random.Range(-pitchRange, pitchRange);
        }
        else
        {
            sfxSource.pitch = 1f;
        }

        sfxSource.PlayOneShot(clip, volume);
    }

    private AudioClip GetRandomClip(EnemySfxType type)
    {
        AudioClip[] clips = null;

        switch (type)
        {
            case EnemySfxType.Attack:
                clips = attackClips;
                break;
            case EnemySfxType.Hit:
                clips = hitClips;
                break;
        }

        if (clips == null || clips.Length == 0) return null;
        int index = Random.Range(0, clips.Length);
        return clips[index];
    }
}

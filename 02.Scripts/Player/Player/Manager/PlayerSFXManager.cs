using System;
using System.Collections;
using UnityEngine;

public class PlayerSFXManager : MonoBehaviour
{
    [SerializeField] Player player;
    public static PlayerSFXManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    public AudioSource AudioSource => audioSource;

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
    /// <param name="seconds">해당 시간이 지난 후 오디오 출력 (default: 0초)</param>
    public void PlaySFXOneShot(AudioClip clip, float seconds = 0f)
    {
        if (seconds == 0f)
        {
            audioSource.PlayOneShot(clip);
            return;
        }

        StartCoroutine(PlaySFXOneShotWaitForSeconds());
        IEnumerator PlaySFXOneShotWaitForSeconds()
        {
            yield return new WaitForSeconds(seconds);
            audioSource.PlayOneShot(clip);
        }
    }
    
}

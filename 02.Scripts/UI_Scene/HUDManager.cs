using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] private Slider hpBar;
    [SerializeField] private RectTransform hpBarRect;
    [SerializeField] private float hpLerpDuration = 0.3f;
    [SerializeField] private float baseWidth = 200f;
    [SerializeField] private GameObject MiniMap;
    [SerializeField] private GameObject BigMap;

    [SerializeField] Volume globalVolume;


    Vignette vignette;

    private Coroutine _hpBarLerpCoroutine;

    // 음식 버프 표시

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        globalVolume.profile.TryGet(out vignette);
        vignette.intensity.value = 0f;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            MiniMap.SetActive(!MiniMap.activeSelf);
            BigMap.SetActive(!BigMap.activeSelf);
        }
    }

    public void HealthBarUpdate(float currentHealth, float maxHealth)
    {
        if (_hpBarLerpCoroutine != null) StopCoroutine(_hpBarLerpCoroutine);
        _hpBarLerpCoroutine = StartCoroutine(HPBarLerp(currentHealth, maxHealth));
    }

    private IEnumerator HPBarLerp(float currentHealth, float maxHealth)
    {
        float targetRatio = currentHealth / maxHealth;
        float startRatio = hpBar.value;
        float elapsed = 0f;

        float newwidth = baseWidth * (maxHealth / 100f);
        hpBarRect.sizeDelta = new Vector2(newwidth, hpBarRect.sizeDelta.y);

        while (elapsed < hpLerpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / hpLerpDuration);

            float ratio = Mathf.Lerp(startRatio, targetRatio, t);
            hpBar.value = ratio;

            if (ratio <= 0.1f)
            {
                float vignetteT = Mathf.InverseLerp(0.1f, 0f, ratio);
                vignette.intensity.value = Mathf.Lerp(0.25f, 0.35f, vignetteT);
            }
            else
            {
                vignette.intensity.value = 0f;
            }

            yield return null;
        }

        hpBar.value = targetRatio;

        if (targetRatio <= 0.1f)
        {
            float vignetteT = Mathf.InverseLerp(0.1f, 0f, targetRatio);
            vignette.intensity.value = Mathf.Lerp(0.25f, 0.35f, vignetteT);
        }
        else
        {
            vignette.intensity.value = 0f;
        }
    }
}
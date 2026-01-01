using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class FadeOut : MonoBehaviour
{
    public GameObject panelObject;
    public Image panel;
    public float fadeDuration = 1f;

    public void StartFadeOut(Action onComplete = null)
    {
        panelObject.SetActive(true);
        StartCoroutine(FadeOutCoroutine(onComplete));
    }

    IEnumerator FadeOutCoroutine(Action onComplete)
    {
        float elapsedTime = 0f;
        Color color = panel.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            panel.color = color;
            yield return null;
        }

        onComplete?.Invoke();
    }
}

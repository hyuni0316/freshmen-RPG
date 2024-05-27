using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeIn : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public float fadeDuration = 2.0f;
    private float currentTime = 0.0f;

    void Start()
    {
        if (startText != null)
        {
            Color color = startText.color;
            color.a = 0;
            startText.color = color;
        }
    }

    void Update()
    {
        if (startText != null && currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(currentTime / fadeDuration);
            Color color = startText.color;
            color.a = alpha;
            startText.color = color;
        }
    }
}


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed;
    
    private Coroutine coroutine;

    private void Start()
    {
        CharacterManager.Instance.Player.playerCondition.onTakeDamage += Flash;
    }

    public void Flash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255);
        coroutine = StartCoroutine(FadeWay());
    }

    private IEnumerator FadeWay()
    {
        float startAlpha = 0.3f;
        float a= startAlpha;

        while (a > 0)
        {
            a -= (startAlpha / flashSpeed * Time.deltaTime);
            image.color = new Color(1f, 100f / 255f, 100f / 255, a);
            yield return null;
        }   
        
        image.enabled = false;
    }
}

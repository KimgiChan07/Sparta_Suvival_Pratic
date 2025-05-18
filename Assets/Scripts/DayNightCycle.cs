using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 0.1f)] 
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    public float timeRate;
    public Vector3 noon; // Vector 90 0 0
    
    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;
    
    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;
    
    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;
        
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);
        
        RenderSettings.ambientIntensity= lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    void UpdateLighting(Light _lightSoeurce, Gradient _gradient, AnimationCurve _intensityCCurve)
    {
        float intensity = _intensityCCurve.Evaluate(time);

        _lightSoeurce.transform.eulerAngles = noon * ((time - (_lightSoeurce == sun ? 0.25f : 0.75f)) * 4f);
        _lightSoeurce.color= _gradient.Evaluate(time);
        _lightSoeurce.intensity = intensity;

        GameObject go = _lightSoeurce.gameObject;
        if (_lightSoeurce.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if(_lightSoeurce.intensity>0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue; //현재 상태 
    public float startValue; //상태의 시작 값
    public float maxValue; //상태의 최댓값
    public float passiveValue;//변화 시켜주는 값

    public Image uiBar;
    
    void Start()
    {
        curValue = startValue;
    }

    void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return curValue/maxValue;
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue+value,maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}

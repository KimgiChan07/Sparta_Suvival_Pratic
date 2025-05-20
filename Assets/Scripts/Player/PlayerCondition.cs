using System;
using UnityEngine;

public interface IDamageIbe
{
    void TakePhysicalDamage(int damage);
}


public class PlayerCondition : MonoBehaviour,  IDamageIbe
{
    public UIConditoin uIConditoin;
    
    public Condition health{get{return uIConditoin.health;}}
    public Condition hunger{get{return uIConditoin.hunger;}}
    public Condition stamina{get{return uIConditoin.stamina;}}

    public float noHungerHealtDecay;
    private IDamageIbe _damageIbeImplementation;
    
    public event Action onTakeDamage;

    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealtDecay * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount); 
    }

    public void Die()
    {
        Debug.Log("die!!");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }
}

using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UIConditoin uIConditoin;
 
    
    public Condition health{get{return uIConditoin.health;}}
    public Condition hunger{get{return uIConditoin.hunger;}}
    public Condition stamina{get{return uIConditoin.stamina;}}

    public float noHungerHealtDecay;
    
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
    
}

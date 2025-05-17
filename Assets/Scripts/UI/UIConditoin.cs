using UnityEngine;

public class UIConditoin : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;


    private void Start()
    {
        CharacterManager.Instance.Player.playerCondition.uIConditoin=this;
    }

    void Update()
    {
        
    }
}

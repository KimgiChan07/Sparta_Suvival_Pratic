using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController  playerController;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        playerController = GetComponent<PlayerController>();
    }
    
}

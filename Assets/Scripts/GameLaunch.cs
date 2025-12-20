
using UnityEngine;

public class GameLaunch : MonoBehaviour
{
    public PlayerInputController playerInputController;

    void Start()
    {
        var playerGo = GameObject.Find("Player");
        var actor = playerGo.GetComponent<Actor>();
        playerInputController.SetControllingActor(actor);
    }
}
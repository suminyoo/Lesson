using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Player player;
    public UIManager mainUI;


    public void Start()
    {
        Player.OnPlayerCollisionEvent += playerCollision;
        Player.OnPlayerCollisionEventWithObj += playerCollisionObj;

        Player.OnPlayerTriggerEvent += playerTrigger;
        Player.OnPlayerTriggerEventWithObj += playerTriggerObj;
    }

    private void playerCollision()
    {
        //Debug.Log("Player Got Hit");
    }
    private void playerCollisionObj(GameObject obj)
    {
        Debug.Log("Player Got Hit by " + obj.gameObject.name);

        if (obj == null) { return; }

        if(obj.layer == 6) 
        {
            player.ChangePlayerHP(-10);
            mainUI.ChangePlayerHp();
        }

    }
    private void playerTrigger()
    {
        Debug.Log("Player Trigger");
    }
    private void playerTriggerObj(GameObject obj)
    {
        Debug.Log("Player Triggered " + obj.gameObject.name);
        if (obj == null) { return; }

        if (obj.layer == 7)
        {
            if (obj.tag == "Life")
            {
                player.ChangePlayerHP(10);
                mainUI.ChangePlayerHp();
                obj.SetActive(false);
            }
        }
    }
}

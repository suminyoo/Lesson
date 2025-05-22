using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public Player player;
    public InGameUI inGameUIDoc;

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
            inGameUIDoc.ChangePlayerHP(player.hp);
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
                inGameUIDoc.ChangePlayerHP(player.hp);
                obj.SetActive(false);
            }
        }
    }
}

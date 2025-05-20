using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void Start()
    {
        Player.OnPlayerHitSomethingEvent += playerHit;
        Player.OnPlayerHitSomethingEventWithObj += playerHitObj;

    }

    private void playerHit()
    {
        Debug.Log("Player Got Hit");
    }
    private void playerHitObj(GameObject obj)
    {
        Debug.Log("Player Got Hit by " + obj.gameObject.name);
    }
}

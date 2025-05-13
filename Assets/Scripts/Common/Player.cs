using UnityEngine;

public class Enemy
{
    public void KillPlayer()
    {
        EventBus<TestEvent>.Raise(new TestEvent());
        EventBus<PlayerEvent>.Raise(new PlayerEvent
        {
            health = 0,
            mana = 0,
        });

    }


}
public class Player : MonoBehaviour
{
    EventBinding<TestEvent> testEventBinding;
    EventBinding<PlayerEvent> playerEventBinding;

    private void OnEnable()
    {
        testEventBinding = new EventBinding<TestEvent>(HandleTestEvent);
        EventBus<TestEvent>.Register(testEventBinding);

        playerEventBinding = new EventBinding<PlayerEvent>(HandlePlayerEvent);
        EventBus<PlayerEvent>.Register(playerEventBinding);
    }
    private void HandleTestEvent(TestEvent @event)
    {
        Debug.Log("Test event" + @event);

    }

    private void HandlePlayerEvent(PlayerEvent @event)
    {
        Debug.Log("Handle Player Event @event.health" + @event.health);
        Debug.Log("Handle Player Event @event.mana" + @event.mana);
    }



    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            EventBus<TestEvent>.Raise(new TestEvent());
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            EventBus<PlayerEvent>.Raise(new PlayerEvent
            {
                health = 0,
                mana = 0,
            });

        }
    }

}


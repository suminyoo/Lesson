using OpenCover.Framework.Model;
using UnityEngine;



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
    private void OnDisable()
    {
        EventBus<TestEvent>.Deregister(testEventBinding);
        EventBus<PlayerEvent>.Deregister(playerEventBinding);
    }
    private void HandleTestEvent(TestEvent @testEvent)
    {
        Debug.Log("Test event" + @testEvent);
    }

    private void HandlePlayerEvent(PlayerEvent @playerEvent)
    {
        Debug.Log("Handle Player Event @event.health" + @playerEvent.health);
        Debug.Log("Handle Player Event @event.mana" + @playerEvent.mana);
        @playerEvent.myAction();
        @playerEvent.myFunc(5);

    }

    void MyMethod()
    {
        Debug.Log("My Action");
    }
    public static int MyMethodWithParam(int number)
    {
        Debug.Log("MyMethodWithParam " + number);
        return number * number;
    }



    void Update() 
    {
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    EventBus<TestEvent>.Raise(new TestEvent());
        //}

        if (Input.GetKeyDown(KeyCode.X))
        {
            EventBus<PlayerEvent>.Raise(new PlayerEvent
            {
                health = 100,
                mana = 20,
                myAction = MyMethod,
                myFunc = MyMethodWithParam,

            });

        }
    }

}


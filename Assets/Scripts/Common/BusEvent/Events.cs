using System;

public interface IEvent { }

public struct TestEvent : IEvent { }

public struct PlayerEvent : IEvent
{
    public int health;
    public int mana;
    public Action myAction; //void and no param
    public Func<int, int> myFunc;

}
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomChaseEvent : UnityEvent<int>
{
}

public class ChaseEventListener : MonoBehaviour
{
    public ChaseEvent Event;
    public CustomChaseEvent Response;
    
    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(int spotter)
    {
        Response.Invoke(spotter);
    }
}
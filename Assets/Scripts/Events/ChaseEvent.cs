using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChaseEvent", menuName = "ScriptableObjects/ChaseEvent", order = 4)]
public class ChaseEvent : ScriptableObject
{
    private readonly List<ChaseEventListener> eventListeners = 
        new List<ChaseEventListener>();

    public void Raise(int spotter)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(spotter);
    }

    public void RegisterListener(ChaseEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(ChaseEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
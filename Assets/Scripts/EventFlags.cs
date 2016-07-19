using UnityEngine;
using System.Collections;

[System.Serializable]
public class EventFlags
{
    public bool kitchenDoorOpen;
    public EventTrigger.Events eventType;

    public EventFlags()
    {
        kitchenDoorOpen = false;
        eventType = EventTrigger.Events.OPEN_KITCHEN_DOOR;
    }
}

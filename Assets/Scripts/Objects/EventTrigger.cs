using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour
{
    public EventManager.Events eventAction = EventManager.Events.OPEN_KITCHEN_DOOR;

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.parent.tag == "Player")
        {
            EventManager.current.TriggerEvent(eventAction);
        }
    }
}

using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour
{
    public enum Events
    {
        OPEN_KITCHEN_DOOR
    }
    public Events eventAction = Events.OPEN_KITCHEN_DOOR;

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.parent.tag == "Player")
        {
            if (eventAction == Events.OPEN_KITCHEN_DOOR)
            {
                EventManager.current.OpenKitchenDoor(gameObject);
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour
{
    public CharacterBehaviour.Type eventType = CharacterBehaviour.Type.PASSIVE;

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.parent.tag == "Player")
        {
            EventManager.current.TriggerEvent(eventType);
            gameObject.SetActive(false);
        }
    }
}

using UnityEngine;
using System.Collections;

public class HideBehaviour : MonoBehaviour {

    public PlayerController player;
    public GameObject QuestionMark;
    public bool isHiding;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.ActivateSelection(PlayerController.Selection.HIDEOBJECT);
            player.isOverlappingHideObject = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.selection = PlayerController.Selection.HIDEOBJECT;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        player.DeactivateSelection();
        player.isOverlappingHideObject = false;
    }
}

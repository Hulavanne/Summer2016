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
            player.isSelectionActive = true;
            QuestionMark.SetActive(true);
            player.selection = PlayerController.Selection.HIDEOBJECT;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {

            player.selection = PlayerController.Selection.HIDEOBJECT;

            }
    }

    void OnTriggerExit2D(Collider2D other)
    {
            player.isSelectionActive = false;
            QuestionMark.SetActive(false);
            player.selection = PlayerController.Selection.DEFAULT;
    }

	void Awake () {
	    
	}
	
	void Update () {
	
	}
}

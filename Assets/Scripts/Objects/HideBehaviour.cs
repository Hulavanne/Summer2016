using UnityEngine;
using System.Collections;

public class HideBehaviour : MonoBehaviour {

    public PlayerController Player;
    public GameObject QuestionMark;
    public bool isHiding;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.isSelectionActive = true;
            QuestionMark.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
            Player.isSelectionActive = false;
            QuestionMark.SetActive(false);
    }

	void Awake () {
	    
	}
	
	void Update () {
	
	}
}

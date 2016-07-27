using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HideBehaviour : MonoBehaviour {
    
    public GameObject QuestionMark;
    public bool isHiding;
    public Animator anim;
    public Sprite creviceSprite;

    void Awake()
    {
        anim = transform.FindChild("Sprite").gameObject.GetComponent<Animator>();
        creviceSprite = transform.FindChild("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite;
        if (transform.name == "Bush")
        {
            anim.SetBool("isHidden", false);
        }
    }
    
    void Update()
    {
        if (transform.name == "Bush")
        {
            if (PlayerController.current.isHidden)
            {
                anim.SetBool("isHidden", true);
            }

            if (PlayerController.current.isUnhiding)
            {
                anim.SetBool("isHidden", false);
            }

            
        }
        else if (transform.name == "Crevice")
        {
            if (PlayerController.current.isHidden)
            {
                transform.FindChild("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = creviceSprite;
            }
            else
            {
                transform.FindChild("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!PlayerController.current.isHidden)
            {
                PlayerController.current.ActivateSelection(PlayerController.Selection.HIDE_OBJECT);
            }
            PlayerController.current.isOverlappingHideObject = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.current.selection = PlayerController.Selection.HIDE_OBJECT;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerController.current.DeactivateSelection();
        PlayerController.current.isOverlappingHideObject = false;
    }
}

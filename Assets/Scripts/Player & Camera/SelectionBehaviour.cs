using UnityEngine;
using System.Collections;

public class SelectionBehaviour : MonoBehaviour {
    public Camera mainCamera;
    public static SelectionBehaviour current;
    public bool hasSelected;

    public Sprite selectionDoor;
    public Sprite selectionHideObject;
    public Sprite selectionNPC;

    public SpriteRenderer thisRenderer;
    public PlayerController player;

    public Vector3 mousePos;
    public MenuController menu;

    void Awake () {
        current = this;
        player = transform.parent.transform.GetComponent<PlayerController>();
        thisRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        menu = GameObject.Find("InGameUI").GetComponent<MenuController>();
    }
	
    void DetectMouse()
    {

        mousePos = new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x,
            mainCamera.ScreenToWorldPoint(Input.mousePosition).y, 0);

        if (mousePos.x > transform.position.x - 1 &&
            mousePos.x < transform.position.x + 1 &&
            mousePos.y > transform.position.y - 1 &&
            mousePos.y < transform.position.y + 1)
        {
            menu.ActionButton();
            menu.PlayButtonSoundEffect();
            hasSelected = true;
        }
    }

	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            DetectMouse();
        }

        if (player.selection == PlayerController.Selection.DOOR)
        {
            thisRenderer.sprite = selectionDoor;
        }

        if (player.selection == PlayerController.Selection.HIDE_OBJECT)
        {
            thisRenderer.sprite = selectionHideObject;
        }

        if (player.selection == PlayerController.Selection.NPC)
        {
            thisRenderer.sprite = selectionNPC;
        }
    }
}
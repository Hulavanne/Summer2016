using UnityEngine;
using System.Collections;

public class SelectionBehaviour : MonoBehaviour {
    public Camera mainCamera;
    public static SelectionBehaviour current;

    public Sprite selectionDoor;
    public Sprite selectionHideObject;
    public Sprite selectionNPC;

    public SpriteRenderer thisRenderer;
    public PlayerController player;

    void Awake () {
        current = this;
        player = transform.parent.transform.GetComponent<PlayerController>();
        thisRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

	void Update () {

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
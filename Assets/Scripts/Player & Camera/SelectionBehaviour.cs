﻿using UnityEngine;
using System.Collections;

public class SelectionBehaviour : MonoBehaviour {

    public Sprite selectionDoor;
    public Sprite selectionHideObject;
    public Sprite selectionNPC;

    public SpriteRenderer thisRenderer;

    public PlayerController player;

    void Start () {
        player = transform.parent.gameObject.GetComponent<PlayerController>();
        thisRenderer = GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        if (player.selection == PlayerController.Selection.DOOR)
        {
            thisRenderer.sprite = selectionDoor;
        }

        if (player.selection == PlayerController.Selection.HIDEOBJECT)
        {
            thisRenderer.sprite = selectionHideObject;
        }

        if (player.selection == PlayerController.Selection.NPC)
        {
            thisRenderer.sprite = selectionNPC;
        }
    }
}
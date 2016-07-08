using UnityEngine;
using System.Collections;

public class GameFlowManager : MonoBehaviour {

    public string npcName;
    public PlayerController player;
    public ActivateTextAtLine textActivate;
    public TextBoxManager textBoxManager;
    public CameraFollowAndEffects cameraEffects;

    public GameObject tempGameObject;

    public bool isIntro = true;
    public bool destroyNPC;

    public bool isNPCAutomatic;
    public int npcIntroBehav = 0;

    public void DestroyNPC()
    {
        if (destroyNPC)
        {
            GameObject npcBeingDestroyed;
            npcBeingDestroyed = GameObject.Find(npcName);
            Destroy(npcBeingDestroyed);
            destroyNPC = false;
        }
    }

    //this function only gets called after speaking with an NPC
    public void ChangeNPCBehaviour()
    {
        if (npcName == "Intro_NPC")
        {
            if (npcIntroBehav == 0)
            {
                player.DeactivateSelection();
                textActivate.npcBehav.textStartLine = 3;
                textActivate.npcBehav.textEndLine = 4;
                player.npcWaitTime = 2.0f;
                isNPCAutomatic = true;
                player.isIntro = false;
                player.canMove = true;
                npcIntroBehav++;
            }
            else if (npcIntroBehav == 1)
            {
                player.DeactivateSelection();
                destroyNPC = true;
                tempGameObject.SetActive(true);
            }
        }

        DestroyNPC(); // checks and destroys the npc if the bool is true
    }

	void Awake () {
        npcName = "";
        textActivate = GameObject.Find("ActivateText").GetComponent<ActivateTextAtLine>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        textBoxManager = GameObject.Find("InGameUI").GetComponent<TextBoxManager>();
        cameraEffects = GameObject.Find("MainCamera").GetComponent<CameraFollowAndEffects>();
	}
	
	void Update () {
        npcName = player.NPCName;
	}
}

using UnityEngine;
using System.Collections;

public class GameFlowManager : MonoBehaviour {

    public string npcName;
    public PlayerController player;
    public ActivateTextAtLine textActivate;
    public TextBoxManager textBoxManager;
    public TextList textNumber;
    public CameraFollowAndEffects cameraEffects;

    public GameObject tempGameObject;

    public bool isIntro = true;
    public bool destroyNPC;

    public bool isNPCAutomatic;
    public int npcIntroBehav = 0;
    public int npc4Behav = 0;
    public int npc5Behav = 0;

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
                textNumber.textIntroStartLine = 3;
                textNumber.textIntroEndLine = 4;
                player.npcWaitTime = 2.0f;
                isNPCAutomatic = true;
                player.isIntro = false;
                player.canMove = true;
                npcIntroBehav++;
            }
            else if (npcIntroBehav == 1)
            {
                player.selection = PlayerController.Selection.DEFAULT;
                player.questionMark.SetActive(false);
                destroyNPC = true;
                tempGameObject.SetActive(true);
            }
        }

        DestroyNPC(); // checks and destroys the npc if the bool is true
    }

	void Awake () {
        if (isIntro)
        {
            //darkScreen = true;
        }
        
        npcName = "";
        textActivate = GameObject.Find("ActivateText").GetComponent<ActivateTextAtLine>();
        textNumber = GameObject.Find("InGameUI").GetComponent<TextList>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        textBoxManager = GameObject.Find("InGameUI").GetComponent<TextBoxManager>();
        cameraEffects = GameObject.Find("MainCamera").GetComponent<CameraFollowAndEffects>();
	}
	
	void Update () {
        npcName = player.NPCName;
	}
}

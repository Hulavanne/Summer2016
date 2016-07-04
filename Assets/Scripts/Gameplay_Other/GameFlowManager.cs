using UnityEngine;
using System.Collections;

public class GameFlowManager : MonoBehaviour {

    public string npcName;
    public PlayerController player;
    public ActivateTextAtLine textActivate;
    public TextBoxManager textBoxManager;
    public TextList textNumber;
    public CameraFollowAndEffects cameraEffects;

    public bool isIntro = true;
    public bool destroyNPC;

    public bool isNPCAutomatic;
    public int npcIntroBehav = 0;
    public int npc1Behav = 0;

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

    public void ChangeNPCBehaviour()
    {
        if (npcName == "Intro_NPC")
        {
            if (npcIntroBehav == 0)
            {
                //darkScreen = false;
                textNumber.text1StartLine = 3;
                textNumber.text1EndLine = 4;
                npcIntroBehav++;
            }
            else if (npcIntroBehav == 1)
            {
                destroyNPC = true;
                npcIntroBehav++;
            }
            
        }

        else if (npcName == "NPC_1")
        {
            if (npc1Behav == 0)
            {
                textNumber.text1StartLine = 3;
                textNumber.text1EndLine = 4;
                npc1Behav++;
                isNPCAutomatic = true;
                Debug.Log("working0");
            }
            else if (npc1Behav == 1)
            {
                destroyNPC = true;
                npc1Behav++;
            }
        }

        DestroyNPC(); // destroys the npc if the bool is true
    }

	void Awake () {
        if (isIntro)
        {
            //darkScreen = true;
        }

        isNPCAutomatic = true;
        npcName = "";
        textActivate = GameObject.Find("ActivateText").GetComponent<ActivateTextAtLine>();
        textNumber = GameObject.Find("TextList").GetComponent<TextList>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        textBoxManager = GameObject.Find("TextBoxManager").GetComponent<TextBoxManager>();
        cameraEffects = GameObject.Find("MainCamera").GetComponent<CameraFollowAndEffects>();
	}
	
	void Update () {
        npcName = player.NPCName;
	}
}

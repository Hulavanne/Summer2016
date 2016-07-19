using UnityEngine;
using System.Collections;

public class GameFlowManager : MonoBehaviour
{   
    public static GameFlowManager current;

    public string npcName;
    public NpcBehaviour npcBehav;
    public PlayerController player;
    public ActivateTextAtLine textActivate;
    public TextBoxManager textBoxManager;
    public CameraFollowAndEffects cameraEffects;

    public bool isIntro = true;
    public bool destroyNPC;
    public bool isNPCAutomatic;
    public int npcIntroBehav = 0;

    void Awake()
    {
        current = this;
        npcName = "";
        textActivate = GameObject.Find("ActivateText").GetComponent<ActivateTextAtLine>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        textBoxManager = GameObject.Find("InGameUI").GetComponent<TextBoxManager>();
        cameraEffects = GameObject.Find("MainCamera").GetComponent<CameraFollowAndEffects>();
    }

    void Update()
    {
        if (player.overlappingNpc != null)
        {
            npcName = player.overlappingNpc.name;
        }
    }

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

    public void ChangeLines(int startLine, int endLine)
    {
        player.DeactivateSelection();
        npcBehav.textStartLine = startLine;
        npcBehav.textEndLine = endLine;
        npcBehav.behaviour++;
    }

    //this function only gets called after speaking with an NPC
    public void ChangeNPCBehaviour()
    {
        if (npcName == "Intro_NPC")
        {
            npcBehav = GameObject.Find("Intro_NPC").GetComponent<NpcBehaviour>();
            if (npcBehav.behaviour == 0)
            {
                ChangeLines(3, 4);
                player.npcWaitTime = 2.0f;
                isNPCAutomatic = true;
                player.isIntro = false;
                player.canMove = true;
            }
            else if (npcBehav.behaviour == 1)
            {
                player.DeactivateSelection();
                destroyNPC = true;
            }
            npcBehav = null;
        }

        if (npcName == "NPC_Deer")
        {
            npcBehav = GameObject.Find("NPC_Deer").GetComponent<NpcBehaviour>();
            if (npcBehav.behaviour == 0)
            {
                ChangeLines(6, 7);
            }
            else if (npcBehav.behaviour == 1)
            {
                player.DeactivateSelection();
            }
            else if (npcBehav.behaviour == 2)
            {
                player.DeactivateSelection();
            }
            npcBehav = null;
        }

        if (npcName == "NPC_Lilies")
        {
            if (npcBehav.behaviour == 0)
            {
                ChangeLines(1, 1); 
                //spawn berries object
            }
            else if (npcBehav.behaviour == 1)
            {
                player.DeactivateSelection();
            }
        }
        DestroyNPC(); // checks and destroys the npc if the bool is true
    }
}

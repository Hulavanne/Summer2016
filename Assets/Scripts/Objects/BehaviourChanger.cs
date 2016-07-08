using UnityEngine;
using System.Collections;

public class BehaviourChanger : MonoBehaviour {

    public GameFlowManager gameFlow;
    public ActivateTextAtLine textActivate;
    public GameObject doorKitchen; // not activated, set this in inspector
    public GameObject doorKitchenNPC;
    public NpcBehaviour npcKitchen;

	void Start () {
        npcKitchen = GameObject.Find("NPC_Kitchen").GetComponent<NpcBehaviour>();
        gameFlow = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();
        textActivate = GameObject.Find("ActivateText").GetComponent<ActivateTextAtLine>();
        doorKitchenNPC = GameObject.Find("NPC_FrontDoor");
	}

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.parent.name == "Player" && transform.name == "NPC_Changer1")
        {
            npcKitchen.textStartLine = 1;
            npcKitchen.textEndLine = 1;

            Destroy(doorKitchenNPC);
            doorKitchen.SetActive(true);
            Destroy(gameObject);
        }
    }
}

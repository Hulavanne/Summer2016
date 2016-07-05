using UnityEngine;
using System.Collections;

public class BehaviourChanger : MonoBehaviour {

    public GameFlowManager gameFlow;
    public ActivateTextAtLine textActivate;
    public GameObject doorKitchen; // not activated, set this in inspector
    public GameObject doorKitchenNPC;
    public TextList textNumber;

	void Start () {
        gameFlow = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();
        textActivate = GameObject.Find("ActivateText").GetComponent<ActivateTextAtLine>();
        doorKitchenNPC = GameObject.Find("NPC_FrontDoor");
        textNumber = GameObject.Find("InGameUI").GetComponent<TextList>();
	}

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.parent.name == "Player")
        {
            textNumber.text4StartLine = 1;
            textNumber.text4EndLine = 1;
            gameFlow.npc4Behav = 1;

            textNumber.text5StartLine = 1;
            textNumber.text5EndLine = 1;
            gameFlow.npc5Behav = 1;

            Destroy(doorKitchenNPC);
            doorKitchen.SetActive(true);
            Destroy(gameObject);
        }
    }
}

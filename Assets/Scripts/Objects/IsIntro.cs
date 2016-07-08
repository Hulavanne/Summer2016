using UnityEngine;
using System.Collections;

public class IsIntro : MonoBehaviour {

    public GameFlowManager gameFlow;
    public CameraFollowAndEffects effects;

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.transform.parent.name == "Player")
        {
            gameFlow.player.isIntro = true;
            gameFlow.isIntro = true;
            gameFlow.isNPCAutomatic = true;
            effects.FadeToBlack();
        }
    }

	void Start ()
	{
        effects = GameObject.Find("MainCamera").GetComponent<CameraFollowAndEffects>();
        gameFlow = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();
	}
}

using UnityEngine;
using System.Collections;

public class IsIntro : MonoBehaviour {

    public GameFlowManager gameFlow;
    public CameraFollowAndEffects effects;

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.transform.parent.name == "Player" && Game.current.newGame)
        {
			Game.current.newGame = false;
            gameFlow.isIntro = true;
            gameFlow.isNPCAutomatic = true;
            effects.FadeToBlack();
            Debug.Log("working0");
        }
		else
			Debug.Log(Game.current.newGame);
    }

	void Start ()
	{
        effects = GameObject.Find("MainCamera").GetComponent<CameraFollowAndEffects>();
        gameFlow = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();
	}
}

using UnityEngine;
using System.Collections;

public class IsIntro : MonoBehaviour
{
    public void StartIntro()
    {
        PlayerController.current.canTalkToNPC = true;

        PlayerController.current.overlappingNpc = gameObject;
        NpcBehaviour npcBehaviour = transform.GetComponent<NpcBehaviour>();

        PlayerController.current.isOverlappingNPC = true;
        PlayerController.current.ActivateSelection(PlayerController.Selection.NPC);
        PlayerController.current.PlayerAnimStop();

        if (ActivateTextAtLine.current.requireButtonPress)
        {
            //ActivateTextAtLine.current.waitForPress = true;
            //return;
        }

        //npcBehaviour.waitTimer = -1;
        //GameFlowManager.current.player.isIntro = true;
        npcBehaviour.isAutomatic = true;
        //CameraFollowAndEffects.current.FadeToBlack();
        ActivateTextAtLine.current.TalkToNPC();
    }
}

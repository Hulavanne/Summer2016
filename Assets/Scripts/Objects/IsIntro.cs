using UnityEngine;
using System.Collections;

public class IsIntro : MonoBehaviour
{
    public bool isIntro;

    NpcBehaviour npcBehaviour;

    void Awake()
    {
        npcBehaviour = transform.GetComponent<NpcBehaviour>();
    }

    public void StartIntro()
    {
        isIntro = true;
        CameraEffects.current.FadeToBlack(true);
        npcBehaviour.isAutomatic = true;
        Intro();
    }

    public void ContinueIntro()
    {
        npcBehaviour.isAutomatic = false;
        Intro();
    }

    void Intro()
    {
        PlayerController.current.isOverlappingNPC = true;
        PlayerController.current.canTalkToNPC = true;
        PlayerController.current.overlappingNpc = gameObject;

        PlayerController.current.ActivateSelection(PlayerController.Selection.NPC);
        PlayerController.current.PlayerAnimStop();

        ActivateTextAtLine.current.TalkToNPC();
    }
}

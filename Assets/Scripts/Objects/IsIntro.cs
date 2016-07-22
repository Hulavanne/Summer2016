using UnityEngine;
using System.Collections;

public class IsIntro : MonoBehaviour
{
    public bool introPlaying;

    NpcBehaviour npcBehaviour;
    bool destroyIntro = false;

    void Awake()
    {
        npcBehaviour = transform.GetComponent<NpcBehaviour>();
        npcBehaviour.isAutomatic = true;
    }

    void Update()
    {
        if (npcBehaviour.waitTimer <= 0.0f && npcBehaviour.isAutomatic && !introPlaying)
        {
            gameObject.GetComponent<IsIntro>().ContinueIntro();
        }
        if (destroyIntro)
        {
            PlayerController.current.DeactivateSelection();
            Destroy(gameObject);
        }
    }

    public void StartIntro()
    {
        introPlaying = true;
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

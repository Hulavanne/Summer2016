using UnityEngine;
using System.Collections;

public class IsIntro : MonoBehaviour
{
    public bool introPlaying;

    CharacterBehaviour npcBehaviour;
    bool destroyIntro = false;

    void Awake()
    {
        npcBehaviour = transform.GetComponent<CharacterBehaviour>();
        npcBehaviour.isAutomatic = true;
    }

    void Update()
    {
        if (npcBehaviour.waitTimer <= 0.0f && npcBehaviour.isAutomatic && !introPlaying && LevelManager.current.currentLevel == 0)
        {
            gameObject.GetComponent<IsIntro>().ContinueIntro();
        }
        if (destroyIntro)
        {
            PlayerController.current.canMove = true;
            PlayerController.current.DeactivateSelection();
            Destroy(gameObject);
        }
    }

    public void StartIntro()
    {
        introPlaying = true;
        CameraEffects.current.FadeToBlack(true, false);
        npcBehaviour.isAutomatic = true;

        PlayerController.current.overlappingNpc = gameObject;
        npcBehaviour.PlayerSelfDialogue();
    }

    public void ContinueIntro()
    {
        npcBehaviour.isAutomatic = false;

        PlayerController.current.overlappingNpc = gameObject;
        npcBehaviour.PlayerSelfDialogue();
    }
}

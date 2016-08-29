using UnityEngine;
using System.Collections;

public class IsIntro : MonoBehaviour
{
    public bool introPlaying;

    CharacterBehaviour behaviour;

    void Awake()
    {
        behaviour = transform.GetComponent<CharacterBehaviour>();
        behaviour.isAutomatic = true;
    }

    void Update()
    {
        if (behaviour.waitTimer <= 0.0f && behaviour.isAutomatic && !introPlaying && LevelManager.current.currentLevel == 0)
        {
            gameObject.GetComponent<IsIntro>().ContinueIntro();
        }
    }

    public void StartIntro()
    {
        introPlaying = true;
        CameraEffects.current.FadeToBlack(true, false, false);
        behaviour.isAutomatic = true;

        PlayerController.current.overlappingNpc = gameObject;
        behaviour.PlayerSelfDialogue();
    }

    public void ContinueIntro()
    {
        behaviour.isAutomatic = false;

        PlayerController.current.overlappingNpc = gameObject;
        behaviour.PlayerSelfDialogue();
    }

    public void DestroyIntro()
    {
        // Remove the intro's CharacterBehaviour from the npcBehaviours list and destroy this object
        EventManager.current.npcBehaviours.Remove(behaviour);

        // If game starts in Cieran's room
        if (LevelManager.current.currentLevel == LevelManager.Levels.CIERAN_BEDROOM)
        {
            PlayerController.current.hud.SetHud(false);
            IntroTutorial.doesShow1 = true;
        }

        Destroy(gameObject);
    }
}
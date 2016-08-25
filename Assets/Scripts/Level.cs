using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    public static Level lastLevel;
    public static SpriteRenderer lightSpriteRenderer;

    public LevelManager.Levels levelName;

    public enum Ground
    {
        SOLID,
        LEAVES
    }
    public Ground groundType;

    public AudioClip levelMusic;
	public bool fixedCamera = false;
    public bool setCameraManually = false;
	public float levelLightAmount;
    public List<Sprite> lightsForLastLevel = new List<Sprite>();

    GameObject boundaries;
    GameObject playerBoundaries;
    GameObject cameraBoundaries;

	void Awake()
	{
        // Boundaries parent object
        if (transform.FindChild("Boundaries") != null)
        {
            boundaries = transform.FindChild("Boundaries").gameObject;

            // Player boundaries
            if (boundaries.transform.FindChild("PlayerBoundary") != null)
            {
                playerBoundaries = boundaries.transform.FindChild("PlayerBoundary").gameObject;
            }

            // Camera boundaries
            if (!setCameraManually)
            {
                if (boundaries.transform.FindChild("CameraBoundary") != null)
                {
                    cameraBoundaries = boundaries.transform.FindChild("CameraBoundary").gameObject;

                    // Set boundaries to the edges of the background
                    if (transform.FindChild("Background").transform.FindChild("BackStatic") != null)
                    {
                        Sprite background = transform.FindChild("Background").transform.FindChild("BackStatic").GetComponent<SpriteRenderer>().sprite;

                        float leftX = transform.position.x - background.bounds.extents.x;
                        float rightX = transform.position.x + background.bounds.extents.x;

                        cameraBoundaries.transform.position = new Vector3(leftX, cameraBoundaries.transform.position.y, cameraBoundaries.transform.position.z);
                        cameraBoundaries.transform.GetChild(0).position = new Vector3(rightX, cameraBoundaries.transform.position.y, cameraBoundaries.transform.position.z);
                    }
                }
            }
        }

        if (levelName == LevelManager.Levels.HELL_CANDLE)
        {
            lastLevel = this;
            lightSpriteRenderer = transform.FindChild("Foreground").GetChild(0).GetComponent<SpriteRenderer>();
            //lightSpriteRenderer.enabled = false;
        }
	}

    public void SwitchLighting()
    {
        // Updating the lighting in the last level
        if (levelName == LevelManager.Levels.HELL_CANDLE &&
            LevelManager.current.currentLevel == LevelManager.Levels.HELL_CANDLE)
        {
            if (EventManager.ending == EventManager.Ending.NORMAL)
            {
                
            }
            else if (EventManager.ending == EventManager.Ending.TRUE)
            {

            }
        }
    }
}

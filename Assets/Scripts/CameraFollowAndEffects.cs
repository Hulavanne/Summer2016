using UnityEngine;
using System.Collections;

public class CameraFollowAndEffects : MonoBehaviour {

    public GameObject darkScreen; // reference for the actual black plane
    public Renderer darkScreenRenderer; // reference for its renderer
    public Color opacityManager; // a color for (0 > red, 0 > green, 0 > blue, 0~1 > opacity)
    public float opacity = 0.0f; // the changeable opacity variable
    public bool turnBlack; // once this is true, the plane gradually turns black
    
    public GameObject player; // reference so camera follows player
    public PlayerController controlPlayer;

    void Awake()
    {
        opacityManager = new Color(0.0f, 0.0f, 0.0f, opacity); // sets opacity to 0
        darkScreenRenderer.material.color = opacityManager;

        darkScreen.GetComponent<Renderer>().material.color = opacityManager;

        turnBlack = false;
        opacity = 1.0f;
    }

	void Update () {
        
        if (turnBlack)
        {
            if (opacity <= 1)
            {
                opacity += 1 * Time.deltaTime; // note that this "1" is a timer and isn't changing anything
            }
        }
        else
        {
            
            if (opacity >= 0)
            {
                opacity -= 1 * Time.deltaTime; // note that this "1" is a timer and isn't changing anything
            }
        }

        opacityManager = new Color(0.0f, 0.0f, 0.0f, opacity); // checks opacity every frame
        darkScreenRenderer.material.color = opacityManager; // and puts it in the material
        
        transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + 2, transform.position.z);
        // ^ this makes the camera follow the player in x axis, and specific y+2 axis
        // Alternatively, just parent the camera to the player, add 2 to y, and delete this
    }
}

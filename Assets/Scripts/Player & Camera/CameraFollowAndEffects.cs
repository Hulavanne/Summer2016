using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraFollowAndEffects : MonoBehaviour {

    public BoxCollider2D rightBoundary;
    public BoxCollider2D leftBoundary;

    public GameObject darkScreen; // reference for the actual black plane
    public CanvasRenderer darkScreenRenderer; // reference for its renderer
    public Color opacityManager; // a color for (0 > red, 0 > green, 0 > blue, 0~1 > opacity)
    public float opacity = 0.0f; // the changeable opacity variable
    public bool turnBlack; // once this is true, the plane gradually turns black

    public float cameraPos = 0.0f, playerPos = 0.0f;
    public float xDistanceToPlayer = 0.0f;
    public GameObject player; // reference so camera follows player
    public PlayerController controlPlayer;
    public TouchInput_Diogo playerMotion;

    public void StartJoinPlayer()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, transform.position.z);
    }

    public void JumpToPlayer()
    {
        if ((xDistanceToPlayer >= 2.0f) || (xDistanceToPlayer <= -2.0f))
        {
            if (playerPos > cameraPos)
            {
                // transform.position = new Vector3(player.transform.position.x - 3, transform.position.y, transform.position.z);
                transform.position = new Vector3(player.transform.position.x - 2.0f, transform.position.y, transform.position.z);
            }

            if (playerPos < cameraPos)
            {
                // transform.position = new Vector3(player.transform.position.x + 3, transform.position.y, transform.position.z);
                transform.position = new Vector3(player.transform.position.x + 2.0f, transform.position.y, transform.position.z);
            }

        }
    }

    void GetDistance()
    {
        playerPos = player.transform.position.x;
        cameraPos = transform.position.x;

        if (playerPos > cameraPos)
            xDistanceToPlayer = (playerPos - cameraPos);
        else
            xDistanceToPlayer = (cameraPos - playerPos);

        xDistanceToPlayer = (transform.position.x) - (player.transform.position.x);
    }

    void Awake()
    {
        StartJoinPlayer();
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

        darkScreenRenderer.SetAlpha(opacity);

        opacityManager = new Color(0.0f, 0.0f, 0.0f, opacity); // checks opacity every frame
                                                               // darkScreenRenderer.material.color = opacityManager; // and puts it in the material

        GetDistance();

        transform.position = new Vector3(transform.position.x, player.transform.position.y + 2, transform.position.z);

        if (!controlPlayer.canCameraFollow)
        {
            return;
        }

        // JumpToPlayer();

       

        // transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, transform.position.z);



        // ^ this makes the camera follow the player in x axis, and specific y+2 axis
        // Alternatively, just parent the camera to the player, add 2 to y, and delete this
    }
}

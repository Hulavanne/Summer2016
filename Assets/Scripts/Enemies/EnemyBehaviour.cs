using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    //new

    //player references

    public GameObject playerObj;
    public PlayerController player;

    //distances
    public float distanceBetweenPlayer, playerPos, enemyPos; // respectively :
              // ^ distance from enemy to Player (placeholder) : will modulate and subtract playerPos with enemyPos and create a vector for it.

    public bool goToPlayerPos;
    public float areaOfVision = 6.0f; // distance radius to start chasing

    //amounts
    public float movementSpeed = 0.5f; // normal patrolling movement speed
    public float suspiciousSpeed = 1.0f; // suspicious more speedy movement speed
    public float chaseSpeed = 2.0f; // chase speed

    //timers
    public float turningTime = 0; // this is a Timer that resets everytime it reaches 1
                // ^ it will mod (%2) always, and every once in a while (everytime it hits 0.5 or 1) switches direction on enemy
    public float waitTime;
    public float suspicionTime;
    public float touchPlayerTime = 0.0f; // this will end the game if the enemy touches the player for too long
    public float unhidePlayerTime = 0.0f; // enemy will remove the player if it knows it just hid (is in Suspicious mode too).
    public float startChaseTime = 0.0f;

    // other
    // public bool IsFollowingPlayer;

    enum EnemyBehav
    {
        PATROLLING,
        SUSPICIOUS,
        CHASING,
        REMOVING_PLAYER,
    };

    EnemyBehav currentEnemy;

    void UnhidePlayer()
    {
        if (((currentEnemy == EnemyBehav.SUSPICIOUS) || (currentEnemy == EnemyBehav.CHASING)) && (!goToPlayerPos))
            // if everything is right, the enemy won't be needing to follow the player by this time
        {


            unhidePlayerTime += Time.deltaTime;
            if (unhidePlayerTime > 1.0f)
            {
                player.PlayerUnhide();
                player.canHide = false;
            }
            
        }
    }

    void OnTriggerStay2D(Collider2D col) // touching player
    {
        if (col.gameObject.tag == "Player")
        {
            touchPlayerTime += Time.deltaTime;
        }

        if (touchPlayerTime >= 0.4)
        {
            player.isGameOver = true;
        }
    }

    void OnTriggetExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            touchPlayerTime = 0;
        }
    }

    void CheckState() // only for debugging
    {
        
    }

    void EnemyMove()
    {
        Vector3 tempVecX = new Vector3 (movementSpeed, 0, 0); // normal movement
        Vector3 tempVecRunX = new Vector3 (chaseSpeed, 0, 0); // chasing movement

        

        if ((playerObj.transform.position.x > transform.position.x) && (currentEnemy == EnemyBehav.PATROLLING))
        {
            transform.position += tempVecX * Time.deltaTime;
        }
        else if ((playerObj.transform.position.x < transform.position.x) && (currentEnemy == EnemyBehav.PATROLLING))
        {
            transform.position -= tempVecX * Time.deltaTime;
        }

        if ((playerObj.transform.position.x > transform.position.x) && (currentEnemy == EnemyBehav.CHASING))
        {
            transform.position += tempVecRunX * Time.deltaTime;
        }
        else if ((playerObj.transform.position.x < transform.position.x) && (currentEnemy == EnemyBehav.CHASING))
        {
            transform.position -= tempVecRunX * Time.deltaTime;
        }

        if ((playerObj.transform.position.x > transform.position.x) && (currentEnemy == EnemyBehav.SUSPICIOUS))
        {
            transform.position += tempVecRunX * Time.deltaTime;
        }
        else if ((playerObj.transform.position.x < transform.position.x) && (currentEnemy == EnemyBehav.SUSPICIOUS))
        {
            transform.position -= tempVecRunX * Time.deltaTime;
        }

        GetDistanceBetweenObjects();   
    }

    void EnemyPatrol()
    {
        
        Vector3 tempVecX = new Vector3(1.0f, 0, 0); // normal movement

        if (goToPlayerPos)
        {
            currentEnemy = EnemyBehav.SUSPICIOUS;
            EnemyMove();
            if ((transform.position.x > player.transform.position.x - 0.05) && (transform.position.x < player.transform.position.x + 0.05))
            {
                Debug.Log("working!");
                goToPlayerPos = false;
                currentEnemy = EnemyBehav.SUSPICIOUS;

                return;
            }
        }
        else
        {
            if ((turningTime % 1.0f) > 0.5f) // getting even/uneven number here
            {
                transform.position += tempVecX * Time.deltaTime;
            }
            else
            {
                transform.position -= tempVecX * Time.deltaTime;
            }
        }

        if (currentEnemy == EnemyBehav.PATROLLING)
        {
            turningTime += 0.1f * Time.deltaTime;
        }
        
        else if (currentEnemy == EnemyBehav.SUSPICIOUS)
        {
            turningTime += 0.3f * Time.deltaTime;
        }


        if (turningTime > 1.0f)
        {
            turningTime = 0.0f;
        }
    }

    void CheckDistance()
    {

        if ((distanceBetweenPlayer > areaOfVision) || (distanceBetweenPlayer < -areaOfVision))
        {
            currentEnemy = EnemyBehav.PATROLLING;
        }
        else
        {
            if (!player.isHidden)
            {
                startChaseTime += Time.deltaTime;
                if (startChaseTime > 0.5)
                {
                    currentEnemy = EnemyBehav.CHASING;

                    startChaseTime = 0;
                }
            }
            else
            {
               
                if (suspicionTime > 0)
                {
                    currentEnemy = EnemyBehav.SUSPICIOUS;
                    suspicionTime -= (Time.deltaTime);
                }
                else if (suspicionTime <= 0)
                {
                    currentEnemy = EnemyBehav.PATROLLING;
                }
            }
        }

    }

    void GetDistanceBetweenObjects ()
    {
        playerPos = playerObj.transform.position.x;
        enemyPos = transform.position.x;
        
        if (playerPos > enemyPos)
            distanceBetweenPlayer = (playerPos - enemyPos);
        else
            distanceBetweenPlayer = (enemyPos - playerPos);
    }

	void Awake () {
        currentEnemy = EnemyBehav.PATROLLING;
	}
	
	void Update () {

        switch (currentEnemy)
        {
            case EnemyBehav.PATROLLING:
                {
                    areaOfVision = 6.0f;
                    // Debug.Log("Patrolling state!");
                    break;
                }
            case EnemyBehav.SUSPICIOUS:
                {
                    areaOfVision = 8.0f;
                    // Debug.Log("Suspicious state!");
                    break;
                }
            case EnemyBehav.CHASING:
                {
                    areaOfVision = 12.0f; // this is probably too much?
                    // Debug.Log("Chasing state!");
                    break;
                }
        }


        if ((currentEnemy == EnemyBehav.CHASING) && (player.isHidden))
        {
            goToPlayerPos = true;
            currentEnemy = EnemyBehav.SUSPICIOUS;
        }

        // currentEnemy = EnemyBehav.defaultCase; //checking that this gets reset in the beggining of every frame

        if ((distanceBetweenPlayer > areaOfVision) || (distanceBetweenPlayer < -areaOfVision))
        {
            currentEnemy = EnemyBehav.SUSPICIOUS;
        }
        
        if (currentEnemy == EnemyBehav.SUSPICIOUS)
        {

        }

        CheckState(); // > for debugging
        GetDistanceBetweenObjects();
        CheckDistance();
        
        if ((transform.position.y > player.transform.position.y - 0.2) && (transform.position.y > player.transform.position.y + 0.2))
        {
            currentEnemy = EnemyBehav.PATROLLING;
        }

        if (currentEnemy == EnemyBehav.SUSPICIOUS)
        {
            UnhidePlayer();
        }

        if (currentEnemy == EnemyBehav.PATROLLING || (currentEnemy == EnemyBehav.SUSPICIOUS))
        {
            EnemyPatrol();
        }
        else if ((currentEnemy == EnemyBehav.CHASING))
        {
            EnemyMove();
        }
        else if (currentEnemy == EnemyBehav.REMOVING_PLAYER)
        {
            // play some animation
        }
        
    }
}

﻿using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour
{
    public AudioClip patrolSound;
    public AudioClip chasingSound;

    public GameObject boundary;
    public GameObject playerObj;
    public PlayerController player;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;

    //distances
    // distance from enemy to Player (placeholder) : will modulate and subtract playerPos with enemyPos and create a vector for it.
    public float distanceBetweenPlayer, playerPos, enemyPos;

    public Vector3 initPos;
    public bool goToPlayerPos;
    public bool isTouchingPlayer;
    public float areaOfVision;
    float vision; // distance radius to start chasing

    //amounts
    public float movementSpeed = 1.0f; // normal patrolling movement speed
    public float suspiciousSpeed = 1.8f; // suspicious more speedy movement speed
    public float chaseSpeed = 4.0f; // chase speed

    //timers
    public bool suspiciousTimerActive;
    public float suspiciousTimer = 0;
    public float turningTime = 0; // this is a Timer that resets everytime it reaches 1  // it will mod (%2) always, and every once in a while (everytime it hits 0.5 or 1) switches direction on enemy
    public int movementDirection = 0;
    public int initialMovementDirection;
    public float waitTime;
    public float chasingTime;
    public float unhidePlayerTime = 0.0f; // enemy will remove the player if it knows it just hid (is in Suspicious mode too).
    public float startChaseTime = 2.0f;
    float startChaseTimer = 0.0f;

    public LevelManager.Levels thisEnemyLevel;

    public enum EnemyBehav
    {
        PATROLLING,
        SUSPICIOUS,
        CHASING,
    };

    public EnemyBehav currentEnemy;

    void Awake()
    {
        initialMovementDirection = movementDirection;
        initPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
        playerObj = GameObject.Find("Player");
        player = playerObj.GetComponent<PlayerController>();
        currentEnemy = EnemyBehav.PATROLLING;
        anim = transform.GetChild(0).GetComponent<Animator>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        audioSource = transform.GetComponent<AudioSource>();
    }

    void Update()
    {
        CheckAnim();
        UnhidePlayer();

        if (currentEnemy == EnemyBehav.SUSPICIOUS || currentEnemy == EnemyBehav.CHASING)
        {
            suspiciousTimer -= Time.deltaTime;

            if (suspiciousTimerActive)
            {
                if (suspiciousTimer <= 0.0f)
                {
                    currentEnemy = EnemyBehav.PATROLLING;
                    suspiciousTimerActive = false;
                    goToPlayerPos = false;
                }
            }
            else
            {
                 suspiciousTimer = 2.0f;
            }
        }

        switch (currentEnemy)
        {
            case EnemyBehav.PATROLLING:
                {
                    vision = areaOfVision * 1.0f;
                    ChangeBreathingSound(patrolSound);
                    break;
                }
            case EnemyBehav.SUSPICIOUS:
                {
                    vision = areaOfVision * 1.2f;
                    ChangeBreathingSound(patrolSound);
                    break;
                }
            case EnemyBehav.CHASING:
                {
                    vision = areaOfVision * 1.4f;
                    ChangeBreathingSound(chasingSound);
                    break;
                }
        }

        if ((currentEnemy == EnemyBehav.CHASING) && (player.isHidden))
        {
            goToPlayerPos = true;
            currentEnemy = EnemyBehav.SUSPICIOUS;
        }

        if ((distanceBetweenPlayer > vision || distanceBetweenPlayer < -vision)
            && !player.isHidden)
        {
            currentEnemy = EnemyBehav.SUSPICIOUS;
        }
        
        GetDistanceBetweenObjects();
        CheckDistance();

        if (currentEnemy == EnemyBehav.PATROLLING || currentEnemy == EnemyBehav.SUSPICIOUS)
        {
            EnemyPatrol();
        }
        else if ((currentEnemy == EnemyBehav.CHASING))
        {
            EnemyMove();
        }
    }

    /*void LateUpdate()
    {
        // If the player hasn't yet given berries to the deer,
        // disable the enemy in the forest
        if (thisEnemyLevel == LevelManager.Levels.FOREST_ENEMY)
        {
            if (Game.current.triggeredEvents.ContainsKey(CharacterBehaviour.Type.DEER))
            {
                if (Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] >= 2)
                {
                    return;
                }
            }

            gameObject.SetActive(false);
        }
        // If the player hasn't yet inspected the door puzzle,
        // disable the enemy in the crevice
        else if (thisEnemyLevel == LevelManager.Levels.CAVE_CREVICE)
        {
            if (!Game.current.triggeredEvents.ContainsKey(CharacterBehaviour.Type.DOOR_PUZZLE))
            {
                gameObject.SetActive(false);
            }
        }
    }*/

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.transform.parent.tag == "Player")
        {
            isTouchingPlayer = true;
        }

        if (col.tag == "PlayerBoundary" || col.tag == "EnemyBoundary")
        {
            movementDirection *= -1;
            turningTime = -2.0f;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.transform.parent.tag == "Player")
        {
            if (!player.isHidden)
            {
                player.isGameOver = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.transform.parent.tag == "Player")
        {
            isTouchingPlayer = false;
        }
    }

    void ChangeBreathingSound(AudioClip sound)
    {
        if (audioSource.clip != sound)
        {
            audioSource.clip = sound;
            audioSource.Play();
        }
    }

    void UnhidePlayer()
    {
        if ((currentEnemy == EnemyBehav.SUSPICIOUS || currentEnemy == EnemyBehav.CHASING) && isTouchingPlayer)
        {
            unhidePlayerTime += Time.deltaTime;
            if (unhidePlayerTime > 0.5f)
            {
                player.isGameOver = true;
            }
        }
    }

    void EnemyMove()
    {
        if (thisEnemyLevel != LevelManager.current.currentLevel)
        {
            transform.position = initPos;
            movementDirection = initialMovementDirection;
            return;
        }
        if (TextBoxManager.current.isTalkingToNPC)
        {
            return;
        }

        Vector3 tempVecX = new Vector3 (movementSpeed, 0, 0); // normal movement
        Vector3 tempVecRunX = new Vector3 (chaseSpeed, 0, 0); // chasing movement
        
        if (currentEnemy == EnemyBehav.PATROLLING)
        {
            if (movementDirection == 1)
            {
                transform.position += tempVecX * Time.deltaTime;
            }
            else if (movementDirection == -1)
            {
                transform.position -= tempVecX * Time.deltaTime;
            }
        }

        if (currentEnemy == EnemyBehav.CHASING || currentEnemy == EnemyBehav.SUSPICIOUS)
        {
            if (transform.position.x >= player.transform.position.x)
            {
                movementDirection = -1;
            }
            else
            {
                movementDirection = 1;
            }
            if (movementDirection == 1)
            {
                if (transform.position.x > player.transform.position.x + 0.1f || transform.position.x < player.transform.position.x - 0.1f)
                {
                    transform.position += tempVecRunX * Time.deltaTime;
                }
                else
                {
                    anim.SetBool("isPatrolling", false);
                }
            }
            else if (movementDirection == -1)
            {
                if (transform.position.x > player.transform.position.x + 0.1f || transform.position.x < player.transform.position.x - 0.1f)
                {
                    transform.position -= tempVecRunX * Time.deltaTime;
                }
                else
                {
                    anim.SetBool("isPatrolling", false);
                }
            }
        }
        
        GetDistanceBetweenObjects();   
    }

    void EnemyPatrol()
    {
        if (thisEnemyLevel != LevelManager.current.currentLevel)
        {
            transform.position = initPos;
            movementDirection = initialMovementDirection;
            return;
        }
        if (TextBoxManager.current.isTalkingToNPC)
        {
            return;
        }

        Vector3 tempVecX = new Vector3(movementSpeed, 0, 0); // normal movement

        if (goToPlayerPos)
        {
            suspiciousTimerActive = true;
            currentEnemy = EnemyBehav.SUSPICIOUS;
            EnemyMove();
            if ((transform.position.x > playerObj.transform.position.x - 0.05) && (transform.position.x < playerObj.transform.position.x + 0.05))
            {
                currentEnemy = EnemyBehav.SUSPICIOUS;
                return;
            }
        }
        else
        {
            if (movementDirection == 1)
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
            turningTime = 0.0f;
        }
        
        else if (currentEnemy == EnemyBehav.SUSPICIOUS)
        {
            turningTime += 0.3f * Time.deltaTime;
        }

        if (turningTime > 1.0f)
        {
            turningTime = Random.Range(-2.0f, -1.0f);
        }
    }

    void CheckAnim()
    {
        if (currentEnemy == EnemyBehav.CHASING)
        {
            anim.SetBool("isChasing", true);
        }
        else if (currentEnemy == EnemyBehav.SUSPICIOUS)
        {
            anim.SetBool("isChasing", false);
            anim.SetBool("isPatrolling", true);
        }
        else if (currentEnemy == EnemyBehav.PATROLLING)
        {
            anim.SetBool("isChasing", false);
            anim.SetBool("isPatrolling", true);
        }

        if (movementDirection == 1)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementDirection == -1)
        {
            spriteRenderer.flipX = false;
        }

        if (thisEnemyLevel != LevelManager.current.currentLevel)
        {
            anim.SetBool("isChasing", false);
            anim.SetBool("isPatrolling", false);
        }
    }

    void CheckDistance()
    {
        if ((distanceBetweenPlayer > vision) || (distanceBetweenPlayer < -vision))
        {
            currentEnemy = EnemyBehav.PATROLLING;
        }
        else
        {
            if (!player.isHidden)
            {
                startChaseTimer += Time.deltaTime;

                if (startChaseTimer >= startChaseTime)
                {
                    currentEnemy = EnemyBehav.CHASING;
                    startChaseTimer = 0;
                }
            }
            else if (player.isHidden && currentEnemy != EnemyBehav.PATROLLING)
            {
                if (chasingTime > 0)
                {
                    suspiciousTimerActive = true;
                    currentEnemy = EnemyBehav.SUSPICIOUS;
                    chasingTime -= (Time.deltaTime);
                    if (suspiciousTimer <= 0.0f)
                    {
                        goToPlayerPos = false;
                        currentEnemy = EnemyBehav.PATROLLING;
                    }
                }
                else if (chasingTime <= 0)
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
}

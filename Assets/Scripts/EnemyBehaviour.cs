using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    public GameObject player;
    public bool IsFollowingPlayer;
    public float distanceBetweenPlayer, distance1, distance2;
    public float minDistance = 4.0f;

    enum EnemyBehav
    {
        Patrolling,
        Suspicious,
        Chasing,
    };

    EnemyBehav currentEnemy;

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("killing player.");
        }
    }
    
    void GetDistanceBetweenObjects ()
    {
        distance1 = player.transform.position.x;
        distance2 = transform.position.x;
        
        if (distance1 > distance2)
            distanceBetweenPlayer = (distance1 - distance2);
        else
            distanceBetweenPlayer = (distance2 - distance1);
    }

	void Awake () {
        IsFollowingPlayer = true;
        currentEnemy = EnemyBehav.Patrolling;
	}
	
	void Update () {

        switch (currentEnemy)
        {
            case EnemyBehav.Patrolling:

                break;

            case EnemyBehav.Suspicious:

                break;

            case EnemyBehav.Chasing:

                break;
        }


        Vector3 tempVecX = new Vector3(0.5f, 0, 0);
        if (((player.transform.position.x)>transform.position.x) && IsFollowingPlayer)
        {
            transform.position += tempVecX * Time.deltaTime;
        }
        else if (((player.transform.position.x)<transform.position.x) && IsFollowingPlayer)
        {
            transform.position -= tempVecX * Time.deltaTime;
        }

        GetDistanceBetweenObjects();

        if ((distanceBetweenPlayer>minDistance)||(distanceBetweenPlayer<-minDistance))
        {
            IsFollowingPlayer = false;
        }
        else
        {
            IsFollowingPlayer = true;
        }
	    
	}
}

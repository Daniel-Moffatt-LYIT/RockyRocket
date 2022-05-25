using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public static GameObject player;
    Vector3[] waypoints;
    NavMeshAgent nav;
    enum EnemyType
    {
        small,big,boss
    }

    [SerializeField]
    EnemyType enemyType;
    public static int startingNumberOfWaypoints = 5;
    int currentWaypoint = 0;
    [SerializeField]
    float waitAtWaypointTime;
    public static PoolObject enemyBulletPool;
    float waitingTimer;
    [SerializeField]
    int chanceOfShooting;
    
    private void OnEnable()
    {
        
        waitingTimer = waitAtWaypointTime;
        GetWaypoints();

    }
    private void Update()
    {
        DieIfPlayerDies();

        switch (enemyType)
        {
            case EnemyType.small:
                SmallAI();
                break;

            case EnemyType.big:
                BigAI();
                break;

            case EnemyType.boss:
                BossAI();
                break;


        }
    }

    private void GetWaypoints()
    {
        currentWaypoint = 0;
        waypoints = new Vector3[startingNumberOfWaypoints];
        for (int i = 0; i < startingNumberOfWaypoints; i++)
        {
            waypoints[i] = new Vector3(Random.Range(-7.0f, 7.0f), -0.5f, Random.Range(0, 20.0f));
        }

        nav = GetComponent<NavMeshAgent>();
        if (nav)
        {
            nav.SetDestination(waypoints[currentWaypoint]);
          
        }
    }

    private void DieIfPlayerDies()
    {
        if (nav == null||player == null)
        {
            return;
        }
        if (!player.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void SmallAI()
    {
        if (!nav.isOnNavMesh)
            return;
        if (nav.remainingDistance < 1 && waitingTimer < 0)
        {
            currentWaypoint++;
            if (currentWaypoint > startingNumberOfWaypoints - 1)
            {
                if (player != null&&player.activeInHierarchy)
                {
                    nav.SetDestination(player.transform.position);
                }
                
                waitingTimer = waitAtWaypointTime;
            }
            else
            {
                nav.SetDestination(waypoints[currentWaypoint]);
            }
        }
        else if (nav.remainingDistance < 1)
        {
            waitingTimer -= Time.deltaTime;
        }
    }

    private void BigAI()
    {
        if (nav.isOnNavMesh)
        {
            if (nav.remainingDistance < 1 && waitingTimer < 0)
            {
                currentWaypoint++;
                int r = (int)Random.Range(0, 100);
                if (chanceOfShooting <= r)
                {
                    Shoot();
                }
                if (currentWaypoint > startingNumberOfWaypoints - 1)
                {
                    if (player != null && player.activeInHierarchy)
                    {
                        nav.SetDestination(player.transform.position);
                    }

                    waitingTimer = waitAtWaypointTime;
                }
                else
                {
                    nav.SetDestination(waypoints[currentWaypoint]);
                }
            }
            else if (nav.remainingDistance < 1)
            {
                waitingTimer -= Time.deltaTime;
            }
        }
    }

    private void BossAI()
    {
        if (nav.remainingDistance < 1 && waitingTimer < 0)
        {
            currentWaypoint++;
            
            if (currentWaypoint > startingNumberOfWaypoints - 1)
            {
                GetWaypoints();
                waitingTimer = waitAtWaypointTime;
            }
            else
            {
                nav.SetDestination(waypoints[currentWaypoint]);
            }
        }
        else if (nav.remainingDistance < 1)
        {
            waitingTimer -= Time.deltaTime;
        }
    }

    public void SetWaitTime(float time)
    {
        waitAtWaypointTime = time;
    }
    public void SetSpeed(float speed)
    {
        nav.speed = speed;
    }

    private void Shoot()
    {
        if(enemyBulletPool == null)
        {
            return;
        }
        GameObject bullet = enemyBulletPool.GetGameObject();
        if (bullet != null)
        {
            bullet.transform.position = this.transform.position;
            bullet.transform.rotation = this.transform.rotation;
            bullet.SetActive(true);
        }
    }

    
}

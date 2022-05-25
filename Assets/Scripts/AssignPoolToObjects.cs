using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignPoolToObjects : MonoBehaviour
{
   
    public PoolObject[] pools;
    private void Start()
    {
        EnemyCollisions.enemyExplosionPool = pools[0];
        EnemyCollisions.pickupPool = pools[1];
        ShootBullets.playerBulletPool = pools[2];
        EnemyAI.enemyBulletPool = pools[3];

    }
}

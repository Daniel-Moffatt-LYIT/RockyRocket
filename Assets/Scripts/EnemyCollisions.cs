using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    [SerializeField]
    GameObject[] ships;
    [SerializeField]
    bool setShipOnEnable;
    Material mat;
    AudioSource source;
    public static PoolObject enemyExplosionPool;
    [SerializeField]
    int startingHealth = 3;
    private int currentHealth;
    
    public static PoolObject pickupPool;
    [SerializeField]
    int percentChanceOfPickup;
    [SerializeField]
    AudioClip deathSound;
    [SerializeField]
    int scoreValue;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        if (setShipOnEnable == true)
        {
            SetShip();
        }
        if(mat == null)
        {
            mat = GetComponent<MeshRenderer>().material;
        }
        currentHealth = startingHealth;
    }
    public void SetHealth(int amount) //used by game manager to set health for all enemies
    {
        startingHealth = amount;
    }
    void SetShip()
    {
        int rand = (int)Random.Range(0, ships.Length);
        foreach (GameObject obj in ships)
        {
            obj.SetActive(false);
        }
        ships[rand].SetActive(true);
    }
private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            if (currentHealth <= 0)
            {
                //add score
                AddScore();
                //play an explosion effect
                Explosion();
                //spawn a pickup random chance
                SpawnPickup();
                this.gameObject.SetActive(false);
            }
            else //still have some health
            {
                //play the hit by bullet sound
                source.Play();

            }
            //decrease health and kill the bullet
            currentHealth--;
            other.gameObject.SetActive(false);
          
        }
    }
    public void AddHealth()
    {
        startingHealth++;
    }

    void AddScore()
    {
        GameManager.instance.AddScore(scoreValue);
    }
    void Explosion()
    {
        GameObject explosion = enemyExplosionPool.GetGameObject();
        explosion.transform.position = this.transform.position;
        explosion.GetComponent<AudioSource>().clip = deathSound;
        explosion.SetActive(true);
    }

    void SpawnPickup()
    {
        int diceRoll = (int)Random.Range(0, 100);
        if (diceRoll < percentChanceOfPickup)
        {
            GameObject pickup = pickupPool.GetGameObject();
            pickup.transform.position = this.transform.position;
            pickup.SetActive(true);
        }
    }
}

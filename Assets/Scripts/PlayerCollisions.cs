using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public static HealthBarHandler healthBarHandler;
    [SerializeField]
    int startingHealth;
    AudioSource source;
    public static int health;
    [SerializeField]
    float lengthOfCooldown;
    float bulletCooldown;
    bool recentlyHit = false;
    [SerializeField]
    GameObject myExplosion;
    public static bool iAmDead = false;
    [SerializeField]
    MeshRenderer[] myRenderers;
    ScreenShake shake;
    [SerializeField]
    LoadAd ads;
    private void Start()
    {
        shake = Camera.main.gameObject.GetComponent<ScreenShake>();
        source = GetComponent<AudioSource>();
        myExplosion = (GameObject)Instantiate(myExplosion);
        myExplosion.SetActive(false);
    }
    public void SetHealthBar()
    {
        if (healthBarHandler != null)
        {
            healthBarHandler.setImages(health);
        }
    }
    private void OnEnable()
    {
        health = GameManager.instance.state.playerHealth;
        SetHealthBar();        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            EnemyHit(other,true);
        }
        else if (other.gameObject.tag == "Pickup")
        {
            GameManager.instance.UpgradePlayer();
            source.Play();
            other.gameObject.SetActive(false);
        }else if(other.gameObject.tag == "Boss")
        {
            EnemyHit(other, false);
        }
    }
    private void Update()
    {

        if (bulletCooldown >= 0&&recentlyHit == true)
        {
            myRenderers[ShipSelect.skinNumber].enabled = !myRenderers[ShipSelect.skinNumber].enabled;
            bulletCooldown -= Time.deltaTime;
        }
        else
        {
            myRenderers[ShipSelect.skinNumber].enabled = true;
            recentlyHit = false;
            bulletCooldown = lengthOfCooldown;
        }
        if(iAmDead == true)
        {
            
            iAmDead = false;
            Invoke("Respawn", 1);
            myExplosion.transform.position = this.transform.position;
            myExplosion.SetActive(true);
            this.gameObject.SetActive(false);
            
        }
    }

    void Respawn()
    {
        this.gameObject.SetActive(true);
    }

    void EnemyHit(Collider enemy,bool killEnemy)
    {
        if (recentlyHit == false)
        {
            GameManager.instance.state.combo = 1;
            shake.shakeTimer = 1.0f;
            Debug.Log("lost health");
            health--;
            Handheld.Vibrate();
            if (healthBarHandler != null)
            {
                healthBarHandler.setImages(health);
            }
            bulletCooldown = lengthOfCooldown;
            recentlyHit = true;
            if(killEnemy == true)
                enemy.gameObject.SetActive(false);
        }
        
        if (health <= 0)
        {
            ads.ShowAd();
            // load reward ad
            iAmDead = true;
        }
    }
}

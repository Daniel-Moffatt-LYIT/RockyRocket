using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullets : MonoBehaviour
{
    public static PoolObject playerBulletPool;
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    Vector3 offsetRotation;
    [SerializeField]
    float fireRate;
    public static bool isShooting = true;
    AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    void OnEnable()
    {
        fireRate = GameManager.instance.state.playerFireRate;
        EnemyAI.player = this.gameObject;
        StartCoroutine(ShootBullet());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator ShootBullet()
    {
        while (true)
        {
            if (isShooting == true&&PlayerCollisions.iAmDead == false)
            {
                GameObject bullet = GetBullet();
                if (bullet != null)
                {
                    bullet.transform.position = this.transform.position + offset;
                    bullet.transform.rotation =
                        Quaternion.Euler(offsetRotation.x+transform.rotation.eulerAngles.x,
                        offsetRotation.y + transform.rotation.eulerAngles.y, 
                        offsetRotation.z + transform.rotation.eulerAngles.z);
                    bullet.SetActive(true);
                    PlaySound();
                }
            }
            yield return new WaitForSeconds(fireRate);
        }
    }


    private GameObject GetBullet()
    {
        return playerBulletPool.GetGameObject();
    }

    void PlaySound()
    {
        source.pitch = Random.Range(0.95f, 1.05f);
        source.Play();
    }
}

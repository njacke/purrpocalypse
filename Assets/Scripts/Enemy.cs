using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreVaule = 100;


    [Header("Projectile")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] int projectiles = 1;
    [SerializeField] GameObject firstProjectile;
    [SerializeField] float projectileSpeedY = 10f;
    [SerializeField] float projectileSpeedX = 0f;
    [SerializeField] GameObject secondProjectile;
    [SerializeField] float projectile2SpeedY = 10f;
    [SerializeField] float projectile2SpeedX = 0f;



    [Header("Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0,1)] float deathSoundVolume;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume;

    bool bossAdd = false;


    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        if (FindObjectsOfType<Boss>().Length > 0)
        {
            bossAdd = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
        if (bossAdd == true)
        {
            if (FindObjectOfType<Boss>().BossAlive() == false)
            {
                Die();
            }
        }
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(
            firstProjectile,
            transform.position,
            Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeedX, -projectileSpeedY);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        
        if (projectiles > 1)
        {
            GameObject laser2 = Instantiate(
                firstProjectile,
                transform.position,
                Quaternion.identity) as GameObject;
            laser2.GetComponent<Rigidbody2D>().velocity = new Vector2(projectile2SpeedX, -projectile2SpeedY);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player") //kamikaze player attack
        {
            Die();
        }
        else
        {
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if (!damageDealer) { return; }
            ProcessHit(damageDealer);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();

        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreVaule);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}

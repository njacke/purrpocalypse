using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] GameObject phaseOnePath;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float health = 100;

    [Header("Attack AOE")]
    [SerializeField] GameObject AOEProjectile;
    [SerializeField] float AOEShotDelay = 1f;
    [SerializeField] float AOEProjectileOneSpeedY = 5f;
    [SerializeField] float AOEProjectileOneSpeedX = 0f;
    [SerializeField] float AOEProjectileTwoSpeedY = 5f;
    [SerializeField] float AOEProjectileTwoSpeedX = 0f;
    [SerializeField] float AOEProjectileThreeSpeedY = 5f;
    [SerializeField] float AOEProjectileThreeSpeedX = 0f;
    [SerializeField] float AOEProjectileFourSpeedY = 5f;
    [SerializeField] float AOEProjectileFourSpeedX = 0f;

    [SerializeField] [Range(0, 1)] float AOEShootSoundVolume = 0.5f;
    [SerializeField] AudioClip AOEShootSound;

    float AOETimer;


    Vector3 centerPos = new Vector3(0, 0, 0);

    bool startAddsSpawned = false;
    AddSpawner startAddsScript;
    float phaseOneAddsDelay = 2f;
    float phaseOneAddsTimer;





    List<Transform> waypoints;
    int waypointIndex = 0;

    /* 
    [Header("Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume;

   */
    // Start is called before the first frame update
    void Start()
    {
        startAddsScript = (AddSpawner)GameObject.Find("Start Adds").GetComponent(typeof(AddSpawner));

        waypoints = GetWaypoints();
        transform.position = centerPos;
    }


    // Update is called once per frame
    void Update()
    {
        /*if (startAddsSpawned == false)
        {
            startAddsSpawned = true;
            StartCoroutine(startAddsScript.SpawnAllWaves());
        }*/
        Move();

        /*if (health <= 500)
        {
            MoveCenter();
            if (transform.position == centerPos)
            {
                AttackAOE();
                if (phaseOneAdds == false)
                    {
                    phaseOneAdds = true;
                    StartCoroutine(FindObjectOfType<AddSpawner>().SpawnAllWaves());
                    Debug.Log("Add spawner function called");
                    }
            }

        }
        else
        Move();*/
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = moveSpeed * Time.deltaTime;
            phaseOneAddsTimer -= Time.deltaTime;
            if (phaseOneAddsTimer <= 0)
            {
                StartCoroutine(startAddsScript.SpawnAllWaves());
                phaseOneAddsTimer = phaseOneAddsDelay;
            }
            transform.position = Vector2.MoveTowards
                (transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            waypointIndex = 0;
        }
    }

    private void MoveCenter()
    {
        var targetPosition = new Vector2(0, 0);
        var movementThisFrame = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
    }

    private List<Transform> GetWaypoints() //dodaj parameter za path
    {
        var bossWaypoints = new List<Transform>();
        foreach (Transform child in phaseOnePath.transform)
        {
            bossWaypoints.Add(child);
        }
        return bossWaypoints;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
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
        Destroy(gameObject);
    }


    private void AttackAOE()
    {
        AOETimer -= Time.deltaTime;
        if (AOETimer <= 0)
        {
            Fire(AOEProjectile, AOEProjectileOneSpeedX, AOEProjectileOneSpeedY, AOEShootSound, AOEShootSoundVolume);
            Fire(AOEProjectile, AOEProjectileTwoSpeedX, AOEProjectileTwoSpeedY, AOEShootSound, AOEShootSoundVolume);
            Fire(AOEProjectile, AOEProjectileThreeSpeedX, AOEProjectileThreeSpeedY, AOEShootSound, AOEShootSoundVolume);
            Fire(AOEProjectile, AOEProjectileFourSpeedX, AOEProjectileFourSpeedY, AOEShootSound, AOEShootSoundVolume);
            Fire(AOEProjectile, -AOEProjectileOneSpeedX, -AOEProjectileOneSpeedY, AOEShootSound, AOEShootSoundVolume);
            Fire(AOEProjectile, -AOEProjectileTwoSpeedX, -AOEProjectileTwoSpeedY, AOEShootSound, AOEShootSoundVolume);
            Fire(AOEProjectile, -AOEProjectileThreeSpeedX, -AOEProjectileThreeSpeedY, AOEShootSound, AOEShootSoundVolume);
            Fire(AOEProjectile, -AOEProjectileFourSpeedX, -AOEProjectileFourSpeedY, AOEShootSound, AOEShootSoundVolume);
            AOETimer = AOEShotDelay;
        }
    }





    private void Fire(GameObject projectile, float speedX, float speedY, AudioClip shotSound, float shotSoundVolume)
    {
        GameObject laser = Instantiate(
            projectile,
            transform.position,
            Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, -speedY);
        AudioSource.PlayClipAtPoint(shotSound, Camera.main.transform.position, shotSoundVolume);
    }


    
/*
    private void OnTriggerEnter2D(Collider2D other)
    {

        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
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
    */
}


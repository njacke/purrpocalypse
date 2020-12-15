using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float health = 100;

    // Phases
    [Header("Phases")]
    [SerializeField] float phaseTwoHP = 600f;
    [SerializeField] float phaseThreeHP = 300f;
    [SerializeField] float prephaseTimer = 10f;
    [SerializeField] GameObject phaseOnePath;
    [SerializeField] float transitionOneTimer = 5f;
    [SerializeField] GameObject phaseTwoPath;
    [SerializeField] float waypointWaitTime = 2f;
    [SerializeField] float transitionTwoTimer = 5f;
    [SerializeField] GameObject phaseThreePath;
    [SerializeField] float phaseThreeAddsDelay = 3f;
    float phaseThreeAddsTimer;


    // Attack AOE config
    [Header("Attack AOE")]
    [SerializeField] GameObject AOEProjectile;
    [SerializeField] float AOEShotDelay = 1f;
    float AOETimer;
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

    // Attack Bomb Config
    [Header("Attack Bomb")]
    [SerializeField] GameObject BombProjectile;
    [SerializeField] float BombShotDelay = 1f;
    float BombTimer;
    [SerializeField] float BombProjectileOneSpeedY = 5f;
    [SerializeField] float BombProjectileOneSpeedX = 0f;
    [SerializeField] [Range(0, 1)] float BombShootSoundVolume = 0.5f;
    [SerializeField] AudioClip BombShootSound;

    // Attack Bomb Config
    [Header("Attack Bomb Two")]
    [SerializeField] GameObject BombProjectileTwo;
    [SerializeField] float BombShotDelayTwo = 1f;
    float BombTimerTwo;
    [SerializeField] float BombProjectileOneSpeedYTwo = 5f;
    [SerializeField] float BombProjectileOneSpeedXTwo = 0f;
    [SerializeField] [Range(0, 1)] float BombShootSoundVolumeTwo = 0.5f;
    [SerializeField] AudioClip BombShootSoundTwo;


    // Attack Cone config
    [Header("Attack Cone")]
    [SerializeField] GameObject ConeProjectile;
    [SerializeField] float ConeShotDelay = 1f;
    [SerializeField] float coneOffsetY = 2f;
    [SerializeField] float coneOffsetX = 0.25f;
    float ConeTimer;
    [SerializeField] float ConeProjectileOneSpeedY = 5f;
    [SerializeField] float ConeProjectileOneSpeedX = 0f;
    [SerializeField] float ConeProjectileTwoSpeedY = 5f;
    [SerializeField] float ConeProjectileTwoSpeedX = 0f;
    [SerializeField] [Range(0, 1)] float ConeShootSoundVolume = 0.5f;
    [SerializeField] AudioClip ConeShootSound;

    // Attack Cone config
    [Header("Attack Cone Two")]
    [SerializeField] GameObject ConeProjectileTwo;
    [SerializeField] float ConeShotDelayTwo = 1f;
    float ConeTimerTwo;
    [SerializeField] float ConeProjectileOneSpeedYTwo = 5f;
    [SerializeField] float ConeProjectileOneSpeedXTwo = 0f;
    [SerializeField] float ConeProjectileTwoSpeedYTwo = 5f;
    [SerializeField] float ConeProjectileTwoSpeedXTwo = 0f;
    [SerializeField] [Range(0, 1)] float ConeShootSoundVolumeTwo = 0.5f;
    [SerializeField] AudioClip ConeShootSoundTwo;


    // bools
    bool fightStarted = true;
    bool phaseOne = false;
    bool phaseTwo = false;
    bool phaseThree = false;
    bool phaseOneTransition = false;
    bool phaseTwoTransition = false;
    bool transitionTwoAddsSpawned = false;
    bool spawnWaypointWave = true;


    //positions
    Vector3 centerPos = new Vector3(0, 0, 0);
    Vector3 startPos = new Vector3(0, 3, 0);
    Vector3 bossPos;
    Vector3 conePosOne;
    Vector3 conePosTwo;

    //AddSpawners
    AddSpawner phaseTwoAdds;
    AddSpawner transitionTwoAdds;
    AddSpawner phaseThreeAdds;

    //phase paths
    List<Transform> waypointsPhaseOne;
    int waypointIndexPhaseOne = 0;
    List<Transform> waypointsPhaseTwo;
    int waypointIndexPhaseTwo = 0;
    List<Transform> waypointsPhaseThree;
    int waypointIndexPhaseThree = 0;

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
        phaseTwoAdds = (AddSpawner)GameObject.Find("Phase Two Adds").GetComponent(typeof(AddSpawner));
        transitionTwoAdds = (AddSpawner)GameObject.Find("Transition Two Adds").GetComponent(typeof(AddSpawner));
        phaseThreeAdds = (AddSpawner)GameObject.Find("Phase Three Adds").GetComponent(typeof(AddSpawner));

        waypointsPhaseOne = GetWaypoints(phaseOnePath);
        waypointsPhaseTwo = GetWaypoints(phaseTwoPath);
        waypointsPhaseThree = GetWaypoints(phaseThreePath);

        phaseThreeAddsTimer = phaseThreeAddsDelay;
        transform.position = startPos;
    }


    // Update is called once per frame
    void Update()
    {
        bossPos = GetBossPos();
        conePosOne = GetConePosOne();
        conePosTwo = GetConePosTwo();

        if (fightStarted == true)
        {
            StartCoroutine(PhaseOneStart());
        }

        if (phaseOne == true)
        {
            MovePhaseOne();
            AttackCone();
            AttackBomb();
        }

        if (health == phaseTwoHP)
        {
            phaseOneTransition = true;
            phaseOne = false;
        }

        if (phaseOneTransition == true)
        {
            MoveCenter();
            if (transform.position == centerPos)
            {
                StartCoroutine(PhaseTwoStart());
                AttackAOE();
            }
        }

        if (phaseTwo == true)
        {
            MovePhaseTwo();
        }

        if (health == phaseThreeHP)
        {
            phaseTwo = false;
            phaseTwoTransition = true;
        }

        if (phaseTwoTransition == true)
        {
            MoveCenter();
            if (transform.position == centerPos)
            {
                StartCoroutine(PhaseThreeStart());
                AttackAOE();
                if (transitionTwoAddsSpawned == false)
                {
                    StartCoroutine(transitionTwoAdds.SpawnAllWaves());
                    transitionTwoAddsSpawned = true;
                }
            }
                
        }

        if (phaseThree == true)
        {
            MovePhaseThree();
            AttackConeTwo();
            AttackBombTwo();
        }
    }

    private IEnumerator PhaseOneStart()
    {
        yield return new WaitForSeconds(prephaseTimer);
        fightStarted = false;
        phaseOne = true;
    }

    private IEnumerator PhaseTwoStart()
    {
        yield return new WaitForSeconds(transitionOneTimer);
        phaseTwo = true;
        phaseOneTransition = false;
    }

    private IEnumerator PhaseThreeStart()
    {
        yield return new WaitForSeconds(transitionTwoTimer);
        phaseThree = true;
        phaseTwoTransition = false;
    }


    private void MovePhaseOne()
    {
        if (waypointIndexPhaseOne <= waypointsPhaseOne.Count - 1)
        {
            var targetPosition = waypointsPhaseOne[waypointIndexPhaseOne].transform.position;
            var movementThisFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards
                (transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndexPhaseOne++;
            }
        }
        else
        {
            waypointIndexPhaseOne = 0;
        }
    }

    private void MovePhaseTwo()
    {
        if (waypointIndexPhaseTwo <= waypointsPhaseTwo.Count - 1)
        {
            var targetPosition = waypointsPhaseTwo[waypointIndexPhaseTwo].transform.position;
            var movementThisFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards
                (transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                if (spawnWaypointWave == true)
                {
                    StartCoroutine(WaitOnWaypointAndSpawn());
                    spawnWaypointWave = false;
                }
            }
        }
        else
        {
            waypointIndexPhaseTwo = 0;
        }
    }

    private IEnumerator WaitOnWaypointAndSpawn()
    {
        StartCoroutine(phaseTwoAdds.SpawnAllWaves());
        yield return new WaitForSeconds(waypointWaitTime);
        waypointIndexPhaseTwo++;
        spawnWaypointWave = true;
    }

    private void MovePhaseThree()
    {
        if (waypointIndexPhaseThree <= waypointsPhaseThree.Count - 1)
        {
            var targetPosition = waypointsPhaseThree[waypointIndexPhaseThree].transform.position;
            var movementThisFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards
                (transform.position, targetPosition, movementThisFrame);
            phaseThreeAddsTimer -= Time.deltaTime;
            if (phaseThreeAddsTimer <= 0)
            {
                StartCoroutine(phaseThreeAdds.SpawnAllWaves());
                phaseThreeAddsTimer = phaseThreeAddsDelay;
            }
            if (transform.position == targetPosition)
            {
                waypointIndexPhaseThree++;
            }
        }
        else
        {
            waypointIndexPhaseThree = 0;
        }
    }

    private void MoveCenter()
    {
        var movementThisFrame = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, centerPos, movementThisFrame);
    }

    private Vector3 GetBossPos()
    {
        return transform.position;
    }


    private List<Transform> GetWaypoints(GameObject path) //dodaj parameter za path
    {
        var bossWaypoints = new List<Transform>();
        foreach (Transform child in path.transform)
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
            Fire(AOEProjectile, AOEProjectileOneSpeedX, AOEProjectileOneSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileTwoSpeedX, AOEProjectileTwoSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileThreeSpeedX, AOEProjectileThreeSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileFourSpeedX, AOEProjectileFourSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileOneSpeedX, -AOEProjectileOneSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileTwoSpeedX, -AOEProjectileTwoSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileThreeSpeedX, -AOEProjectileThreeSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileFourSpeedX, -AOEProjectileFourSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            AOETimer = AOEShotDelay;
        }
    }

    private void AttackBomb()
    {
        BombTimer -= Time.deltaTime;
        if (BombTimer <= 0)
        {
            Fire(BombProjectile, BombProjectileOneSpeedX, BombProjectileOneSpeedY, BombShootSound, BombShootSoundVolume, bossPos);
            BombTimer = BombShotDelay;
        }
    }

    private void AttackBombTwo()
    {
        BombTimerTwo -= Time.deltaTime;
        if (BombTimerTwo <= 0)
        {
            Fire(BombProjectileTwo, BombProjectileOneSpeedXTwo, BombProjectileOneSpeedYTwo, BombShootSoundTwo, BombShootSoundVolumeTwo, bossPos);
            BombTimerTwo = BombShotDelayTwo;
        }
    }


    private void AttackCone()
    {
        ConeTimer -= Time.deltaTime;
        if (ConeTimer <= 0)
        {
            Fire(ConeProjectile, ConeProjectileOneSpeedX, ConeProjectileOneSpeedY, ConeShootSound, ConeShootSoundVolume, conePosOne);
            Fire(ConeProjectile, ConeProjectileTwoSpeedX, ConeProjectileTwoSpeedY, ConeShootSound, ConeShootSoundVolume, conePosOne);
            Fire(ConeProjectile, -ConeProjectileOneSpeedX, ConeProjectileOneSpeedY, ConeShootSound, ConeShootSoundVolume, conePosTwo);
            Fire(ConeProjectile, -ConeProjectileTwoSpeedX, ConeProjectileTwoSpeedY, ConeShootSound, ConeShootSoundVolume, conePosTwo);
            ConeTimer = ConeShotDelay;
        }
    }

    private void AttackConeTwo()
    {
        ConeTimerTwo -= Time.deltaTime;
        if (ConeTimerTwo <= 0)
        {
            Fire(ConeProjectileTwo, ConeProjectileOneSpeedXTwo, ConeProjectileOneSpeedYTwo, ConeShootSoundTwo, ConeShootSoundVolumeTwo, conePosOne);
            Fire(ConeProjectileTwo, ConeProjectileTwoSpeedXTwo, ConeProjectileTwoSpeedYTwo, ConeShootSoundTwo, ConeShootSoundVolumeTwo, conePosOne);
            Fire(ConeProjectileTwo, -ConeProjectileOneSpeedXTwo, ConeProjectileOneSpeedYTwo, ConeShootSoundTwo, ConeShootSoundVolumeTwo, conePosTwo);
            Fire(ConeProjectileTwo, -ConeProjectileTwoSpeedXTwo, ConeProjectileTwoSpeedYTwo, ConeShootSoundTwo, ConeShootSoundVolumeTwo, conePosTwo);
            ConeTimerTwo = ConeShotDelayTwo;
        }
    }

    private Vector3 GetConePosOne()
    {
        return transform.position + new Vector3(coneOffsetX, coneOffsetY, 0);
    }

    private Vector3 GetConePosTwo()
    {
        return transform.position + new Vector3(-coneOffsetX, coneOffsetY, 0);
    }


    private void Fire(GameObject projectile, float speedX, float speedY, AudioClip shotSound, float shotSoundVolume, Vector3 position)
    {
        GameObject laser = Instantiate(
            projectile,
            position,
            Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, -speedY);
        AudioSource.PlayClipAtPoint(shotSound, Camera.main.transform.position, shotSoundVolume);
    }


    
/*
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


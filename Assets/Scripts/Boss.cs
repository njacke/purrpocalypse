﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float health = 4500f;
    float startHealth;

    [Header("Sprites")]
    [SerializeField] Sprite[] bossSprites;
    float nextSpriteHP;
    int spriteIndex = 0;

    // Phases
    [Header("Phases")]
    [SerializeField] float phaseTwoHP = 3000f;
    [SerializeField] float phaseThreeHP = 1500f;
    [SerializeField] float spriteDiffHP = 500f;
    [SerializeField] float prephaseTimer = 10f;
    [SerializeField] GameObject phaseOnePath;
    [SerializeField] float transitionOneTimer = 5f;
    [SerializeField] GameObject phaseTwoPath;
    [SerializeField] float waypointWaitTime = 2f;
    [SerializeField] float transitionTwoTimer = 5f;
    [SerializeField] GameObject phaseThreePath;
    [SerializeField] float phaseThreeAddsStart = 1f;
    [SerializeField] float phaseThreeAddsDelay = 3f;
    float phaseThreeAddsTimer;

    //Easy
    [Header("Easy Mode")]
    [SerializeField] float easyHealth = 2700f;
    [SerializeField] float easyPhaseTwoHP = 1800f;
    [SerializeField] float easyPhaseThreeHP = 900f;
    [SerializeField] float easySpriteDiffHP = 300f;


    // VFX and sound effects
    [Header("Effects")]
    [SerializeField] AudioClip battleStartSound;
    [SerializeField] [Range(0, 1)] float battleStartSoundVolume;
    [SerializeField] AudioClip catSpawnSound;
    [SerializeField] [Range(0, 1)] float catSpawnSoundVolume;
    [SerializeField] AudioClip breakSound;
    [SerializeField] [Range(0, 1)] float breakSoundVolume;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume;
    [SerializeField] float destroyDelay = 3f;




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
    [SerializeField] float AOEProjectileFiveSpeedY = 5f;
    [SerializeField] float AOEProjectileFiveSpeedX = 0f;
    [SerializeField] float AOEProjectileSixSpeedY = 5f;
    [SerializeField] float AOEProjectileSixSpeedX = 0f;
    [SerializeField] float AOEProjectileSevenSpeedY = 5f;
    [SerializeField] float AOEProjectileSevenSpeedX = 0f;
    [SerializeField] float AOEProjectileEightSpeedY = 5f;
    [SerializeField] float AOEProjectileEightSpeedX = 0f;
    [SerializeField] float AOEProjectileNineSpeedY = 5f;
    [SerializeField] float AOEProjectileNineSpeedX = 0f;
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


    // bools
    bool prePhase = true;
    bool phaseOne = false;
    bool phaseTwo = false;
    bool phaseThree = false;
    bool phaseOneTransition = false;
    bool phaseOneTransitionCalled = false;
    bool phaseTwoTransition = false;
    bool phaseTwoTransitionCalled = false;
    bool spawnWaypointWave = true;
    bool battleStartSoundPlayed = false;
    bool bossAlive = true;
    bool destroyTriggered = false;



    //positions
    Vector3 centerPos = new Vector3(0, 0, 0);
    Vector3 startPos = new Vector3(0, 3, 0);
    Vector3 bossPos;
    Vector3 conePosOne;
    Vector3 conePosTwo;

    //AddSpawners
    AddSpawner phaseTwoAdds;
    AddSpawner phaseThreeAdds;

    //phase paths
    List<Transform> waypointsPhaseOne;
    int waypointIndexPhaseOne = 0;
    List<Transform> waypointsPhaseTwo;
    int waypointIndexPhaseTwo = 0;
    List<Transform> waypointsPhaseThree;
    int waypointIndexPhaseThree = 0;


    // Start is called before the first frame update
    void Start()
    {
        phaseTwoAdds = (AddSpawner)GameObject.Find("Phase Two Adds").GetComponent(typeof(AddSpawner));
        phaseThreeAdds = (AddSpawner)GameObject.Find("Phase Three Adds").GetComponent(typeof(AddSpawner));

        waypointsPhaseOne = GetWaypoints(phaseOnePath);
        waypointsPhaseTwo = GetWaypoints(phaseTwoPath);
        waypointsPhaseThree = GetWaypoints(phaseThreePath);

        phaseThreeAddsTimer = phaseThreeAddsStart;
        transform.position = startPos;
        startHealth = health;

        if (FindObjectOfType<Difficulty>().EasyDifficulty() == true)
        {
            startHealth = easyHealth;
            phaseTwoHP = easyPhaseTwoHP;
            phaseThreeHP = easyPhaseThreeHP;
            spriteDiffHP = easySpriteDiffHP;
        }

        nextSpriteHP = startHealth - spriteDiffHP;
    }


    // Update is called once per frame
    void Update()
    {
        bossPos = GetBossPos();
        conePosOne = GetConePosOne();
        conePosTwo = GetConePosTwo();

        ShowSprite();

        if (prePhase == true)
        {
            StartCoroutine(PhaseOneStart());
            if (battleStartSoundPlayed == false)
            {
                
                AudioSource.PlayClipAtPoint(battleStartSound, Camera.main.transform.position, battleStartSoundVolume);
                battleStartSoundPlayed = true;
            }
        }

        if (phaseOne == true)
        {
            MovePhaseOne();
            AttackCone();
            AttackBomb();
        }

        if (health <= phaseTwoHP)
        {
            if (phaseOneTransitionCalled == false)
            {
                phaseOne = false;
                phaseOneTransition = true;
                phaseOneTransitionCalled = true;
            }
        }

        if (phaseOneTransition == true)
        {
            MoveCenter();
            if (transform.position == centerPos)
            {
                AttackAOE();
                StartCoroutine(PhaseTwoStart());
            }
        }

        if (phaseTwo == true)
        {
            MovePhaseTwo();
        }

        if (health <= phaseThreeHP)
        {
            if (phaseTwoTransitionCalled == false)
            {
                phaseTwo = false;
                phaseTwoTransition = true;
                phaseTwoTransitionCalled = true;
            }
        }

        if (phaseTwoTransition == true)
        {
            MoveCenter();
            if (transform.position == centerPos)
            {
                AttackAOE();
                StartCoroutine(PhaseThreeStart());
            }
                
        }

        if ((phaseThree == true) && (bossAlive == true))
        {
            MovePhaseThree();
            AttackCone();
            AttackBomb();
        }
    }

    private void ShowSprite()
    {
        if (health <= nextSpriteHP)
        {
            GetComponent<SpriteRenderer>().sprite = bossSprites[spriteIndex];
            if (spriteIndex != bossSprites.Length - 1)
            {
                spriteIndex += 1;
                AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position, breakSoundVolume);
            }
            nextSpriteHP -= spriteDiffHP;
        }

    }



        private IEnumerator PhaseOneStart()
    {
        prePhase = false;
        yield return new WaitForSeconds(prephaseTimer);
        health = startHealth;
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
                    AudioSource.PlayClipAtPoint(catSpawnSound, Camera.main.transform.position, catSpawnSoundVolume);
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
            {
                if (phaseThreeAddsTimer <= 0)
                {
                    StartCoroutine(phaseThreeAdds.SpawnAllWaves());
                    AudioSource.PlayClipAtPoint(catSpawnSound, Camera.main.transform.position, catSpawnSoundVolume);
                    phaseThreeAddsTimer = phaseThreeAddsDelay;
                }
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
        phaseThree = false;
        bossAlive = false;

        if (destroyTriggered == false)
        {
            StartCoroutine(DestroyDelay());
            destroyTriggered = true;
        }

        GameObject explosion = Instantiate(deathVFX, transform.position , transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);

        FindObjectOfType<LevelLoading>().LoadVictory();
    }

    private IEnumerator DestroyDelay()
    {
        yield return  new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }


    private void AttackAOE()
    {
        AOETimer -= Time.deltaTime;
        if (AOETimer <= 0)
        {
            Fire(AOEProjectile, AOEProjectileOneSpeedX, AOEProjectileOneSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileOneSpeedX, -AOEProjectileOneSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileTwoSpeedX, AOEProjectileTwoSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileTwoSpeedX, -AOEProjectileTwoSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);

            Fire(AOEProjectile, AOEProjectileThreeSpeedX, AOEProjectileThreeSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileThreeSpeedX, -AOEProjectileThreeSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileThreeSpeedX, -AOEProjectileThreeSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileThreeSpeedX, AOEProjectileThreeSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);

            Fire(AOEProjectile, -AOEProjectileFourSpeedX, -AOEProjectileFourSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileFourSpeedX, AOEProjectileFourSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileFourSpeedX, -AOEProjectileFourSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileFourSpeedX, AOEProjectileFourSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);

            Fire(AOEProjectile, AOEProjectileFiveSpeedX, AOEProjectileFiveSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileFiveSpeedX, -AOEProjectileFiveSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileFiveSpeedX, -AOEProjectileFiveSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileFiveSpeedX, AOEProjectileFiveSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);


            Fire(AOEProjectile, AOEProjectileSixSpeedX, AOEProjectileSixSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileSixSpeedX, -AOEProjectileSixSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileSixSpeedX, -AOEProjectileSixSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileSixSpeedX, AOEProjectileSixSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);

            Fire(AOEProjectile, AOEProjectileSevenSpeedX, AOEProjectileSevenSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileSevenSpeedX, -AOEProjectileSevenSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileSevenSpeedX, -AOEProjectileSevenSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileSevenSpeedX, AOEProjectileSevenSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);

            Fire(AOEProjectile, AOEProjectileEightSpeedX, AOEProjectileEightSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileEightSpeedX, -AOEProjectileEightSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileEightSpeedX, -AOEProjectileEightSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileEightSpeedX, AOEProjectileEightSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);

            Fire(AOEProjectile, AOEProjectileNineSpeedX, AOEProjectileNineSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileNineSpeedX, -AOEProjectileNineSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, AOEProjectileNineSpeedX, -AOEProjectileNineSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);
            Fire(AOEProjectile, -AOEProjectileNineSpeedX, AOEProjectileNineSpeedY, AOEShootSound, AOEShootSoundVolume, bossPos);

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

    public bool BossAlive()
    {
        return bossAlive;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int health = 200;
    [SerializeField] float padding = 1f;
    [SerializeField] float paddingTop = 1f;


    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    [Header("Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume;
    [SerializeField] float hitVisualTime = 0.05f;
    bool visualHitTrigger = true;

    Coroutine firingCoroutine;

    LevelLoading leveLoading;

    float xMin;
    float xMax;
    float yMin;
    float yMax;
    
    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundries();
        leveLoading = FindObjectOfType<LevelLoading>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy" )
        {
            health = 0;
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
        if (visualHitTrigger == true)
        {
            visualHitTrigger = false;
            StartCoroutine(VisualHit());
        }

        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (health <= 0)
        {
           Die();
        }
    }

    private IEnumerator VisualHit()
        {
            var sprite = GetComponent<SpriteRenderer>();
            var originalColor = sprite.color;
            sprite.color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(hitVisualTime);
            sprite.color = originalColor;
            visualHitTrigger = true;
    }

    private void Die()
    {
        LoadGameOver();

        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    private void LoadGameOver()
    {
        string currentScene = leveLoading.GetCurrentScene();
        if (currentScene == "Level One")
        {
            leveLoading.LoadGameOverStory();
        }
        if (currentScene == "Boss")
        {
            leveLoading.LoadGameOverBoss();
        }
        if (currentScene == "Arena")
        {
            leveLoading.LoadGameOverArena();
        }
    }

    public int GetHealth()
    {
        if (health < 0)
        {
            health = 0;
        }
        return health;
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireCoutinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireCoutinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(
            laserPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - paddingTop;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make a prefab and place this on that
/// </summary>
public class Enemy : MonoBehaviour
{
    //TODO: Make enemies only shoot when visible
    private const int B_PLAYER = 0, B_RANDOM = 1;
    public int health = 10;

    public float speed = 10f;
    public float speedLimit = 100f;
    public float turnFactor = 5f;

    public int damage = 1; // maybe this will be relevant
    public float fireRate = 1f;
    public float projectileSpeed = 20f;

    private float fireTimer = 1f;
    public float behaviorTimer = 5f;
    public float moveTimer = 0.5f; 
    public float dropRate = 0.2f;
    public GameObject p;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private AudioClip clip;

    private int behaviorState;
    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer spiRend;
    bool isVisible;
    
    /* targets you, but has a hard time getting to you */

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.instance.player.transform;
        spiRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        fireTimer = 1/fireRate;
        StartCoroutine(Move());
        StartCoroutine(RandBehavior());
        StartCoroutine(Shoot());
        GameManager.instance.enemyCount++;
        int randInt = Random.Range(0, sprites.Length);
        spiRend.sprite = sprites[randInt];
        

    }

    // Update is called once per frame
    void Update()
    {
        
        
        isVisible = GameManager.instance.playerCam.thisCamera.IsObjectVisible(spiRend);
        switch(behaviorState) {
            case B_RANDOM:
                Vector2 randVect = Random.insideUnitCircle;
                float randRotation = -Mathf.Atan2(randVect.x, randVect.y) * 180/Mathf.PI;
                rb.rotation = randRotation;
                break;
            default:
                LookAt(target.position);
                break;
            
        }
        

    }


    /// <summary>
    /// Enemies have health, but they are meant to die in one hit.
    /// but I realized that a bit late.
    /// Still works.
    /// </summary>
    /// <param name="damage"></param>
    public void takeDamage(int damage) {
        health -= damage;

        if (health <= 0) {
            float randFloat = Random.Range(0f, 1f);
            if (randFloat < (dropRate + 0.1)) {
                PowerUp.SpawnPowerUp(transform.position);
            }
            AudioSource.PlayClipAtPoint(clip, transform.position);
            GameManager.instance.enemyCount--;
            Destroy(gameObject);
        }
    }

    
    /// <summary>
    /// To make the enemy point at the player, though maybe this should be changed
    /// baseline for staring at something is transform.up = transform.up, targetPos - transform.position
    /// </summary>
    /// <param name="targetPos"></param>
    private void LookAt(Vector3 targetPos) {
        // REMINDER: quick look at: transform.up = transform.up, targetPos - transform.position
        transform.up = Vector2.Lerp(transform.up, targetPos - transform.position, Time.deltaTime * turnFactor);
    }

    
    /// <summary>
    /// Randomly determine a behavior for the enemy
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandBehavior() {
        while(true) {  
            int rand = Random.Range(B_PLAYER, (B_RANDOM + 1));
            behaviorState = rand;
            // Debug.Log("I am now tracking: " + rand);
            switch(rand) {
                default:
                    target = GameManager.instance.player.transform;
                    break;
                
            }
            yield return new WaitForSeconds(behaviorTimer);
            //yield return new WaitForSeconds(behaviorTimer);
        }
    }

    /// <summary>
    /// Always move towards the object's up vector.
    /// Speed should be regarded as a "step" per move
    /// </summary>
    /// <returns></returns>
    private IEnumerator Move() {
        while(true) {
            Vector2 forceVector;
            switch(behaviorState) {
                case B_RANDOM:
                    // move randomly forward
                    forceVector = transform.up * Random.Range(0f, speed*0.5f);                
                    break;  
                default:
                    forceVector= transform.up * speed;
                    break;
                          
            }
            rb.AddForce(forceVector, ForceMode2D.Impulse);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speedLimit);
            yield return new WaitForSeconds(moveTimer);
        }
    }

    /// <summary>
    /// Shoot coroutine.
    /// Has less to care about than player
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shoot() {
        while(true) {
            if (isVisible) {
                GameObject pGameObject = Instantiate(p, transform.position, transform.rotation);
                Projectile projectile = pGameObject.GetComponent<Projectile>();
                projectile.damage = damage;
                projectile.speed = projectileSpeed;
                projectile.origin = Projectile.Origin.ENEMY;
            }
            yield return new WaitForSeconds(fireTimer);
        }
    }
}

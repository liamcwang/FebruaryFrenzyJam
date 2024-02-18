using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const int B_PLAYER = 0, B_ALLIES = 1, B_RANDOM = 2;
    public int health = 10;
    public float speed = 10f;
    public float speedLimit = 100f;
    public float fireRate = 1f;
    private float fireTimer = 1f;
    public int damage = 1;
    public float projectileSpeed = 20f;
    public float behaviorTimer = 5f;
    public float moveTimer = 0.5f; 
    public float turnFactor = 5f;
    public GameObject p;
    [SerializeField] private GameObject powUpPrefab;
    [SerializeField] private Sprite[] sprites;
    private int behaviorState;
    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer spiRend;
    
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

    void OnDestroy() {
        GameManager.instance.enemyCount--;    
    }

    public void takeDamage(int damage) {
        health -= damage;

        if (health <= 0) {
            PowerUp.SpawnPowerUp(transform.position);
            Destroy(gameObject);
        }
    }

    private void LookAt(Vector3 targetPos) {
        transform.up = Vector2.Lerp(transform.up, targetPos - transform.position, Time.deltaTime * turnFactor);
    }

    

    private IEnumerator RandBehavior() {
        while(true) {  
            
            int rand = Random.Range(B_PLAYER, (B_RANDOM + 1));
            behaviorState = rand;
            Debug.Log("I am now tracking: " + rand);
            switch(rand) {
                case B_ALLIES:
                    break;
                default:
                    target = GameManager.instance.player.transform;
                    break;
                
            }
            yield return new WaitForSeconds(behaviorTimer);
            //yield return new WaitForSeconds(behaviorTimer);
        }
    }

    private IEnumerator Move() {
        while(true) {
            Vector2 forceVector;
            switch(behaviorState) {
                case B_RANDOM:
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

    private IEnumerator Shoot() {
        while(true) {
            GameObject pGameObject = Instantiate(p, transform.position, transform.rotation);
            Projectile projectile = pGameObject.GetComponent<Projectile>();
            projectile.damage = damage;
            projectile.speed = projectileSpeed;
            projectile.origin = Projectile.Origin.ENEMY;
            yield return new WaitForSeconds(fireTimer);
        }
    }
}

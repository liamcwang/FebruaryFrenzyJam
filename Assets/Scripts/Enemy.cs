using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const int B_PLAYER = 0, B_ALLIES = 1, B_RANDOM = 2;
    public int health = 10;
    public float speed = 10f;
    public float speedLimit = 20f;
    public float fireRate = 1f;
    private float fireTimer = 1f;
    public int damage = 1;
    public float projectileSpeed = 20f;
    public float behaviorTimer = 5f;
    public float moveTimer = 0.5f; 
    public float turnFactor = 5f;
    public GameObject p;
    private int behaviorState;
    private Transform target;
    private Rigidbody2D rb;
    /* targets you, but has a hard time getting to you */

    // Start is called before the first frame update
    void Start()
    {
        target = Player.instance.transform;
        rb = GetComponent<Rigidbody2D>();
        fireTimer = 1/fireRate;
        StartCoroutine(Move());
        StartCoroutine(RandBehavior());
    }

    // Update is called once per frame
    void Update()
    {
        
        switch(behaviorState) {
            case B_RANDOM:
                Vector3 randPos = Random.insideUnitCircle;
                Vector2 randVect = Random.insideUnitCircle;
                float randRotation = -Mathf.Atan2(randVect.x, randVect.y) * 180/Mathf.PI;
                rb.rotation = randRotation;
                break;
            default:
                LookAt(target.position);
                break;
            
        }
        

    }

    void FixedUpdate() {

    }

    public void takeDamage(int damage) {
        health -= damage;

        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    private void LookAt(Vector3 targetPos) {
        transform.up = Vector2.Lerp(transform.up, targetPos - transform.position, Time.deltaTime * turnFactor);
    }

    

    private IEnumerator RandBehavior() {
        while(true) {  
            yield return new WaitForSeconds(behaviorTimer);
            int rand = Random.Range(B_PLAYER, (B_RANDOM + 1));
            behaviorState = rand;
            Debug.Log("I am now tracking: " + rand);
            switch(rand) {
                case B_ALLIES:
                    break;
                default:
                    target = Player.instance.transform;
                    break;
                
            }
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

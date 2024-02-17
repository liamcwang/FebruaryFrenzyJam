using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    
    public float speed = 10f;
    public float speedLimit = 100f;
    public int health = 100;
    public int defense = 10;
    public float moveTimer = 0.5f; 
    public float turnFactor = 5f;
    public float spawnTimer = 120f;
    private Rigidbody2D rb;
    private Transform target;

    void Awake() {
        GameManager.instance.boss = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameManager.instance.player.transform;
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        LookAt(target.position);
    }

    private void LookAt(Vector3 targetPos) {
        transform.up = Vector2.Lerp(transform.up, targetPos - transform.position, Time.deltaTime * turnFactor);
    }

    public void takeDamage(int damage) {
        health -= damage - defense;

        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("I hit: " + other);

        if (other.CompareTag("Player")) {
            Player player = GameManager.instance.player;
            player.die();
        }
    }

    

    private IEnumerator Move() {
        while(true) {
            Vector2 forceVector;
            
            forceVector= transform.up * speed;
                          
            rb.AddForce(forceVector, ForceMode2D.Impulse);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speedLimit);
            yield return new WaitForSeconds(moveTimer);
        }
    }
}

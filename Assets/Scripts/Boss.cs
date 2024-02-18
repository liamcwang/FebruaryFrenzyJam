using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    
    public float speed = 10f;
    public float minSpeed = 5f;
    public float maxSpeed = 100f;
    public int health = 100;
    public int defense = 10;
    public float moveTimer = 0.5f; 
    public float turnFactor = 5f;
    public float spawnTimer = 120f;
    public float scanTimer = 5f;
    public int screamFrequency = 10;
    private bool asleep = true;
    private Rigidbody2D rb;
    private Transform target;
    public AudioClip clip;
    public AudioSource audioSaus;

    void Awake() {
        GameManager.instance.boss = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSaus = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        target = GameManager.instance.player.transform;
        StartCoroutine(Scanning());
        StartCoroutine(BossTimer());
        
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
            GameManager.Victory();
        }
    }

    public void debuff(Tower.Debuff effect, float value) {
        Debug.Log("Alas, I am debuffed! " + effect + ": " + value);
        switch(effect) {
            case Tower.Debuff.HP:
                health -= (int)value;
                break;
            case Tower.Debuff.DEF:
                defense -= (int)value;
                break;
            case Tower.Debuff.SPEED:
                speed -= value;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("I hit: " + other);

        if (other.CompareTag("Player")) {
            Player player = GameManager.instance.player;
            player.die();
        }
    }

    private IEnumerator Scanning(){
        while (asleep) {
            yield return new WaitForSeconds(5f);
            PlayerCam.instance.PlaySound(PlayerCam.SCANNING);
        }
        
    }

    private IEnumerator Move() {
        int moveCount = 1;
        while(true) {
            moveCount = moveCount % screamFrequency;
            if (moveCount == 0) {
                audioSaus.Play(0);
            }
            
            Vector2 forceVector;
            
            forceVector= transform.up * speed;
            
                          
            rb.AddForce(forceVector, ForceMode2D.Impulse);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            yield return new WaitForSeconds(moveTimer);
        }
    }

    private IEnumerator BossTimer() {
        yield return new WaitForSeconds(spawnTimer);
        asleep = false;
        PlayerCam.instance.PlaySound(PlayerCam.BOSS_THEME);
        StartCoroutine(Move());
    }

    

    
}

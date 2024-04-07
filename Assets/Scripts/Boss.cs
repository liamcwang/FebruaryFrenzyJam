using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Le boss, be sure to place an object with this script on the map
/// Special to the boss are the timers used to decide when it should be
/// active
/// </summary>
public class Boss : MonoBehaviour
{
    public int health = 100;
    public int defense = 10;

    public float speed = 10f;
    public float minSpeed = 5f;
    public float maxSpeed = 100f;
    public float turnFactor = 5f;

    public float moveTimer = 0.5f; 
    public float spawnTimer = 120f;
    public float scanTimer = 5f;
    public int screamFrequency = 10; // as in, the number of moves it takes to scream

    private bool asleep = true;
    private Rigidbody2D rb;
    private Transform target;
    public AudioClip clip;
    public AudioSource audioSaus;
    public Animator anim;
    private EnemySpawner spawner;
    private float initialHealth;

    void Awake() {
        GameManager.instance.boss = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSaus = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spawner = GetComponent<EnemySpawner>();
        spawner.enabled = false;
        target = GameManager.instance.player.transform;
        StartCoroutine(Scanning());
        StartCoroutine(BossTimer());
        initialHealth = health;
        
    }

    // Update is called once per frame
    void Update()
    {
        float calc = health/initialHealth * 100f;
        anim.SetFloat("BossHealth", calc);
        LookAt(target.position);
    }

    /// <summary>
    /// To make the boss point at the player, though maybe this should be changed
    /// baseline for staring at something is transform.up = transform.up, targetPos - transform.position
    /// </summary>
    /// <param name="targetPos"></param>
    private void LookAt(Vector3 targetPos) {
        transform.up = Vector2.Lerp(transform.up, targetPos - transform.position, Time.deltaTime * turnFactor);
    }

    /// <summary>
    /// Special to the boss is the defense value, which reduces incoming damage
    /// Thing is, doesn't it affect the effective health of the boss the same way as health anyway?
    /// </summary>
    /// <param name="damage"></param>
    public void takeDamage(int damage) {
        int diff = damage - defense;

        if (diff > 0) {
            health -= diff;
        }
        

        if (health <= 0) {
            Destroy(gameObject);
            GameManager.Victory();
        }
    }

    /// <summary>
    /// Reduce the value of the boss's stats, based on the effect given
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="value"></param>
    public void debuff(Tower.Debuff effect, float value) {
        Debug.Log("Alas, I am debuffed! " + effect + ": " + value);
        switch(effect) {
            case Tower.Debuff.HP:
                health -= (int) (initialHealth * value);
                break;
            case Tower.Debuff.TIMER:
                spawnTimer += value;
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

    /// <summary>
    /// While the boss timer is running, this will play the scanning sound
    /// We play it on the playercam, because that's where the listener is
    /// </summary>
    /// <returns></returns>
    private IEnumerator Scanning(){
        while (asleep) {
            yield return new WaitForSeconds(5f);
            if (asleep) {
                PlayerCam.instance.PlaySound(PlayerCam.SCANNING);
            }
        }
        
    }

    /// <summary>
    /// Always move towards the object's up vector.
    /// Speed should be regarded as a "step" per move
    /// </summary>
    /// <returns></returns>
    private IEnumerator Move() {
        int moveCount = 0;
        while(true) {
            moveCount++;
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

    /// <summary>
    /// Begins when the boss is loaded
    /// </summary>
    /// <returns></returns>
    private IEnumerator BossTimer() {
        float currentTimer = 0;
        while (currentTimer < spawnTimer) {
            yield return new WaitForSeconds(1);
            currentTimer += 1;
            GameManager.instance.mainMenu.SetBossTimer(currentTimer, spawnTimer);
        }
        asleep = false;
        PlayerCam.instance.BossTime();
        spawner.enabled = true;

        StartCoroutine(Move());
    }

    

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectiles in game are built from here.
/// Super modular foundation to have a bunch of different functions 
/// with reused code
/// </summary>
public class Projectile : MonoBehaviour
{
    public enum Origin{PLAYER, ENEMY, BOSS, VOID}; // Void was added, incase we wanted rogue projectiles
    public enum Behavior{DEFAULT, HOMING};
    public float speed = 20;
    public int damage = 1;
    public Origin origin = Origin.VOID;
    public Behavior behaviorState;
    public LayerMask ignoreLayer;
    public float decayTimer = 5f;
    [SerializeField] private Rigidbody2D rb;
    public SpriteRenderer spiRend;
    [SerializeField] private Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        spiRend.sprite = sprites[(int) origin];
        rb.velocity = transform.up * speed;
        if (decayTimer > 0) { // this means we can disable the decay from happening
            StartCoroutine(Decay(decayTimer));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        /*if (behaviorState == Behavior.HOMING) {
            
        }*/
        
    }

    
    /// <summary>
    /// Coroutine to destroy the projectile after some time
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    private IEnumerator Decay(float f) {
        yield return new WaitForSeconds(f);
        Destroy(gameObject);
    }

    /// <summary>
    /// Projectiles are responsible for calling damage-related functions 
    /// on other objects.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Hit!");
        if (origin != Origin.ENEMY && origin != Origin.BOSS) {
            GameObject gObj = other.gameObject;
            // lotta room for improvement here
            // Could make a for loop for every type of damageable object
            if (gObj.GetComponent<Enemy>() != null) {
                gObj.GetComponent<Enemy>().takeDamage(damage);
                Destroy(gameObject);
            }
            if (gObj.GetComponent<Tower>() != null ) {
                gObj.GetComponent<Tower>().takeDamage(damage);
                Destroy(gameObject);
            }
            if (gObj.GetComponent<Boss>() != null) {
                gObj.GetComponent<Boss>().takeDamage(damage);
                Destroy(gameObject);
            }
        }
        
        if (origin != Origin.PLAYER) {
            Player p = other.gameObject.GetComponent<Player>();
            if (p != null) {
                p.die();
                Destroy(gameObject);
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum Origin{VOID, PLAYER, ENEMY};
    public enum Behavior{DEFAULT, HOMING};
    public float speed = 20;
    public int damage = 1;
    public Origin origin = Origin.VOID;
    public Behavior behaviorState;
    public LayerMask ignoreLayer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spiRend;
    [SerializeField] private Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
        /*if (behaviorState == Behavior.HOMING) {
            
        }*/
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Hit!");
        if (origin != Origin.ENEMY) {
            GameObject gabe = other.gameObject;
            // lotta room for improvement here
            if (gabe.GetComponent<Enemy>() != null) {
                gabe.GetComponent<Enemy>().takeDamage(damage);
                Destroy(gameObject);
            }
            if (gabe.GetComponent<Tower>() != null ) {
                gabe.GetComponent<Tower>().takeDamage(damage);
                Destroy(gameObject);
            }
            if (gabe.GetComponent<Boss>() != null) {
                gabe.GetComponent<Boss>().takeDamage(damage);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum Origin{VOID, PLAYER, ENEMY};
    public float speed = 20;
    public int damage = 1;
    public Origin origin = Origin.VOID;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * speed;
    }

    void OnBecameInvisible()
    {
        // Debug.Log("bye");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Hit!");
        if (origin != Origin.ENEMY) {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (e != null) {
                e.takeDamage(damage);
                Destroy(gameObject);
            }
        }
        if (origin != Origin.PLAYER) {
            Player p = other.gameObject.GetComponent<Player>();
            if (p != null) {
                p.takeDamage(damage);
                Destroy(gameObject);
            }
        }
        
    }
}

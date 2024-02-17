using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum States {};
    public int health = 10;
    public float speed = 10f;
    private Transform target;
    private Rigidbody2D rb;
    /* targets you, but has a hard time getting to you */

    // Start is called before the first frame update
    void Start()
    {
        target = Player.instance.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.right = target.position - transform.position; // basic look at
    }

    public void takeDamage(int damage) {
        health -= damage;

        if (health <= 0) {
            Destroy(gameObject);
        }
    }


}

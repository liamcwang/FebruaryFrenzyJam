using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float maxSpeed = 10f;
    public float acceleration = 10f;
    public float rotationFactor = 15f;
    public float fireRate = 1f;
    public Projectile p;
    private float fireTimer = 1f;
    private bool alive = true;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fireTimer = 1/fireRate;
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxis("MoveHorizontal");
        float yMove = Input.GetAxis("MoveVertical");
        float xShoot = Input.GetAxis("ShootHorizontal");
        // float yShoot = Input.GetAxis("ShootVertical");
        Vector2 motionVector = Vector2.zero;

        
        if (xMove != 0 || yMove != 0) {
            motionVector = new Vector2 (xMove, yMove);
        }
        if (xShoot != 0) {
            float newAngle = -xShoot * rotationFactor;
            //float newAngle = -Mathf.Atan2(xShoot, yShoot) * 180/Mathf.PI;
            //newAngle = Mathf.Lerp(rb.rotation, newAngle, Time.deltaTime * rotationFactor);
            rb.rotation += newAngle * Time.deltaTime; 
        }
        

        rb.velocity = motionVector * maxSpeed;
    }

    private IEnumerator Shoot() {
        while(alive) {
            Instantiate(p, transform.position, transform.rotation);
            yield return new WaitForSeconds(fireTimer); //wait 2 seconds
        }
    }
}

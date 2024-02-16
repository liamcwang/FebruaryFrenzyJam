using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float maxSpeed = 10;
    public float acceleration = 10;
    public float rotationFactor = 15;
    public float fireRate = 1;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
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
        if (xShoot != 0 || yShoot != 0) {
            float newAngle = -xShoot * rotationFactor;
            //float newAngle = -Mathf.Atan2(xShoot, yShoot) * 180/Mathf.PI;
            //newAngle = Mathf.Lerp(rb.rotation, newAngle, Time.deltaTime * rotationFactor);
            rb.rotation += newAngle * Time.deltaTime; 
        }
        

        rb.velocity = motionVector * maxSpeed;
        


    }
}

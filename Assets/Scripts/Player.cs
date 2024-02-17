using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PowerUp{FIRE_RATE, DODGE, MORE_BULLETS, HOMING_SHOT, BEHIND_SHOT};
    public static Player instance;
    public float maxSpeed = 10f;
    // public float acceleration = 10f;
    public float rotationFactor = 15f;
    public float fireRate = 1f;
    public int health = 10;
    public int damage = 1;
    public float projectileSpeed = 20f;
    public GameObject p;
    private Dictionary<PowerUp, Effect> powUpDict;
    private float fireTimer = 1f;
    private bool alive = true;
    private Rigidbody2D rb;

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fireTimer = 1/fireRate;
        powUpDict = new Dictionary<PowerUp, Effect>();
        foreach (PowerUp pow in Enum.GetValues(typeof(PowerUp))) {
            powUpDict.Add(pow, new Effect(false, null));
        }
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

    private void updatePowers(PowerUp pow, Effect newEff) {
        powUpDict[pow] = newEff;
        switch(pow) {
            case PowerUp.FIRE_RATE:
                if (newEff.active) {
                    fireTimer = 1f/(fireRate* (float)newEff.magnitude);
                } else{ // return to old firerate if set to false
                    fireTimer = 1f/fireRate;
                }                
                break;
            default:
                // nothing needs to be updated
                break;
        }
    }

    private IEnumerator Shoot() {
        while(alive) {
            spawnProjectile(Instantiate(p, transform.position, transform.rotation), Projectile.Behavior.DEFAULT);


            if(powUpDict[PowerUp.MORE_BULLETS].active) {
                for (int i = 0; i < (int)powUpDict[PowerUp.MORE_BULLETS].magnitude; i++) {
                    int mod2 = i % 2;
                    int LeftRight =  (mod2 == 0) ? -1 : 1;
                    float offset = 15 * LeftRight;
                    spawnProjectile(Instantiate(p, transform.position, transform.rotation), Projectile.Behavior.DEFAULT);
                }
            }

            
            yield return new WaitForSeconds(fireTimer); 
        }
    }

    private void spawnProjectile(GameObject pGameObject, Projectile.Behavior PB) {
        Projectile projectile = pGameObject.GetComponent<Projectile>();
        projectile.damage = damage;
        projectile.speed = projectileSpeed;
        projectile.origin = Projectile.Origin.PLAYER;
        projectile.behaviorState = PB;
    }

    public void takeDamage(int damage) {
        health -= damage;

        if (health <= 0) {
            
        }
    }
    /*
    void OnBecameInvisible()
    {
        float xAxis = transform.position.x;
        float yAxis = transform.position.y;
        float zAxis = transform.position.z;

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        if(pos.x < 0f) {
            xAxis = topRight.x;
        }
        if(pos.x > 1f){
            xAxis = bottomLeft.x;
        } 
        if(pos.y < 0f){
            yAxis = topRight.y;
        } 
        if(pos.y > 1f){
            yAxis = bottomLeft.y;
        }
        
        transform.position = new Vector3(xAxis,yAxis,zAxis);
    }*/

    public class Effect {
        public bool active;
        public object magnitude;
        public Type t;

        public Effect(bool status, object o) {
            active = status;
            magnitude = o;
        }

    }

}
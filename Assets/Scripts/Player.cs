using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PowerUp{FIRE_RATE, DODGE, MORE_BULLETS, BEHIND_SHOT};
    public float maxSpeed = 10f;
    // public float acceleration = 10f;
    public float rotationFactor = 15f;
    public float fireRate = 1f;
    // public int health = 10;
    public int damage = 1;
    public float projectileSpeed = 20f;
    public GameObject p;
    public float dodgeRange = 20f;
    private Dictionary<PowerUp, Effect> powUpDict;
    private float fireTimer = 1f;
    private bool alive = true;
    private Rigidbody2D rb;
    public bool canShoot = true;
    public bool canDie = true;
    public AudioClip[] sounds;
    public AudioSource audioSaus;

    void Awake() {
        GameManager.instance.player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSaus = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        fireTimer = 1/fireRate;
        powUpDict = new Dictionary<PowerUp, Effect>();
        foreach (PowerUp pow in Enum.GetValues(typeof(PowerUp))) {
            powUpDict.Add(pow, new Effect(false, 0));
        }
        #if UNITY_EDITOR
        if (canShoot) {
            StartCoroutine(Shoot());
        }
        #else
        StartCoroutine(Shoot());
        #endif
        foreach (AudioClip clip in sounds) {
            bool fail = clip.LoadAudioData();
            if (fail) {
                Debug.LogWarning($"Failed to load Audio clip: {clip}");
            }
        }
        
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
        
        rb.velocity = motionVector * maxSpeed;

        if (xShoot != 0) {
            float newAngle = -xShoot * rotationFactor;
            //float newAngle = -Mathf.Atan2(xShoot, yShoot) * 180/Mathf.PI;
            //newAngle = Mathf.Lerp(rb.rotation, newAngle, Time.deltaTime * rotationFactor);
            rb.rotation += newAngle * Time.deltaTime; 
            // don't interpolate
        }

    }

    public void updatePowers(PowerUp pow, Effect newEff) {
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
        int bulletCount = 0;
        while(alive) {
            yield return new WaitForSeconds(fireTimer); 
            Quaternion rot = transform.rotation;
            Vector3 eulerRot = rot.eulerAngles;
            spawnProjectile(p, transform.position,rot, Projectile.Behavior.DEFAULT);

            if(powUpDict[PowerUp.MORE_BULLETS].active) {
                
                int numBullets = (int)powUpDict[PowerUp.MORE_BULLETS].magnitude;
                int offMult = 0;
                for (int i = 0; i < numBullets; i++) {
                    int mod2 = i % 2;
                    if (mod2 == 0) {
                        offMult++;
                    }
                    int LeftRight =  (mod2 == 0) ? -1 : 1;
                    float offset = 15 * LeftRight * offMult;
                    Quaternion newRot = Quaternion.Euler(eulerRot.x, eulerRot.y, eulerRot.z + offset);
                    spawnProjectile(p, transform.position, newRot, Projectile.Behavior.DEFAULT);
                }
            }

            if(powUpDict[PowerUp.BEHIND_SHOT].active) {
                if (bulletCount % 3 == 0) {
                    Quaternion newRot = Quaternion.Euler(eulerRot.x, eulerRot.y, eulerRot.z + 180);
                    spawnProjectile(p, transform.position, newRot, Projectile.Behavior.DEFAULT);
                }
            /*} else if(powUpDict[PowerUp.HOMING_SHOT].active){
                if (bulletCount % 5 == 0) {
                    spawnProjectile(p, transform.position, rot, Projectile.Behavior.HOMING);
                }*/
            } else {
                bulletCount = 0;
            }

            bulletCount++;
            bulletCount = bulletCount % 30; //limit to how high it goes to prevent overflow, not really needed but y'know

            
        }
    }
    

    private void spawnProjectile(GameObject pGameObject, Vector3 position, Quaternion rot, Projectile.Behavior PB) {
        Instantiate(pGameObject, position, rot);
        Projectile projectile = pGameObject.GetComponent<Projectile>();
        projectile.damage = damage;
        projectile.speed = projectileSpeed;
        projectile.origin = Projectile.Origin.PLAYER;
        projectile.behaviorState = PB;
        audioSaus.clip = sounds[0];
        audioSaus.Play(0);
    }

    public void die() {
        if (powUpDict[PowerUp.DODGE].active) {
            updatePowers(PowerUp.DODGE, new Effect(false, 0));
            Vector2 randVect = UnityEngine.Random.insideUnitCircle;
            randVect.Normalize();
            transform.position += new Vector3(randVect.x, randVect.y, 0f) * dodgeRange;
            
        } else {
            AudioSource.PlayClipAtPoint(sounds[3], transform.position);
            #if UNITY_EDITOR
            if (canDie) {
                GameManager.Defeat();
                Destroy(gameObject);
            }
            #else
            GameManager.Defeat();
            Destroy(gameObject);
            #endif
            
        }
    }


}

[System.Serializable]
public struct Effect {
        public bool active;
        public float magnitude;
        // public Type t;

        public Effect(bool status, float value) {
            active = status;
            magnitude = value;
        }

    }
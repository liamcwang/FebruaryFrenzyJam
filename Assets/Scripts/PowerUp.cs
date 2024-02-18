using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PowerUp : MonoBehaviour
{
    public Player.PowerUp type;
    public float fireRate = 10f;
    public float moreBullets = 2f;
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spiRend;
    private Effect effect;
    private static Object PowFab;
    

    // Start is called before the first frame update
    void Start()
    {
        spiRend = GetComponent<SpriteRenderer>();
        switch(type) {
            case Player.PowerUp.FIRE_RATE:
                effect = new Effect(true, fireRate);
                break;
            case Player.PowerUp.MORE_BULLETS:
                effect = new Effect(true, moreBullets);
                break;
            default:
                effect = new Effect(true, 0);
                break;
        }
        spiRend.sprite = sprites[(int)type];
        
        //if (type == null) Debug.LogWarning("No powerup was set.");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player p = GameManager.instance.player;
            p.updatePowers(type, effect);
            Destroy(gameObject);
        }
    }

    [MenuItem("GameFunctions/SpawnRandPowerUp")]
    public static void SpawnRandPowerUp() {
        SpawnPowerUp(Vector3.zero);
    }


    public static void SpawnPowerUp(Vector3 location) {
        if (PowFab == null) {
            PowFab = Resources.Load<GameObject>("PowerUp");
        }
        GameObject newObject = Instantiate((GameObject)PowFab, location, Quaternion.identity);
        PowerUp newPow = newObject.GetComponent<PowerUp>();
        int randInt = Random.Range(0, Player.PowerUp.GetNames(typeof(Player.PowerUp)).Length);
        newPow.type = (Player.PowerUp) randInt;
        //Instantiate(newObject, location, Quaternion.identity);
    }
}

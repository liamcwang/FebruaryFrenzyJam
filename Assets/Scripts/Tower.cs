using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum Debuff{HP, DEF, SPEED};
    public float health = 10f;
    public Debuff effect;
    [SerializeField] private float[] debuffValues;
    private float magnitude;
    private Boss boss;
    [SerializeField] private SpriteRenderer spiRend;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        spiRend.sprite = sprites[(int) effect];
        boss = GameManager.instance.boss;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMode(int mode) {
        effect = (Debuff) mode;
        magnitude = debuffValues[mode];
    }

    public void takeDamage(float damage) {
        health -= damage;

        if (health <= 0) {
            // looks safer to do it this way, rather than OnDestroy
            boss.debuff(effect, magnitude);
            AudioSource.PlayClipAtPoint(clip, transform.position);
            Destroy(gameObject);
            
        }
    }

}

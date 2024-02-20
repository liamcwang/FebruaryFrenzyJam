using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// These towers will debuff the boss when destroyed
/// </summary>
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

    /// <summary>
    /// Set what debuff this tower is
    /// </summary>
    /// <param name="mode"></param>
    public void setMode(int mode) {
        // REMINDER: Casting ints to enums, very useful
        effect = (Debuff) mode;
        magnitude = debuffValues[mode];
    }

    /// <summary>
    /// No defense on this so, causes no problems when taking damage
    /// </summary>
    /// <param name="damage"></param>
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

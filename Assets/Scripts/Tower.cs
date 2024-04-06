using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// These towers will debuff the boss when destroyed
/// </summary>
public class Tower : MonoBehaviour
{
    public enum Debuff{HP, TIMER, SPEED};
    public float health = 10f;
    public Debuff effect;
    public float explosionOffset = 0.65f;
    [SerializeField] private float[] debuffValues;
    private float magnitude;
    private Boss boss;
    [SerializeField] private SpriteRenderer spiRend;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private AudioClip clip;
    private Collider2D towerCollider;

    private Vector3 explosionLocation = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        spiRend.sprite = sprites[(int) effect];
        boss = GameManager.instance.boss;
        towerCollider = GetComponent<BoxCollider2D>();
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
            GameManager.instance.mainMenu.UpdateBossValues(effect);
            StartCoroutine(DestroySequence());
            
        }
    }

    IEnumerator DestroySequence() {
        towerCollider.enabled = false;
        explosionLocation.x = transform.position.x;
        explosionLocation.y = transform.position.y + explosionOffset;
        Explosion.SpawnExplosion(explosionLocation, effect);
        spiRend.sprite = sprites[sprites.Length - 1];
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }

    #if UNITY_EDITOR
    private Vector3 cubeSize = new Vector3(0.5f,0.5f,0.5f);
    private Vector3 gizmoLocation = Vector3.zero;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        gizmoLocation.x = transform.position.x;
        gizmoLocation.y = transform.position.y + explosionOffset;
        Gizmos.DrawCube(gizmoLocation, cubeSize);
    }
    #endif

}

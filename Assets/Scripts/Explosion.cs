using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Explosion : MonoBehaviour
{
    public Tower.Debuff debuffType;
    public string[] explosions;
    private Animator animator;
    private static GameObject explodeFab;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play($"{explosions[(int) debuffType]}_Explosion");
    }

    public void onAnimationEnd() {
        Destroy(gameObject);
    }

    #if UNITY_EDITOR
    [MenuItem("GameFunctions/SpawnExplosion")]
    static public void SpawnFixedExplosion() {
        Vector3 pos = Vector3.zero;
        int randInt = Random.Range(0, Tower.Debuff.GetNames(typeof(Tower.Debuff)).Length);
        SpawnExplosion(pos, (Tower.Debuff) randInt);
    }
    #endif

    static public void SpawnExplosion(Vector3 location, Tower.Debuff debuffType) {
        if (explodeFab == null) {
            explodeFab = Resources.Load<GameObject>("Explosion");
        }
        GameObject newObject = Instantiate((GameObject)explodeFab, location, Quaternion.identity);
        Explosion newExplosion = newObject.GetComponent<Explosion>();
        newExplosion.debuffType = debuffType;
    }
}

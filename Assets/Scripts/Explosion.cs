using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Tower.Debuff debuffType;
    public ExplosionBundle[] explosions;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public struct ExplosionBundle {
    Animation animation;
    Sprite sprite;
}
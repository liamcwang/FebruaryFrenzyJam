using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum Debuff{HP, DEF, SPEED};
    public Debuff effect;
    public float magnitude;
    private Boss boss;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameManager.instance.boss;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy() {
        
    }
}

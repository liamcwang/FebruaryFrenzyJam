using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public Tower.Debuff barType;
    [SerializeField] Sprite[] sprites;
    Image spriteRenderer;
    int currentValue;

    private void Start()
    {
        spriteRenderer = GetComponent<Image>();
        currentValue = 0;
        spriteRenderer.overrideSprite = sprites[currentValue];

    }

    public void IncrementBar() {
        currentValue++;
        spriteRenderer.overrideSprite = sprites[currentValue];
    }
}
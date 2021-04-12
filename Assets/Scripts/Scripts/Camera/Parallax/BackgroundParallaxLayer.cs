using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BackgroundParallaxLayer
{
    [Tooltip("Sprite renderers to apply Parallax effect on")]
    public SpriteRenderer SpriteRenderer;

    [Tooltip("Parallax effect to apply")]
    public float ParallaxEffect;

    [Header("Calculated")]

    [ReadOnly]
    [Tooltip("Calculated layer startPosition")]
    public float StartPosition;

    [ReadOnly]
    [Tooltip("Calculated length of the SpriteRenderer bounds")]
    public float Length;
}

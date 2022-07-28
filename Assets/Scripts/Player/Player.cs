using System;
using UnityEngine;

public class Player : Singleton<Player> {
    public Transform bodyTransform;
    
    
    public Health health;
    public Mana mana;
    
    public SpriteRenderer playerSpriteRenderer;
}

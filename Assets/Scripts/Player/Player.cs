using System;
using Saver;
using UnityEngine;

public class Player : Singleton<Player> {
    public Transform bodyTransform;
    
    
    public Health health;
    public Mana mana;
    
    public SpriteRenderer playerSpriteRenderer;

    public PlayerMemento SaveState() {
        return new PlayerMemento(bodyTransform.position, health.GetCurrentHealth());
    }
}

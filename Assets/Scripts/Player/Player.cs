using System;
using Saver;
using UnityEngine;

public class Player : Singleton<Player> {
    public Transform bodyTransform;
    public Transform abilityContainer;
    
    public Health health;
    public Mana mana;
    
    public SpriteRenderer playerSpriteRenderer;


    public void ResetPlayer() {
        health.Heal(100);
        mana.RestMana(100);
    }
}

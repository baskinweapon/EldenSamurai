using UnityEngine;

public class Player : Singleton<Player> {
    public Rigidbody2D rb;
    public Transform bodyTransform;
    public Transform abilityContainer;
    
    // Gentleman set
    public Health health;
    public Mana mana;
    public Expirience expirience;
    
    public SpriteRenderer playerSpriteRenderer;

    private bool isCastingAbility;
    public bool IsCastingAbility() => isCastingAbility;
    public void SetCastingState(bool _state) => isCastingAbility = _state;

    public float damagePassTime;
    

    public void ResetPlayer() {
        health.Heal(100);
        mana.RestMana(100);
    }
}

using System;
using UnityEngine;

[Serializable]
public enum AbilityButton {
    first,
    second,
    third,
    four
}

namespace Abilities {
    public abstract class Ability : ScriptableObject {
        public string abilityName = "New Ability";
        public Sprite sprite;
        public AudioClip sound;
        public float baseCooldown = 1f;
        public float manaCost = 10f;

        // for chose button position on UI 
        public AbilityButton buttonPosition;
        
        public abstract Damager Initiliaze(GameObject obj, Transform parent);
        public abstract void TriggerAbility(Damager _damager = null);
    }
}
using Damage;
using UnityEngine;

namespace Abilities {
    
    [CreateAssetMenu(fileName = "Fire", menuName = "Ability/FireEnemy", order = 0)]
    public class FireBallEnemyAbility : Ability {
        public GameObject prefab;
        
        public override BaseDamager Initiliaze(GameObject obj, Transform parent) {
            var inst = Instantiate(obj, parent);
            var _damager = inst.GetComponent<Fireball>().damager.GetComponent<BaseDamager>();
            _damager.damageValue = damage;
            _damager.duration = duration;
            _damager.gameObject.SetActive(false);
            
            return _damager;
        }
        
        public override void TriggerAbility(BaseDamager _damager) {
            _damager.GetComponentInParent<Fireball>().StartCast(duration);
        }
    }
}
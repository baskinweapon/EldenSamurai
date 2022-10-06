using Damage;
using UnityEngine;

namespace Abilities {
    
    [CreateAssetMenu(fileName = "Fire", menuName = "Ability/FireEnemy", order = 0)]
    public class FireBallEnemyAbility : Ability {
        public GameObject prefab;
        
        public override Damager Initiliaze(GameObject obj, Transform parent) {
            var inst = Instantiate(obj, parent);
            var _damager = inst.GetComponent<BalisticProjectile>().damager.GetComponent<Damager>();
            _damager.damageValue = damage;
            _damager.duration = duration;
            _damager.gameObject.SetActive(false);
            
            return _damager;
        }
        
        public override void TriggerAbility(Damager _damager = null) {
            if (_damager != null) _damager.GetComponentInParent<BalisticProjectile>().StartCast(duration);
        }
    }
}
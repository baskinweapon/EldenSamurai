using Enemies;
using UnityEngine;

namespace Abilities {
    
    [CreateAssetMenu(fileName = "Fire", menuName = "Ability/FireEnemy", order = 0)]
    public class FireBallEnemyAbility : Ability {
        
        public override Damager Initiliaze(GameObject obj, Transform parent) {
            var inst = Instantiate(obj, parent);
             var _damager = inst.GetComponent<Damager>();
             _damager.damageValue = damage;
             _damager.duration = castTime;
             _damager.gameObject.SetActive(false);
             
             return _damager;
        }

        public override void TriggerAbility(Damager _damager) {
            if (_damager.transform.GetComponentInParent<Enemy>().enemyAI.enemyGFX.flipX) {
                _damager.transform.localPosition = new Vector3(-0.7f, -0.13f, 0);
                _damager.transform.localScale = new Vector3(-1, 1, 1);
            } else {
                _damager.transform.localPosition = new Vector3(0.7f, -0.13f, 0);
                _damager.transform.localScale = new Vector3(1, 1, 1);
            }
            
            _damager.gameObject.SetActive(true);
        }
    }
}
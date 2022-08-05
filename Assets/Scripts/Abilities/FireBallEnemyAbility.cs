using System.Collections;
using UnityEngine;

namespace Abilities {
    
    [CreateAssetMenu(fileName = "Fire", menuName = "Ability/FireEnemy", order = 0)]
    public class FireBallEnemyAbility : Ability {
        
        public override Damager Initiliaze(GameObject obj, Transform parent) {
            Damager _damager;
            
            var inst = Instantiate(obj, parent);
            _damager = inst.GetComponent<Damager>();
            _damager.gameObject.SetActive(false);
            return _damager;
        }

        public override void TriggerAbility(Damager _damager) {
            _damager.gameObject.SetActive(true);
            var moveDirection = _damager.gameObject.GetComponent<MoveDirection>();
            if (_damager.transform.parent.GetComponent<EnemyAI>().enemyGFX.localScale.x < 0) {
                _damager.transform.localPosition = new Vector3(-1f, 0, 0);
                _damager.transform.localScale = new Vector3(-1, 1, 1);
                moveDirection.SetSign(-1);
            } else {
                moveDirection.SetSign(1);
                _damager.transform.localPosition = new Vector3(1f, 0, 0);
                _damager.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
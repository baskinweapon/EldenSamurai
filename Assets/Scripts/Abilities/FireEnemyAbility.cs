using System.Collections;
using UnityEngine;

namespace Abilities {
    
    [CreateAssetMenu(fileName = "Fire", menuName = "Ability/FireEnemy", order = 0)]
    public class FireEnemyAbility : Ability {

        private Damager _damager;
        private MoveDirection moveDirection;
        private Transform owner;
        
        public override void Initiliaze(GameObject obj, Transform parent) {
            var inst = Instantiate(obj, parent);
            owner = parent;
            _damager = inst.GetComponent<Damager>();
            moveDirection = inst.GetComponent<MoveDirection>();
            _damager.SetActive(false);
            
        }

        public override void TriggerAbility() {
            _damager.gameObject.SetActive(true);
            _damager.SetActive(true);
            if (owner.GetComponent<EnemyAI>().enemyGFX.localScale.x < 0) {
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
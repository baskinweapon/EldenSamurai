using Damage;
using Effects;
using UnityEngine;

namespace Abilities {
    
    [CreateAssetMenu(fileName = "FireBreath", menuName = "Ability/FireBreath", order = 0)]
    public class FireBreathAbility : Ability {
        private PlayEffects _playEffects;
        
        public override BaseDamager Initiliaze(GameObject obj, Transform parent) {
            var inst = Instantiate(obj, parent);
            _playEffects = inst.GetComponent<PlayEffects>();
            var _damager = inst.GetComponentInChildren<Damager>();
            _damager.duration = duration;
            _damager.damageValue = damage;
            _playEffects.Stop();
            _damager.gameObject.SetActive(false);
            return _damager;
        }

        public override void TriggerAbility(BaseDamager damager) {
            var prevParent = _playEffects.transform.parent;
            
            if (Player.instance.playerSpriteRenderer.flipX) {
                _playEffects.transform.parent = Player.instance.bodyTransform;
                _playEffects.transform.localPosition = new Vector3(-0.3f, 1f, 0);
                _playEffects.transform.localRotation = new Quaternion(0, 90, 0, 0);
                _playEffects.transform.parent = prevParent;
            } else {
                _playEffects.transform.parent = Player.instance.bodyTransform;
                _playEffects.transform.localPosition = new Vector3(0.3f, 1f, 0);
                _playEffects.transform.localRotation = new Quaternion(0, 0, 0, 0);
                _playEffects.transform.parent = prevParent;
            }
            
            damager.gameObject.SetActive(true);
            _playEffects.Play();
        }
    }
    
}
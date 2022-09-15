using Damage;
using Effects;
using UnityEngine;

namespace Abilities {
    
    [CreateAssetMenu(fileName = "FireBreath", menuName = "Ability/FireBreath", order = 0)]
    public class FireBreathAbility : Ability {
        private PlayEffects _playEffects;
        private BaseDamager _damager;
        
        public override BaseDamager Initiliaze(GameObject obj, Transform parent) {
            var inst = Instantiate(obj, parent);
            _playEffects = inst.GetComponent<PlayEffects>();
            _damager = inst.GetComponentInChildren<Damager>();
            _damager.duration = duration;
            _damager.damageValue = damage;
            _playEffects.Stop();
            _damager.gameObject.SetActive(false);
            return _damager;
        }

        public override void TriggerAbility(BaseDamager damager = null) {
            var tr = _playEffects.transform;
            var prevParent = tr.parent;
            
            if (Player.instance.playerSpriteRenderer.flipX) {
                tr.parent = Player.instance.bodyTransform;
                tr.localPosition = new Vector3(-0.3f, 1f, 0);
                tr.localRotation = new Quaternion(0, 90, 0, 0);
                tr.parent = prevParent;
            } else {
                tr.parent = Player.instance.bodyTransform;
                tr.localPosition = new Vector3(0.3f, 1f, 0);
                tr.localRotation = new Quaternion(0, 0, 0, 0);
                tr.parent = prevParent;
            }
            
            _damager.gameObject.SetActive(true);
            _playEffects.Play(duration);
        }
    }
    
}
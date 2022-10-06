using Damage;
using Effects;
using UnityEngine;

namespace Abilities {
    
    [CreateAssetMenu(fileName = "FireFromEarth", menuName = "Ability/FireFromEarth", order = 0)]
    public class FireFromEarthAbility : Ability {
        private PlayEffects _playEffects;
        private Damager _damager;
        
        public override Damager Initiliaze(GameObject obj, Transform parent) {
            var inst = Instantiate(obj, parent);
            _playEffects = inst.GetComponent<PlayEffects>();
            _damager = inst.GetComponentInChildren<Damager>();
            _damager.duration = castTime;
            _damager.damageValue = damage;
            _playEffects.Stop();
            _damager.gameObject.SetActive(false);
            return _damager;
        }
        
        public override void TriggerAbility(Damager damager = null) {
            var tr = _playEffects.transform;
            var prevParent = tr.parent;
            if (Player.instance.playerSpriteRenderer.flipX) {
                tr.parent = Player.instance.bodyTransform;
                tr.localPosition = new Vector3(-2.0f, 1f, 0);
                tr.localRotation = new Quaternion(0, 90, 0, 0);
                tr.parent = prevParent;
            } else {
                tr.parent = Player.instance.bodyTransform;
                tr.localPosition = new Vector3(2.0f, 1f, 0);
                tr.localRotation = new Quaternion(0, 0, 0, 0);
                tr.parent = prevParent;
            }
            _damager.gameObject.SetActive(true);
            _playEffects.Play(duration);
        }
    }
    
}
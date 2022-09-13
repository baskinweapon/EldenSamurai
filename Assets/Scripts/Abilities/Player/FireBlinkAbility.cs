using Damage;
using Effects;
using UnityEngine;

namespace Abilities {
    
    [CreateAssetMenu(fileName = "FireBlink", menuName = "Ability/FireBlink", order = 0)]
    public class FireBlinkAbility: Ability {
        public float power = 100f;
        
        private PlayEffects _playEffects;
        
        public override BaseDamager Initiliaze(GameObject obj, Transform parent) {
            var inst = Instantiate(obj, parent);
            _playEffects = inst.GetComponent<PlayEffects>();
            BaseDamager damager = inst.GetComponentInChildren<BaseDamager>();
            damager.duration = duration;
            damager.damageValue = damage;
            damager.gameObject.SetActive(false);
            _playEffects.Stop();
            
            return damager;
        }

        public override void TriggerAbility(BaseDamager _damager) {
            var tr = _playEffects.transform;
            var prevParent = tr.parent;

            var dir = Vector2.left;
            if (Player.instance.playerSpriteRenderer.flipX) {
                tr.parent = Player.instance.bodyTransform;
                tr.localPosition = new Vector3(0f, 1f, 0);
                tr.localRotation = new Quaternion(0, 90, 0, 0);
                dir = Vector2.right;
            } else {
                tr.parent = Player.instance.bodyTransform;
                tr.localPosition = new Vector3(0, 1f, 0);
                tr.localRotation = new Quaternion(0, 0, 0, 0);
            }
            
            Player.instance.rb.velocity = Vector2.zero;
            Player.instance.rb.AddForce(power * 1000f * dir);
            _damager.gameObject.SetActive(true);
            _playEffects.Play();
            tr.parent = prevParent;
        }
    }
}
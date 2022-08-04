using Effects;
using UnityEngine;

namespace Abilities {
    
    [CreateAssetMenu(fileName = "FireBreath", menuName = "Ability/FireBreath", order = 0)]
    public class FireBreathAbility : Ability {
        private PlayEffects _playEffects;

        private Damager _damager;
        public override void Initiliaze(GameObject obj, Transform parent) {
            var inst = Instantiate(obj, Player.instance.bodyTransform);
            _playEffects = inst.GetComponent<PlayEffects>();
            _damager = inst.GetComponent<Damager>();
            _damager.SetActive(false);
            _playEffects.Stop();
        }

        public override void TriggerAbility() {
            _damager.gameObject.SetActive(true);
            _damager.SetActive(true);
            if (Player.instance.playerSpriteRenderer.flipX) {
                _playEffects.transform.localPosition = new Vector3(-0.6f, 1.2f, 0);
                _playEffects.transform.localRotation = new Quaternion(0, 0, 180, 0);
            } else {
                _playEffects.transform.localPosition = new Vector3(0.6f, 1.2f, 0);
                _playEffects.transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
            _playEffects.Play();
        }
    }
    
}
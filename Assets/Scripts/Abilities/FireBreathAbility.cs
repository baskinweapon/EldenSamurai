using Effects;
using UnityEngine;

namespace Abilities {
    
    [CreateAssetMenu(fileName = "FireBreath", menuName = "Ability/FireBreath", order = 0)]
    public class FireBreathAbility : Ability {
        private PlayEffects _playEffects;

        private Damager _damager;
        public override Damager Initiliaze(GameObject obj, Transform parent) {
            var inst = Instantiate(obj, Player.instance.bodyTransform);
            _playEffects = inst.GetComponent<PlayEffects>();
            _damager = inst.GetComponent<Damager>();
            _playEffects.Stop();
            _damager.gameObject.SetActive(false);
            return _damager;
        }

        public override void TriggerAbility(Damager damager) {
            if (Player.instance.playerSpriteRenderer.flipX) {
                _playEffects.transform.localPosition = new Vector3(-1f, 1.2f, 0);
                _playEffects.transform.localRotation = new Quaternion(0, 90, 0, 0);
            } else {
                _playEffects.transform.localPosition = new Vector3(1f, 1.2f, 0);
                _playEffects.transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
            _damager.gameObject.SetActive(true);
            _damager.TriggerAbility();
            _playEffects.Play();
        }
    }
    
}
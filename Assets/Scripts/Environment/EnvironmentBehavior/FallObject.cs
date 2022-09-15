using Architecture.Interfaces;
using UnityEngine;

namespace Environment.EnvironmentBehavior {
    public class FallObject : MonoBehaviour {
        [SerializeField] private GameObject rbObject;
        [SerializeField] private SpriteRenderer render;
        public float distanceToActivate;
        public float coolDown;
        
        private bool isFalling;
        private float time;
        
        private void Update() {
            time += Time.deltaTime;
            if (time >= coolDown) {
                time = 0;
                isFalling = false;
            }
            if (isFalling) return;
            if (Mathf.Abs(Player.instance.bodyTransform.position.x - transform.position.x) <= distanceToActivate) {
                isFalling = true;
                StartFall();
            }
        }

        private void StartFall() {
            render.enabled = false;
            rbObject.SetActive(true);
        }
        
        public void ResetObj() {
            time = 0;
            rbObject.transform.localPosition = Vector3.zero;
            rbObject.SetActive(false);
            render.enabled = true;
        }
    }
}
using Unity.VisualScripting;
using UnityEngine;

namespace Enemies {
    public class SpliEnemy : Enemy {
        public GameObject wave;
        public int stackId;

        protected override void OnEnable() {
            base.OnEnable();
            if (stackId >= 2) {
                Destroy(gameObject);
            }
        }
    }
}
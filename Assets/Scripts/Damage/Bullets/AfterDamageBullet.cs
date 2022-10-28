using UnityEngine;

namespace Damage.Bullets {
    public class AfterDamageBullet : Bullet {
        public GameObject prefab;
        public Rigidbody2D rb;
        
        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
                Debug.Log("Create earth object");
                var obj = Instantiate(prefab);
                obj.transform.position = rb.position;
                obj.transform.rotation = rb.transform.rotation;
                gameObject.SetActive(false);
            }
        }
    }
}
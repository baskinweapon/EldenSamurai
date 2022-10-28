using UnityEngine;

namespace Damage.Bullets {
    public class LineProjectile : MonoBehaviour {
        public ObjectPool poolObj;
        public Transform attackPoint;

        public float speed = 2f;
        
        public void CreateBullet() {
            var obj = poolObj.GetPooledObject();
            var bullet = obj.GetComponent<Rigidbody2D>();
            bullet.velocity = Vector2.zero;
            bullet.transform.position = attackPoint.position;
            obj.SetActive(true);
            
            var vec = Player.instance.rb.transform.position - bullet.transform.position;
            // Debug.Log("Player " + Player.instance.rb.position + "Bullet " + bullet.transform.position + "vec " + vec);
            
            bullet.AddForce(vec.normalized * speed);
        }
    }
}
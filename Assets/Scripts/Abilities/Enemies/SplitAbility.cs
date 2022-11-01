using System;
using UnityEngine;

namespace Abilities.Enemies {
    
    public class SplitAbility : MonoBehaviour {
        [SerializeField] private GameObject prefab;
        
        public void Split() {
            Debug.Log("Split enemy");
            var secondEnemy = Instantiate(prefab);
            secondEnemy.GetComponent<Health>().Heal(100f);
            secondEnemy.transform.position += Vector3.left;
            secondEnemy.transform.localScale *= 0.5f;
            
            var thirdEnemy = Instantiate(prefab);
            thirdEnemy.GetComponent<Health>().Heal(100f);
            thirdEnemy.transform.position += Vector3.right;
            thirdEnemy.transform.localScale *= 0.5f;
        }
    }
}
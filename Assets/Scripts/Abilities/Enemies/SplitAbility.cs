using Enemies;
using UnityEngine;

namespace Abilities.Enemies {
    
    public class SplitAbility : MonoBehaviour {
        [SerializeField] private GameObject prefab;
        
        public void Split() {
            Debug.Log("Split enemy");
            var secondEnemy = Instantiate(prefab);
            secondEnemy.GetComponent<Health>().Heal(100f);
            var _ = secondEnemy.GetComponent<SpliEnemy>();
            _.wave.SetActive(true);
            _.stackId++;
            secondEnemy.transform.position += Vector3.left;
            secondEnemy.transform.localScale *= 0.5f;
            
            
            var thirdEnemy = Instantiate(prefab);
            thirdEnemy.GetComponent<Health>().Heal(100f);
            var split = thirdEnemy.GetComponent<SpliEnemy>();
            split.wave.SetActive(true);
            split.stackId++;
            thirdEnemy.transform.position += Vector3.right;
            thirdEnemy.transform.localScale *= 0.5f;
            
        }
    }
}
using Abilities;
using UnityEngine;

namespace Enemies {
    public class EnemyAttack : MonoBehaviour {
        [SerializeField] private Enemy enemy;
     
        [SerializeField] 
        private BaseAbilityCooldown abilityCooldown;

        private void OnEnable() {
            enemy.enemyAI.OnAttack += Attack;
        }
        
        private void Attack() {
            if (abilityCooldown.CooldownComplete())
                enemy.animator.SetTrigger("Attack");
            abilityCooldown.Triggered();
        }

        private void OnDisable() {
            enemy.enemyAI.OnAttack -= Attack;
        }
    }
}
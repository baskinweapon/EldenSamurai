using UnityEngine;

namespace Enemies {
    public class Enemy : MonoBehaviour {
        public Owner owner;
        public EnemyAI enemyAI;
        public Animator animator;
    }
}
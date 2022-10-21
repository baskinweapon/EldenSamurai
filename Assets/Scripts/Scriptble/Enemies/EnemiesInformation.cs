using UnityEngine;

namespace Scriptble.Enemies {
    [CreateAssetMenu(fileName = "EnemiesInformation", menuName = "Game/EnemiesInformation", order = 0)]
    public class EnemiesInformation : ScriptableObject {
        public EnemyInfo[] enemies;
    }
}
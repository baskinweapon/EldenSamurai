using UnityEngine;

namespace Saver {
    public class PlayerMemento {
        public Vector3 position;
        public float curHP;

        public PlayerMemento(Vector3 position, float curHp) {
            this.position = position;
            this.curHP = curHp;
        }
    }
}
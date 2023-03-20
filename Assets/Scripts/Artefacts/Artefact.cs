using UnityEngine;

namespace Artefacts {
    
    public abstract class Artefact : MonoBehaviour {
        public string description;

        public Vector2 position = new Vector2(10, 10);
        public int powerMultiplier = 10;

        public abstract string GetDescription();
    }
    
}
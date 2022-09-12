using UnityEngine;

namespace Scriptble {
    [CreateAssetMenu(fileName = "FILENAME", menuName = "Game/CharacterSounds", order = 0)]
    public class SoundCharacterAsset : ScriptableObject {
        public AudioClip walk;
        public AudioClip hurt;
        public AudioClip die;
    }
}
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Game/Enemies", order = 0)]
public class EnemyInfo : ScriptableObject {
    
    public Sprite image;
    public string description;
    public GameObject prefab;
    
}
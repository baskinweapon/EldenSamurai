using UnityEngine;

public class SkullCreator : MonoBehaviour {
    public float cooldown;
    public GameObject prefab;

    private float time;
    private void Update() {
        time += Time.deltaTime;
        if (time >= cooldown) {
            Create();
            time = 0;
        }
    }

    private void Create() {
        var skull = Instantiate(prefab, transform.position + new Vector3(Random.Range(0.001f, 0.01f), 0, 0), Quaternion.identity, transform);
        Destroy(skull, Random.Range(10f, 30f));
    }
}

using UnityEngine;

public class DropEvent : MonoBehaviour {
    public GameObject dropPrefab;

    public int minDropObject;
    public int maxDropObject;
    
    public void OnEnable() {
        GetComponent<Health>().OnDeath.AddListener(Dropped);
    }

    private void Dropped() {
        var range = Random.Range(minDropObject, maxDropObject);
        for (int i = 0; i < range; i++) {
            var dropGO = Instantiate(dropPrefab);
            dropGO.transform.position = transform.position;
        }
    }

    private void OnDisable() {
        GetComponent<Health>().OnDeath.RemoveListener(Dropped);
    }
}

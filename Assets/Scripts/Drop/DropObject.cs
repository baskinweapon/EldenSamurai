using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(IDrop))]
[RequireComponent(typeof(Collider2D))]
public class DropObject : MonoBehaviour {
    private IDrop drop;
    
    private void OnEnable() {
        AddRandomForce();
        drop = GetComponent<IDrop>();
    }

    private void AddRandomForce() {
        var vec = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f));
        GetComponent<Rigidbody2D>().AddForce(vec * Random.Range(500f, 1000f));
    }
    
    private void OnCollisionEnter2D(Collision2D col) {
        if (!col.gameObject.tag.Contains("Player")) return;
        
        //Get a Drop
        drop.CompleteDrop();
        
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(CollectProcess());
    }
    
    IEnumerator CollectProcess() {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}

// #if UNITY_EDITOR
// [CustomEditor(typeof(DropObject))]
// public class DropObjectEditor : Editor {
//     public override void OnInspectorGUI() {
//         var me = target as DropObject;
//         base.OnInspectorGUI();
//
//         // if (GUILayout.Button("Set random drop type")) {
//         //     // me.SetDropType();
//         // }
//         
//         
//     }
// }
//
// #endif



using System.Collections;
using Damage;
using UnityEngine;

public class DamageHorizontalWave : Damager {
    private void OnEnable() {
        StartCast();
    }

    private void StartCast() {
        StartCoroutine(ScaleProcess());
    }

    private float time;
    IEnumerator ScaleProcess() {
        time = 0;
        while (true) {
            time += Time.deltaTime;
            float x = Mathf.Lerp(0, 5, Mathf.PingPong(time, 1));
            transform.ChangeScale(x: x);
            yield return null;
        }
    }

    private void OnDisable() {
        StopAllCoroutines();
    }
}

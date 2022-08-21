using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class PlayStopVFXEvent : MonoBehaviour {
    public VisualEffect effect;
    public BoxCollider2D col;

    public float speedColSize = 10f;
    
    public UnityEvent OnPlay;
    public UnityEvent OnStop;

    public float cooldown = 1;
    public float duration = 2;

    public Vector2 maxSizeCol = new Vector2(1, 4);

    private float time = 0;

    private void Start() {
        effect.Stop();
    }

    private void Update() {
        if (cooldown == 0) return;
        time += Time.deltaTime;
        if (time >= cooldown && !isPlaying) {
            Play();
        }

        if (isPlaying) {
            col.offset = Vector2.Lerp(col.offset, maxSizeCol.y * 0.5f * Vector2.up, Time.deltaTime * speedColSize);
            col.size = Vector2.Lerp(col.size, maxSizeCol, Time.deltaTime * speedColSize);
        }

        if (time >= duration + cooldown) {
            Stop();
            col.offset = Vector2.zero;
            col.size = Vector2.zero;
            time = 0f;
        }
        
    }

    private bool isPlaying;
    public void Play() {
        effect.Play();
        OnPlay?.Invoke();
        isPlaying = true;
    }

    public void Stop() {
        effect.Stop();
        OnStop?.Invoke();
        isPlaying = false;
    }

}

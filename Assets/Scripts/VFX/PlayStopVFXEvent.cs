using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class PlayStopVFXEvent : MonoBehaviour {
    public VisualEffect effect;

    public UnityEvent OnPlay;
    public UnityEvent OnStop;

    public float cooldown = 1;
    public float duration = 2;

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

        if (time >= duration + cooldown) {
            Stop();
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

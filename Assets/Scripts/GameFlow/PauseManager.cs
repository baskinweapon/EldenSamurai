using System;
using UnityEngine;

public class PauseManager : Singleton<PauseManager> {
    public static Action<bool> OnPause;

    private void OnEnable() {
        OnPause += Pause;
    }

    private void Pause(bool _state) {
        Time.timeScale = 0;
    }

    private void OnDisable() {
        OnPause -= Pause;
    }
}

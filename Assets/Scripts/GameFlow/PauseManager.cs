using System;
using UnityEngine;

public class PauseManager : Singleton<PauseManager> {
    public static Action<bool> OnPause;
    
    public void Pause(bool _state) {
        if (_state) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
        OnPause?.Invoke(_state);
    }
    
}

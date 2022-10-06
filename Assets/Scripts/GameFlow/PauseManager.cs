using System;
using UnityEngine;

public class PauseManager : Singleton<PauseManager> {
    public static Action<bool> OnPause;
    
    public void Pause(bool _state) {
        Time.timeScale = _state ? 0 : 1;
        OnPause?.Invoke(_state);
    }
    
}

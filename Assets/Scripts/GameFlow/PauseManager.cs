using System;
using UnityEngine;

public class PauseManager : Singleton<PauseManager> {
    public static Action<bool> OnPause;
}

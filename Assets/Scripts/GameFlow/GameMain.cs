
using System;
using UnityEngine;

public class GameMain : Singleton<GameMain> {
    public SettingsAsset settingsAsset;
    public InteractionAsset interactionAsset;
    public Camera mainCamera;
    
    
    public int sceneId;

    protected override void Awake() {
        base.Awake();
        settingsAsset.LoadFromFile();
    }

    private void Start() {
        SceneController.instance.LoadScene(sceneId);
        Player.instance.health.OnDeath.AddListener(OnFinishGame);
    }

    public void OnFinishGame() {
        SceneController.instance.LoadScene(sceneId);
        Player.instance.ResetPlayer();
    }

    private void OnApplicationQuit() {
        settingsAsset.SaveToFile();
    }

    private void OnDestroy() {
        Player.instance.health.OnDeath.RemoveListener(OnFinishGame);
    }
}

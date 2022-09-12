
using System;
using UnityEngine;

public class GameMain : Singleton<GameMain> {
    public SettingsAsset settingsAsset;
    public InteractionAsset interactionAsset;
    
    public Scenes sceneId;

    protected override void Awake() {
        base.Awake();
        settingsAsset.LoadFromFile();
    }

    private void Start() {
        SceneController.instance.LoadScene(sceneId);
        Player.instance.health.OnDeath.AddListener(FinishGame);
    }

    public void FinishGame() {
        SceneController.instance.LoadScene(sceneId);
        Player.instance.ResetPlayer();
    }

    private void OnApplicationQuit() {
        settingsAsset.SaveToFile();
    }

    private void OnDestroy() {
        Player.instance.health.OnDeath.RemoveListener(FinishGame);
    }
}

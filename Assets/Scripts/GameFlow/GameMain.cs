public class GameMain : Singleton<GameMain> {
    public SettingsAsset settingsAsset;
    public InteractionAsset interactionAsset;

    public GameMode gameMode;
    public Scenes firstSceneId;

    protected override void Awake() {
        base.Awake();
        settingsAsset.LoadFromFile();
    }

    private void Start() {
        SceneController.instance.LoadScene(firstSceneId);
        Player.instance.health.OnDeath.AddListener(FinishGame);
    }

    private void FinishGame() {
        SceneController.instance.LoadScene(firstSceneId);
        Player.instance.ResetPlayer();
    }

    private void OnApplicationQuit() {
        settingsAsset.SaveToFile();
    }

    private void OnDestroy() {
        Player.instance.health.OnDeath.RemoveListener(FinishGame);
    }
}

public enum GameMode {
    Normal,
    God_Mode,
}

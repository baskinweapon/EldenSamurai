
public class GameMain : Singleton<GameMain> {
    public SettingsAsset settingsAsset;
    public InteractionAsset interactionAsset;

    public int sceneId;

    protected override void Awake() {
        base.Awake();
        settingsAsset.LoadFromFile();
    }

    private void Start() {
        SceneController.instance.LoadScene(sceneId);
    }

    private void OnApplicationQuit() {
        settingsAsset.SaveToFile();
    }
}

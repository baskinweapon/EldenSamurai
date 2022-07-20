
using System;

public class GameMain : Singleton<GameMain> {
    public InteractionAsset interactionAsset;

    public int sceneId;
    private void Start() {
        SceneController.instance.LoadScene(sceneId);
    }
}

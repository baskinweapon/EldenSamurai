
using System;

public class GameMain : Singleton<GameMain> {


    public int sceneId;
    private void Start() {
        SceneController.instance.LoadScene(sceneId);
    }
}

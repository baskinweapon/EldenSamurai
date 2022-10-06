public class PortalInteraction : Interaction {
    public int scene_id;
    
    protected override void InteractionProcess() {
        SceneController.instance.LoadScene(scene_id);
    }
}

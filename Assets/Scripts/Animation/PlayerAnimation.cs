using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //For animation
    void AE_runStop() {
        AudioManager.instance.PlaySound("RunStop");
    }

    void AE_footstep() {
        AudioManager.instance.PlaySound("Footstep");
    }

    void AE_Jump() {
        AudioManager.instance.PlaySound("Jump");
    }

    void AE_Landing() {
        AudioManager.instance.PlaySound("Landing");
    }
}

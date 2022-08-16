using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerDirection : MonoBehaviour
{
    void LateUpdate() {
        if (Player.instance.playerSpriteRenderer.flipX) {
            transform.localRotation = new Quaternion(0, 90, 0, 0);
        } else {
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }
}

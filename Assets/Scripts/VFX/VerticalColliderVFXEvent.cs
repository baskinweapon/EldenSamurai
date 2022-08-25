using UnityEngine;
using VFX;

public class VerticalColliderVFXEvent : VFXEvents {
    public BoxCollider2D col;

    public float speedColSize = 10f;
    
    public Vector2 maxSizeCol = new Vector2(1, 4);
    
    protected override void Update() {
        base.Update();
        
        if (isPlaying && col) {
            col.offset = Vector2.Lerp(col.offset, maxSizeCol.y * 0.5f * Vector2.up, Time.deltaTime * speedColSize);
            col.size = Vector2.Lerp(col.size, maxSizeCol, Time.deltaTime * speedColSize);
        }
    }
    
    public override void Stop() {
        base.Stop();
        col.offset = Vector2.zero;
        col.size = Vector2.zero;
    }

}

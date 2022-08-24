using UnityEngine;

namespace VFX {
    public class HorizontalColliderVFXEVent : VFXEvents {
        public BoxCollider2D col;
        
        public Vector2 maxSizeCol = new Vector2(4, 1);
        
        public float speedColSize = 10f;

        protected override void Update() {
            base.Update();
            
            if (isPlaying && col) {
                col.offset = Vector2.Lerp(col.offset, maxSizeCol.x * 0.5f * Vector2.right, Time.deltaTime * speedColSize);
                col.size = Vector2.Lerp(col.size, maxSizeCol, Time.deltaTime * speedColSize);
            }
        }
        
        protected override void Stop() {
            base.Stop();
            col.offset = Vector2.zero;
            col.size = Vector2.zero;
        }
    }
}
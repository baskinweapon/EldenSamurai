using System;
using UnityEngine;

namespace Helpers {
    public class Trajectory : MonoBehaviour {

        private LineRenderer lineRenderer;
        
        private void Start() {
            lineRenderer = GetComponent<LineRenderer>();
        }

        public void Show(Vector2 origin, Vector2 speed) {
            Vector3[] points = new Vector3[30];

            lineRenderer.positionCount = points.Length;
            var dir = transform.position - Player.instance.bodyTransform.position;
            var angle = Mathf.Atan2(dir.x, dir.y);
            
            for (int i = 0; i < points.Length; i++) {
                float time = i * 0.1f;
                float x = time * 100 * Mathf.Cos(angle);
                float y = time * 100 * Mathf.Sin(angle) + Physics.gravity.y * time * time / 2;
                points[i] = new Vector3(x, y, 0);

                // if (points[i].y < 0) {
                //     lineRenderer.positionCount = i + 1;
                //     break;
                // }
            }
            
            lineRenderer.SetPositions(points);
        }
    }
}
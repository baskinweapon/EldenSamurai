using System.Collections;
using StableFluids;
using UnityEngine;

public class TestFluidTouch : MonoBehaviour {
   public Vector2 point;
   public Vector2 lastPoint;

   public Fluid fluid;
   
   private void OnCollisionEnter2D(Collision2D col) {
      if (col.contacts.Length > 0) {
         var color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
         fluid.ChangeColor(color);
         fluid._state = false;
         fluid._force = 30f;
         point = col.contacts[0].point;
         lastPoint = col.contacts[^1].point;
      }
   }
   
   
   private void OnTriggerExit2D(Collider2D other) {
      fluid._state = true;
      fluid._force = 0f;
   }
}

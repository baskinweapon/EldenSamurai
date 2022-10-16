using System.Collections;
using UnityEngine;


public class BaseTreasure : MonoBehaviour {
    public Animator animator;
    
    public void DamageAnim() {
        animator.SetTrigger("Damaged");
        Debug.Log("Pass damage");
    }

    public void Disactivated() {
        StartCoroutine(WaitDisactivated());
    }

    IEnumerator WaitDisactivated() {
        yield return new WaitForSeconds(0.5f);
        
        gameObject.SetActive(false);
    }
}

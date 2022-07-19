using UnityEngine;

public class Dust_DestroyEvent : MonoBehaviour
{
    private void Start() {
        Invoke(nameof(destroyEvent), 0.1f);
    }

    public void destroyEvent()
    {
        Destroy(gameObject);
    }
}

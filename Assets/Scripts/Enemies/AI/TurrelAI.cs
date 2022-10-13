using System;
using UnityEngine;

//idea turrel AI
//turrel wakeUp when player nearest
//turrel have 3 level of Y axis (min when have < 20 % health, medium > 20% < 60%, max > 60%)
// bullet looks like a stones
// turrel look like holem from dota 2
public class TurrelAI : MonoBehaviour {
    [SerializeField] private ObjectPool pool;

    [SerializeField] private float speed;

    [SerializeField] private float cooldown;

    [Header("Part of holem")] 
    [SerializeField]
    private GameObject firstPart;
    [SerializeField]
    private GameObject secondPart;
    [SerializeField]
    private GameObject thirdPart;
    
    [SerializeField] 
    private Health health;

    public Action<Stage> OnChangeStage;

    
    public enum Stage {
        sleep,
        wakeUp,
        maxHattack,
        medHattack,
        lowHattack,
    }
    public Stage stage = Stage.sleep;
    
    private float time;
    private void Update() {
        time += Time.deltaTime;
        SelectStage();
        switch (stage) {
            case Stage.sleep:
                //zzZZzzzzZZZzz
                break;
            case Stage.wakeUp:
                //anim wake Up
                break;
            case Stage.maxHattack:
                AttackCooldown(0.4f);
                firstPart.SetActive(true);
                secondPart.SetActive(true);
                thirdPart.SetActive(true);
                break;
            case Stage.medHattack:
                AttackCooldown(0.15f);
                firstPart.SetActive(true);
                secondPart.SetActive(true);
                thirdPart.SetActive(false);
                break;
            case Stage.lowHattack:
                AttackCooldown(0f);
                firstPart.SetActive(true);
                secondPart.SetActive(false);
                thirdPart.SetActive(false);
                break;
        }
    }

    private void SelectStage() {
        var prevStage = stage;
        if (health.GetCurrentHPPercent() > 0.1f) {
            stage = Stage.lowHattack;
        } if (health.GetCurrentHPPercent() > 0.3f) {
            stage = Stage.medHattack;
        } if (health.GetCurrentHPPercent() > 0.7f) {
            stage = Stage.maxHattack;
        }

        if (prevStage != stage) OnChangeStage?.Invoke(stage);
    }

    private void AttackCooldown(float _height) {
        if (time < cooldown) return;
        time = 0;

        var bullet = pool.GetPooledObject();
        if (bullet == null) return;
        bullet.SetActive(true);
        var rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        bullet.transform.parent = transform;
        bullet.transform.position = transform.position + new Vector3(0, _height, 0);
        bullet.transform.rotation = transform.rotation;
        
        var dir = Player.instance.rb.position - (Vector2)bullet.transform.position;
        
        dir = new Vector2(dir.x > 0 ? 1 : -1, 0);
        rb.AddForce(dir * speed);
    }
}

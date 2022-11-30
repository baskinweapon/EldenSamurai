using System;
using UnityEditor;
using UnityEngine;

public class DynamicWater : MonoBehaviour {
    
    [Serializable]
    public struct Bound {
        public float top;
        public float right;
        public float bottom;
        public float left;
    }

    [Header("Water Settings")]
    public Bound bound;
    public int quality;

    public Material waterMaterial;
    public GameObject splash;

    private Vector3[] vertices;

    private Mesh mesh;

    [Header("Physics Settings")] 
    public float floatingPower = 200;
    public float springConstant = 0.02f;
    public float damping = 0.1f;
    public float spread = 0.1f;
    public float collisionVelocityFactor = 0.04f;
    public float underWaterDrag;
    public float underWaterAngularDrag;
    
    public float airDrag;
    public float airAngularDrag;

    [Header("Damage Settings")] 
    public float permanentDamage = 1f;

    private float[] velocities;
    private float[] accelerations;
    private float[] leftDeltas;
    private float[] rightDeltas;

    private float timer;


   
    private void Start() {
        InitializePhysics ();
        GenerateMesh ();
        SetBoxCollider2D ();
    }
    
    private void InitializePhysics () {
        velocities = new float[quality];
        accelerations = new float[quality];
        leftDeltas = new float[quality];
        rightDeltas = new float[quality];
    }

    private void Update() {
        if (timer <= 0) return;
        timer -= Time.deltaTime;
        
        //update Physics
        for (int i = 0; i < quality; i++) {
            float force = springConstant * (vertices[i].y - bound.top) + velocities[i] * damping;
            accelerations[i] = -force;
            vertices[i].y += velocities[i];
            velocities[i] += accelerations[i];
        }

        for (int i = 0; i < quality; i++) {
            if (i > 0) {
                leftDeltas[i] = spread * (vertices[i].y - vertices[i - 1].y);
                velocities[i - 1] += leftDeltas[i];
            }

            if (i < quality - 1) {
                rightDeltas[i] = spread * (vertices[i].y - vertices[i + 1].y);
                velocities[i + 1] += rightDeltas[i];
            }
        }
        
        //update mesh
        mesh.vertices = vertices;
    }


    private bool firstSplash;
    private void OnTriggerEnter2D(Collider2D col) {
        Rigidbody2D rb = col.attachedRigidbody;
        Splash(col, rb.velocity.y * collisionVelocityFactor);
        firstSplash = true;
    }


    private float time;
    private void OnTriggerStay2D(Collider2D other) {
        time += Time.deltaTime;
        if (time >= 1f) {
            time = 0;
            var health = other.GetComponentInParent<Health>();
            if (health) {
                health.Damage(permanentDamage, other);
            }
        }
        
        if (firstSplash && timer <= 0f)
            SplashContinue(other, other.attachedRigidbody.velocity.y * collisionVelocityFactor);
        
        float difference = other.transform.position.y - bound.top;
        if (difference < 0) {
            other.attachedRigidbody.AddForceAtPosition(Vector2.up * floatingPower * Mathf.Abs(difference), other.transform.position, ForceMode2D.Force);
            SwitchState(true, other.attachedRigidbody);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        SwitchState(false, other.attachedRigidbody);
        firstSplash = false;
    }

    void SwitchState(bool isUnderWater, Rigidbody2D rb) {
        if (isUnderWater) {
            rb.drag = underWaterDrag;
            rb.angularDrag = underWaterAngularDrag;
        } else {
            rb.drag = airDrag;
            rb.angularDrag = airAngularDrag;
        }
    }

    public void Splash(Collider2D col, float force) {
        timer = 0.5f;
        float radius = col.bounds.max.x - col.bounds.min.x;
        Vector2 center = new Vector2(col.bounds.center.x, bound.top);
        
        //instantiate splash particle
        GameObject splashGO = Instantiate(splash, new Vector3(center.x, center.y, 0), Quaternion.Euler(0, 0, 60));
        Destroy(splashGO, 2f);
        
        //applying physics
        for (int i = 0; i < quality; i++) {
            if (PointIncideCircle(vertices[i], center, radius)) {
                velocities[i] = force;
            }
        }
    }

    private void SplashContinue(Collider2D col, float force) {
        timer = 0.3f;
        float radius = col.bounds.max.x - col.bounds.min.x;
        Vector2 center = new Vector2(col.bounds.center.x, bound.top);
        
        //applying physics
        for (int i = 0; i < quality; i++) {
            if (PointIncideCircle(vertices[i], center, radius)) {
                velocities[i] = force;
            }
        }
    }

    bool PointIncideCircle(Vector2 point, Vector2 center, float radius) {
        return Vector2.Distance(point, center) < radius;
    }

    public void GenerateMesh() {
        float range = (bound.right - bound.left) / (quality - 1);
        vertices = new Vector3[quality * 2];
        
        //generate vertices
        for (int i = 0; i < quality; i++) {
            vertices[i] = new Vector3(bound.left + (i * range), bound.top, 0);
        }

        for (int i = 0; i < quality; i++) {
            vertices[i + quality] = new Vector2(bound.left + (i * range), bound.bottom
            );
        }
        
        //generate tris
        int[] template = new int[6];
        template[0] = quality;
        template[1] = 0;
        template[2] = quality + 1;
        template[3] = 0;
        template[4] = 1;
        template[5] = quality + 1;

        int maker = 0;
        int[] tris = new int[((quality - 1) * 2) * 3];
        for (int i = 0; i < tris.Length; i++) {
            tris[i] = template[maker++]++;
            if (maker >= 6) maker = 0;
        }
        
        //generate mesh
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        if (waterMaterial) meshRenderer.sharedMaterial = waterMaterial;
        meshRenderer.sortingOrder = 10000;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        //set up mesh
        meshFilter.mesh = mesh;
    }

    private void SetBoxCollider2D() {
        BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DynamicWater))]
public class DynamicWaterEditor : Editor {
    public override void OnInspectorGUI() {
        var me = target as DynamicWater;
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Generate Mesh")) {
            me.GenerateMesh();
        }
    }
}

#endif

﻿// StableFluids - A GPU implementation of Jos Stam's Stable Fluids on Unity
// https://github.com/keijiro/StableFluids

using UnityEngine;

namespace StableFluids
{
    public class Fluid : MonoBehaviour
    {
        #region Editable attributes

        [SerializeField] private Vector2 direction;
        [SerializeField] int _resolution = 512;
        [SerializeField] float _viscosity = 1e-6f;
        public float _force = 300;
        [SerializeField] float _exponent = 200;
        [SerializeField] Texture2D _initial;

        [SerializeField] private Color color;

        #endregion

        #region Internal resources

        [SerializeField] private GameObject cube;
        [SerializeField] ComputeShader _compute;
        [SerializeField] Shader _shader;
        [SerializeField] private TestFluidTouch testFluidTouch;

        #endregion

        #region Private members

        Material _shaderSheet;
        Vector2 _previousInput;

        static class Kernels
        {
            public const int Advect = 0;
            public const int Force = 1;
            public const int PSetup = 2;
            public const int PFinish = 3;
            public const int Jacobi1 = 4;
            public const int Jacobi2 = 5;
        }

        int ThreadCountX { get { return (_resolution                                + 7) / 8; } }
        int ThreadCountY { get { return (_resolution * Screen.height / Screen.width + 7) / 8; } }

        int ResolutionX { get { return ThreadCountX * 8; } }
        int ResolutionY { get { return ThreadCountY * 8; } }

        // Vector field buffers
        static class VFB
        {
            public static RenderTexture V1;
            public static RenderTexture V2;
            public static RenderTexture V3;
            public static RenderTexture P1;
            public static RenderTexture P2;
        }

        // Color buffers (for double buffering)
        RenderTexture _colorRT1;
        RenderTexture _colorRT2;

        RenderTexture AllocateBuffer(int componentCount, int width = 0, int height = 0)
        {
            var format = RenderTextureFormat.ARGBHalf;
            if (componentCount == 1) format = RenderTextureFormat.RHalf;
            if (componentCount == 2) format = RenderTextureFormat.RGHalf;

            if (width  == 0) width  = ResolutionX;
            if (height == 0) height = ResolutionY;

            var rt = new RenderTexture(width, height, 0, format);
            rt.enableRandomWrite = true;
            rt.Create();
            return rt;
        }

        #endregion

        #region MonoBehaviour implementation

        void OnValidate()
        {
            _resolution = Mathf.Max(_resolution, 8);
        }

        void Start()
        {
            _shaderSheet = new Material(_shader);

            VFB.V1 = AllocateBuffer(2);
            VFB.V2 = AllocateBuffer(2);
            VFB.V3 = AllocateBuffer(2);
            VFB.P1 = AllocateBuffer(1);
            VFB.P2 = AllocateBuffer(1);

            _colorRT1 = AllocateBuffer(4, Screen.width, Screen.height);
            _colorRT2 = AllocateBuffer(4, Screen.width, Screen.height);

            _shaderSheet.SetColor("_Color", color);
            
            Graphics.Blit(_initial, _colorRT1);
            var mat = cube.GetComponent<MeshRenderer>().material = _shaderSheet;
            mat.mainTexture = _colorRT2;

#if UNITY_IOS
            Application.targetFrameRate = 60;
#endif
        }

        public void ChangeColor(Color _color) {
            color = _color;
            _shaderSheet.SetColor("_Color", color);
        }

        void OnDestroy()
        {
            Destroy(_shaderSheet);

            Destroy(VFB.V1);
            Destroy(VFB.V2);
            Destroy(VFB.V3);
            Destroy(VFB.P1);
            Destroy(VFB.P2);

            Destroy(_colorRT1);
            Destroy(_colorRT2);
        }

        public bool _state;
        private float time;
        public float workTime;
        void Update() {
            // time += Time.deltaTime;
            // if (time >= workTime) {
            //     _state = true;
            //     time = 0;
            // }
            // if (_state) {
            //     _compute.SetVector("ForceVector", Vector4.zero);
            //     return;
            // }
            var dt = Time.deltaTime;
            var dx = 1.0f / ResolutionY;

            // Input point
            // var input = new Vector2(
            //     (Input.mousePosition.x - Screen.width  * 0.5f) / Screen.height,
            //     (Input.mousePosition.y - Screen.height * 0.5f) / Screen.height
            // );

            // var input = new Vector2(0.1f, 0.1f);

            // Common variables
            _compute.SetFloat("Time", Time.time);
            _compute.SetFloat("DeltaTime", dt);

            // Advection
            _compute.SetTexture(Kernels.Advect, "U_in", VFB.V1);
            _compute.SetTexture(Kernels.Advect, "W_out", VFB.V2);
            _compute.Dispatch(Kernels.Advect, ThreadCountX, ThreadCountY, 1);

            // Diffuse setup
            var dif_alpha = dx * dx / (_viscosity * dt);
            _compute.SetFloat("Alpha", dif_alpha);
            _compute.SetFloat("Beta", 4 + dif_alpha);
            Graphics.CopyTexture(VFB.V2, VFB.V1);
            _compute.SetTexture(Kernels.Jacobi2, "B2_in", VFB.V1);

            // Jacobi iteration
            for (var i = 0; i < 20; i++)
            {
                _compute.SetTexture(Kernels.Jacobi2, "X2_in", VFB.V2);
                _compute.SetTexture(Kernels.Jacobi2, "X2_out", VFB.V3);
                _compute.Dispatch(Kernels.Jacobi2, ThreadCountX, ThreadCountY, 1);

                _compute.SetTexture(Kernels.Jacobi2, "X2_in", VFB.V3);
                _compute.SetTexture(Kernels.Jacobi2, "X2_out", VFB.V2);
                _compute.Dispatch(Kernels.Jacobi2, ThreadCountX, ThreadCountY, 1);
            }
            
            float diffY = -Vector3.Dot(Vector3.up, transform.up.normalized);
            float diffX = -Vector3.Dot(Vector3.left, transform.up.normalized);

            
            direction = new  Vector2(diffX, diffY).normalized;
            Debug.Log(direction);
            
            Vector2 force;
            if (!_state) {
                var scaleY = cube.transform.localScale.y;
                var scaleX = cube.transform.localScale.x;
                
                //2.5 == 0.5  2.5 == 100% -2.5 == 0% 0.5 == 100% -0.5 == 0%
                force = testFluidTouch.point;
                
                var center = cube.transform.position;
                if (force.x < center.x) {
                    force.x = (testFluidTouch.point.x - center.x) / (scaleX / 2f);
                    force.y = (testFluidTouch.point.y - center.y) / scaleY;
                } else {
                    force.x = (testFluidTouch.lastPoint.x - center.x) / (scaleX / 2f);
                    force.y = (testFluidTouch.lastPoint.y - center.y) / scaleY;
                }
            } else { 
                force = Vector2.zero;
                _force = 0f;
            }

            // Add external force
            _compute.SetVector("ForceOrigin", force);
            _compute.SetFloat("ForceExponent", _exponent);
            _compute.SetTexture(Kernels.Force, "W_in", VFB.V2);
            _compute.SetTexture(Kernels.Force, "W_out", VFB.V3);

            _compute.SetVector("ForceVector", direction * _force);

            _compute.Dispatch(Kernels.Force, ThreadCountX, ThreadCountY, 1);

            // Projection setup
            _compute.SetTexture(Kernels.PSetup, "W_in", VFB.V3);
            _compute.SetTexture(Kernels.PSetup, "DivW_out", VFB.V2);
            _compute.SetTexture(Kernels.PSetup, "P_out", VFB.P1);
            _compute.Dispatch(Kernels.PSetup, ThreadCountX, ThreadCountY, 1);

            // Jacobi iteration
            _compute.SetFloat("Alpha", -dx * dx);
            _compute.SetFloat("Beta", 4);
            _compute.SetTexture(Kernels.Jacobi1, "B1_in", VFB.V2);

            for (var i = 0; i < 20; i++)
            {
                _compute.SetTexture(Kernels.Jacobi1, "X1_in", VFB.P1);
                _compute.SetTexture(Kernels.Jacobi1, "X1_out", VFB.P2);
                _compute.Dispatch(Kernels.Jacobi1, ThreadCountX, ThreadCountY, 1);

                _compute.SetTexture(Kernels.Jacobi1, "X1_in", VFB.P2);
                _compute.SetTexture(Kernels.Jacobi1, "X1_out", VFB.P1);
                _compute.Dispatch(Kernels.Jacobi1, ThreadCountX, ThreadCountY, 1);
            }

            // Projection finish
            _compute.SetTexture(Kernels.PFinish, "W_in", VFB.V3);
            _compute.SetTexture(Kernels.PFinish, "P_in", VFB.P1);
            _compute.SetTexture(Kernels.PFinish, "U_out", VFB.V1);
            _compute.Dispatch(Kernels.PFinish, ThreadCountX, ThreadCountY, 1);

            // Apply the velocity field to the color buffer.
            var offs = Vector2.one * (_state ? float.PositiveInfinity : 0);
            _shaderSheet.SetVector("_ForceOrigin", force + offs);
            _shaderSheet.SetFloat("_ForceExponent", _exponent);
            _shaderSheet.SetTexture("_VelocityField", VFB.V1);
            Graphics.Blit(_colorRT1, _colorRT2, _shaderSheet, 0);

            // Swap the color buffers.
            var temp = _colorRT1;
            _colorRT1 = _colorRT2;
            _colorRT2 = temp;

            // _previousInput = input;
        }

        // void OnRenderImage(RenderTexture source, RenderTexture destination) {
        //     Graphics.Blit(_colorRT1, destination, _shaderSheet, 1);
        // }

        #endregion
    }
}

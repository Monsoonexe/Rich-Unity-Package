using UnityEngine;

namespace RichPackage.Cameras
{
    /// <summary>
    /// Add pillars or gutters to screen.
    /// Pro tip: add a second camera (depth -2, clear flags solid color) to choose the gutter color.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraResolution : RichMonoBehaviour
    {
        public float targetScreenWidth = 16.0f;
        public float targetScreenHeight = 10.0f;

        private int screenWidth = 0;
        private int screenHeight = 0;

        private Camera mainCamera;

        protected override void Awake()
        {
            base.Awake();
            mainCamera = GetComponent<Camera>();
        }

        private void Start()
        {
            RescaleCamera();
        }

        //private void Update()
        //{
        //    RescaleCamera();
        //}

        //make edges black
        //void OnPreCull()
        //{
        //    //if (Application.isEditor) return;
        //    Rect wp = mainCamera.rect;
        //    Rect nr = new Rect(0, 0, 1, 1);

        //    mainCamera.rect = nr;
        //    GL.Clear(true, true, Color.black);

        //    mainCamera.rect = wp;
        //}

        public void RescaleCamera()
        {
            if (Screen.width == screenWidth && Screen.height == screenHeight) return;

            screenWidth = Screen.width;
            screenHeight = Screen.height;

            float targetaspect = targetScreenWidth / targetScreenHeight;
            float windowaspect = (float)screenWidth / (float)screenHeight;
            float scaleheight = windowaspect / targetaspect;
            Rect rect = mainCamera.rect;

            if (scaleheight < 1.0f)
            {
                rect.width = 1.0f;
                rect.height = scaleheight;
                rect.x = 0;
                rect.y = (1.0f - scaleheight) / 2.0f;
            }
            else // add pillarbox
            {
                float scalewidth = 1.0f / scaleheight;

                rect.width = scalewidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;
            }

            mainCamera.rect = rect;
        }
    }
}

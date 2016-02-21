using UnityEngine;

namespace Sagan.Framework {

    public class Camera : SaganObject {

        public UnityEngine.Camera camera;

        public float hFOV {
            get;
            private set;
        }

        public float vFOV {
            get {
                return this.camera.fieldOfView;
            }

            set {
                this.camera.fieldOfView = value;
                this.UpdateFOV();
            }
        }

        public int viewportWidth {
            get {
                return this.camera.pixelWidth;
            }
        }

        public float perspectiveScalingFactor {
            get {
                return this.viewportWidth / (2 * Mathf.Tan(Mathf.Deg2Rad * this.hFOV * 0.5f));
            }
        }

        public Camera(UnityEngine.Camera camera) : base("Camera") {
            this.camera = camera;
            this.gameObject = camera.gameObject;
            this.UpdateFOV();
        }

        public void UpdateFOV() {
            var radVFOV = Mathf.Deg2Rad * this.camera.fieldOfView;
            var radHFOV = 2 * Mathf.Atan( Mathf.Tan( radVFOV * 0.5f ) * this.camera.aspect);

            this.hFOV = Mathf.Rad2Deg * radHFOV;

        }

    }
}
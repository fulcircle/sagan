using UnityEngine;

namespace Sagan.Framework {

    public class Camera : SaganObject {

        public UnityEngine.Camera camera {
            get {
                return gameObject.GetComponent<UnityEngine.Camera>();
            }
        }

        public static UnityEngine.Camera mainCamera {
            get {
                return UnityEngine.Camera.main;
            }
        }

        public Camera() : base() {
            gameObject.AddComponent<UnityEngine.Camera>();
        }

    }
}
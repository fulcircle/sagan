using UnityEngine;

namespace Sagan.Terrain.Behavior {

    public class SaganBehavior : MonoBehaviour {

        private Sagan.Planet.Planet planet;

        void Start() {
            var cam = new Sagan.Framework.Camera(UnityEngine.Camera.main);
            planet = new Sagan.Planet.Planet(cam, 10.0f, 3);
        }

        void Update() {
//            planet.Update();
        }
    }
}
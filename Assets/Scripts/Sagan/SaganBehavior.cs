using UnityEngine;

namespace Sagan.Terrain.Behavior {

    public class SaganBehavior : MonoBehaviour {

        private Sagan.Planet.Planet _planet;

        private Sagan.Framework.Camera _cam;

        public int depth = 1;

        public int radius = 10;


        void Start() {
            this._cam = new Sagan.Framework.Camera(UnityEngine.Camera.main);
            this._planet = new Sagan.Planet.Planet(this._cam, this.radius, this.depth);
        }

        void Update() {
            this._planet.Update();
        }
    }
}
using UnityEngine;

namespace Sagan.Terrain.Behavior {

    public class SaganBehavior : MonoBehaviour {

        private Sagan.Planet.Planet _planet;
        private Sagan.Terrain.Terrain _terrain;

        private Sagan.Framework.Camera _cam;

        public int depth = 1;

        public int radius = 10;

        void Start() {
            this._cam = new Sagan.Framework.Camera(UnityEngine.Camera.main);
            this._terrain = new Sagan.Terrain.Terrain(128, 6, this._cam);
            this._terrain.Precalculate();
            this._terrain.Create();
        }

        void Update() {
            this._terrain.Update();
        }
    }
}
using UnityEngine;

namespace Sagan.Terrain.Behavior {

    public class TerrainBehavior : MonoBehaviour {

        private Sagan.Terrain.Terrain terrain;

        void Start() {
            var cam = new Sagan.Framework.Camera(UnityEngine.Camera.main);
            terrain = new Sagan.Terrain.Terrain(10, 6, cam);
        }

        void Update() {
            terrain.Update();
        }
    }
}
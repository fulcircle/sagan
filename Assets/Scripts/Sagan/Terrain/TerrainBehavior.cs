using UnityEngine;

namespace Sagan.Terrain.Behavior {

    public class TerrainBehavior : MonoBehaviour {

        private Sagan.Terrain.Terrain terrain;

        void Start() {
            terrain = new Sagan.Terrain.Terrain(10, 6);
        }

        void Update() {
            terrain.Update();
        }
    }
}
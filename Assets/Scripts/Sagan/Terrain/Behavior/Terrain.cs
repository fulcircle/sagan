using UnityEngine;

namespace Sagan.Terrain.Behavior {

    public class Terrain : MonoBehaviour {

        void Start() {
            new Sagan.Terrain.Terrain(10, 5);
        }
    }
}
using UnityEngine;

namespace Sagan.Framework {

    public class SaganCube : SaganMesh {

        public SaganCube(string name="SaganCube") :
        base(GameObject.CreatePrimitive(PrimitiveType.Cube), name) {
        }
    }
}
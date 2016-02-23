using UnityEngine;

namespace Sagan.Framework {

    public class SaganCube : SaganMesh {

        public SaganCube(string name="") :
        base(GameObject.CreatePrimitive(PrimitiveType.Cube), "SaganCube") {
        }
    }
}
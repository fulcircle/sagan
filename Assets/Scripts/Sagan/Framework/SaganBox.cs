using UnityEngine;

namespace Sagan.Framework {

    public class SaganBox : SaganMesh {

        public SaganBox(string name="SaganBox") :
        base(GameObject.CreatePrimitive(PrimitiveType.Cube), name) {
        }

    }
}
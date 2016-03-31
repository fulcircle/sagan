using UnityEngine;

namespace Sagan.Terrain
{
    public class TessellationLodStrategy : ProlandLodStrategy {

        public override string shaderName {
            get { return "Sagan/Tessellation"; }
        }

        public TessellationLodStrategy(Sagan.Terrain.Terrain terrain) : base (terrain) {
        }
    }
}

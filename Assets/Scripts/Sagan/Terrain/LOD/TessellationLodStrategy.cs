using UnityEngine;

namespace Sagan.Terrain
{
    public class TessellationLodStrategy : ProlandLodStrategy {

        public override string shaderName {
            get { return "Sagan/Tessellation"; }
        }

        public TessellationLodStrategy(Sagan.Terrain.Terrain terrain) : base (terrain) {
        }

        public override Quad NewChildQuad(Quad parentQuad=null) {
            if (parentQuad == null) {
                return new TessellationQuad(0, this.terrain.size, this.terrain.heightMap);
            }
            else {
                return new TessellationQuad(parentQuad.LOD + 1, parentQuad.size * 0.5f, this.terrain.heightMap);
            }
        }

    }
    public class TessellationQuad : Quad {

        public float error { get; private set; }
        public TessellationQuad(int LOD, float size, HeightMap heightMap) :
            base(LOD, size, heightMap, subdivisions:1) {
        }
    }
}

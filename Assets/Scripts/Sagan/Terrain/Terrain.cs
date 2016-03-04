using Sagan.Framework;
using UnityEngine;
using Camera = Sagan.Framework.Camera;

namespace Sagan.Terrain {
    public class Terrain : SaganObject {
        private GameObject _parent;

        public ILodStrategy _lodStrategy { get; private set; }

        public int depth { get; private set; }
        public int size { get; private set; }

        private Camera cam;

        public HeightMap heightMap { get; private set; }

        private bool _boundingBoxesVisible = false;

        public bool showQuadBoundingBox  {
            get { return this._boundingBoxesVisible; }
            set {
                this._lodStrategy.quads.ForEach(q => q.boundingBoxVisible = value);
                this._boundingBoxesVisible = value;
            }
        }

        public Terrain(int size, int depth, Camera cam, ILodStrategy lodStrategy = null)
            : base(name: "SaganTerrain") {

            this.size = size;

            this.depth = depth;

            // Add +1 to width and height of heightmap so bilinear interpolation of quad can interpolate extra heightArray point beyond edge of quad
            this.heightMap = new HeightMap(size + 1);

            this.cam = cam;

            if (lodStrategy == null) {
                this._lodStrategy = new ProlandLodStrategy(this);
            }
        }


        public void Precalculate() {
            this._lodStrategy.Precalculate();
            this._lodStrategy.quads.ForEach(this.AddChild);
        }

        public void Create() {
            this._lodStrategy.Create();
        }

        public void Update() {
            this._lodStrategy.Update(this.cam);
        }

//        /// <summary>
//        /// Spherify all Quads with given radius.
//        /// This method assumes the vertices were already precalculated this Terrain via preCalculateVertices()
//        /// </summary>
//        /// <param name="radius">Radius of the sphere</param>
//        public void Spherify(float radius) {
//            // Create the rootQuad mesh so we can get it's bounding box
//            this.rootQuad.Create();
//            // Get the center of the rootQuad, this is essentially the center of the entire terrain square
//            var center = this.rootQuad.localBoundingBox.center;
//
//            // Set the center point to a length of radius below the rootQuad that will simulate the center of the planet sphere
//            center.y = -radius;
//
//            foreach (Quad quad in this._lodStrategy.quads) {
//                for (int i = 0; i < quad.verts.Count; i++) {
//                    // Get the quad's vertex relative to the parent terrain
//                    var vert = quad.verts[i] + quad.transform.localPosition;
//                    // Remove height component for now
//                    float height = vert.y;
//                    vert.y = 0;
//                    // Convert the vector to a unit from our simulated sphere center and then multiply by radius to spherify
//                    // See: http://ducttapeeinstein.com/mapping-a-cube-to-sphere-in-c-unity-3d-the-start-of-procedural-planet-generation/
//                    var spherizedVert = (vert - center).normalized*(radius + height);
//                    // Remove the radius height and parent vertex transform to get back the local coordinate again
//                    quad.verts[i] = (spherizedVert - new Vector3(0, radius, 0) - quad.transform.localPosition);
//                }
//            }
//        }
    }
}
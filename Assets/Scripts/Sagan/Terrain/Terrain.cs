using Sagan.Framework;
using System.Collections.Generic;
using UnityEngine;
using Camera = Sagan.Framework.Camera;

namespace Sagan.Terrain {

    public class Terrain : SaganObject {

        private GameObject _parent;

        public Quad rootQuad {
            get;
            private set;
        }

        private int levels;
        private Camera cam;
        public HeightMap heightMap {
            get;
            private set;
        }

        public Terrain(int terrainSize, int levels, Camera cam) : base(name: "SaganTerrain") {
            this.levels = levels;

            // Add +1 to width and height of heightmap so bilinear interpolation of quad can interpolate extra data point beyond edge of quad
            this.heightMap = new HeightMap(terrainSize + 1);

            this.rootQuad = new Quad(1, terrainSize, terrainSize, this.heightMap);

            this.cam = cam;

        }

        public void Update() {
            foreach (Quad q in this._children) {
                q.active = false;
            }
            this.ChunkedLOD(rootQuad, cam.perspectiveScalingFactor);
        }

        public void PrecalculateQuads() {
            this.GenerateQuadTree(this.rootQuad);
        }

        public void CreateQuads() {
            foreach(Quad q in this._children) {
                q.Create();
            }
        }

        void GenerateQuadTree(Quad parentQuad) {

            this.AddChild(parentQuad);

            parentQuad.PreCalculate();

            if (parentQuad.LOD == this.levels) {
                parentQuad.isLeaf = true;
                return;
            }

            parentQuad.isLeaf = false;

            var currX = parentQuad.transform.localPosition.x;
            var currY = parentQuad.transform.localPosition.y;
            var currZ = parentQuad.transform.localPosition.z;

            var stride = parentQuad.size * 0.5f;

            for (int i=0; i < 4; i++) {
                var childQuad = new Quad(parentQuad.LOD + 1,
                        parentQuad.size * 0.5f,
                        parentQuad.size * 0.5f,
                        this.heightMap);

                parentQuad.children.Add(childQuad);

                childQuad.transform.localPosition = new Vector3(currX, currY, currZ);

                this.GenerateQuadTree(childQuad);

                currX = currX + stride;
                if ((currX - parentQuad.transform.localPosition.x) >= parentQuad.size) {
                    currX = parentQuad.transform.localPosition.x;
                    currZ = currZ + stride;
                }


            }
        }

        /// <summary>
        /// Spherify all Quads with given radius.
        /// This method assumes the vertices were already precalculated this Terrain via preCalculateVertices()
        /// </summary>
        /// <param name="radius">Radius of the sphere</param>
        public void Spherify(float radius) {

            // Create the rootQuad mesh so we can get it's bounding box
            this.rootQuad.Create();
            // Get the center of the rootQuad, this is essentially the center of the entire terrain square
            var center = this.rootQuad.localBoundingBox.center;

            // Set the center point to a length of radius below the rootQuad that will simulate the center of the planet sphere
            center.y = -radius;

            foreach (Quad quad in this._children) {
                for (int i = 0; i < quad.verts.Count; i++) {
                    // Get the quad's vertex relative to the parent terrain
                    var vert = quad.verts[i] + quad.transform.localPosition;
                    // Remove height component for now
                    float height = vert.y;
                    vert.y = 0;
                    // Convert the vector to a unit from our simulated sphere center and then multiply by radius to spherify
                    // See: http://ducttapeeinstein.com/mapping-a-cube-to-sphere-in-c-unity-3d-the-start-of-procedural-planet-generation/
                    var spherizedVert = (vert - center).normalized * (radius + height);
                    // Remove the parent vertex transform to get back the local coordinate again
                    quad.verts[i] = (spherizedVert - quad.transform.localPosition);
                }
            }
        }

        // Chunked LOD implementation: http://tulrich.com/geekstuff/sig-notes.pdf
        // TODO: Optimizations
        // Store coordinates of bounding boxes and exclude branches in quadtree that are out of range
        void ChunkedLOD(Quad quad, float scalingFactor=1.0f) {

            var camPos = this.cam.transform.position;
            Vector3 closestPoint = quad.boundingBox.ClosestPoint(camPos);
            float distance = Vector3.Distance(closestPoint, camPos);

            // Screen space error
            float rho = (quad.error / distance ) * scalingFactor;

            // Largest allowable screen error
            float tau = 120;

            if (quad.isLeaf || rho <= tau) {
                quad.active = true;
            } else {
                // TODO: When we implement excluding of whole subbranches, we'll have to turn off visibility for all chunks in that branch
                foreach (Quad q in quad.children) {
                    this.ChunkedLOD(q, scalingFactor);
                }
            }
        }

    }

}
using Sagan.Framework;
using System.Collections.Generic;
using UnityEngine;
using Camera = Sagan.Framework.Camera;

namespace Sagan.Terrain {

    public class Terrain {

        private List<Quad> _quads = new List<Quad>();

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

        public Terrain(int terrainSize, int levels, Camera cam, GameObject parent) {
            this.levels = levels;

            // Add +1 to width and height of heightmap so bilinear interpolation of quad can interpolate extra data point beyond edge of quad
            this.heightMap = new HeightMap(terrainSize + 1);

            this._parent = parent;

            this.rootQuad = new Quad(1, 10, 10, this.heightMap);


            this.cam = cam;

            this.GenerateQuadTree(rootQuad);




        }

        public void Update() {
            foreach (Quad q in _quads) {
                q.active = false;
            }
            this.ChunkedLOD(rootQuad, cam.perspectiveScalingFactor);
        }

        void GenerateQuadTree(Quad parentQuad) {

            parentQuad.transform.parent = this._parent.transform;

            parentQuad.Generate();

            this._quads.Add(parentQuad);

            if (parentQuad.LOD == this.levels) {
                parentQuad.isLeaf = true;
                return;
            }

            parentQuad.isLeaf = false;

            var currX = parentQuad.transform.position.x;
            var currY = parentQuad.transform.position.y;
            var currZ = parentQuad.transform.position.z;

            var stride = parentQuad.size * 0.5f;

            for (int i=0; i < 4; i++) {
                var childQuad = new Quad(parentQuad.LOD + 1,
                        parentQuad.size * 0.5f,
                        parentQuad.size * 0.5f,
                        this.heightMap);

                parentQuad.children.Add(childQuad);

                childQuad.transform.position = new Vector3(currX, currY, currZ);

                GenerateQuadTree(childQuad);

                currX = currX + stride;
                if ((currX - parentQuad.transform.position.x) >= parentQuad.size) {
                    currX = parentQuad.transform.position.x;
                    currZ = currZ + stride;
                }


            }
        }

        // Chunked LOD implementation: http://tulrich.com/geekstuff/sig-notes.pdf
        // TODO: Optimizations
        // Store coordinates of bounding boxes and exclude branches in quadtree that are out of range
        void ChunkedLOD(Quad quad, float scalingFactor=1.0f) {

            // TODO: Get closest point from camera, not origin
            quad.mesh.RecalculateBounds();
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
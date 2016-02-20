using System.Collections.Generic;
using UnityEngine;

namespace Sagan.Terrain {

    public class Terrain {

        private List<Quad> _quads = new List<Quad>();

        private Quad rootQuad;
        private int levels;

        public Terrain(int terrainSize, int levels) {
            this.levels = levels;

            rootQuad = new Quad(1, 10, 10);
            GenerateQuadTree(rootQuad);

        }

        void GenerateQuadTree(Quad parentQuad) {

            parentQuad.Generate();

            if (parentQuad.LOD == this.levels) {
                parentQuad.isLeaf = true;
                return;
            }

            var currX = parentQuad.transform.position.x;
            var currY = parentQuad.transform.position.y;
            var currZ = parentQuad.transform.position.z;

            var stride = parentQuad.size * 0.5f;

            for (int i=0; i < 4; i++) {
                var childQuad = new Quad(parentQuad.LOD + 1, parentQuad.size * 0.5f, parentQuad.size * 0.5f);

                parentQuad.children.Add(childQuad);

                childQuad.transform.position = new Vector3(currX, currY, currZ);
                Debug.Log(childQuad.transform.position);

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
        void chunkedLOD(Quad quad, float scalingFactor=1) {

            // TODO: Need to get distance to nearest face, not centroid
            // TODO: Get closest point from camera, not origin
            Vector3 closestPoint = quad.mesh.bounds.ClosestPoint(new Vector3(0,0,0));
            float distance = Vector3.Distance(closestPoint, new Vector3(0,0,0));

            // Screen space error
            float rho = (quad.error / distance ) * scalingFactor;

            // distance = 0 so screenspace error should be 0
            //        if (!isFinite(rho)) {
            //            rho = 0;
            //        }
            // Largest allowable screen error
            float tau = 45;

            if (quad.isLeaf || rho <= tau) {
                quad.active = true;
            } else {
                // TODO: When we implement excluding of whole subbranches, we'll have to turn off visibility for all chunks in that branch
                foreach (Quad q in quad.children) {
                    this.chunkedLOD(q, scalingFactor);
                }
            }
        }

    }

}